using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Shared.HubContract;
using Microsoft.AspNetCore.SignalR;

namespace Apollon.Mud.Server.Domain.Implementations.Chat
{
    /// <inheritdoc cref="IChatService"/>
    public class ChatService : IChatService
    {
        private IGameDbService GameDbService { get; }

        private IConnectionService ConnectionService { get; }

        private IHubContext<ChatHub, IClientChatHubContract> ChatHubContext { get; }

        /// <summary>
        /// Initializes a new instance of ChatService
        /// </summary>
        /// <param name="gameDbService"></param>
        /// <param name="connectionService"></param>
        /// <param name="chatHubContext"></param>
        public ChatService(IGameDbService gameDbService, IConnectionService connectionService, IHubContext<ChatHub, IClientChatHubContract> chatHubContext)
        {
            GameDbService = gameDbService;
            ConnectionService = connectionService;
            ChatHubContext = chatHubContext;
        }

        /// <inheritdoc cref="IChatService.PostRoomMessage"/>
        public async Task PostRoomMessage(Guid dungeonId, Guid avatarId, string message)      // TODO: In UML anpassen
        {
            Avatar senderAvatar;

            senderAvatar = await GameDbService.Get<Avatar>(avatarId);

            if (senderAvatar is null) return;

            var recipientChatConnectionIds = new List<string>();
            foreach (var avatar in senderAvatar.CurrentRoom.Avatars)
            {
                Connection recipientConnection;
                if (avatar.Status == Status.Approved && 
                    (recipientConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id)) is not null)
                    recipientChatConnectionIds.Add(recipientConnection.ChatConnectionId);
            }

            await ChatHubContext.Clients.Clients(recipientChatConnectionIds).ReceiveChatMessage(senderAvatar.Name, message);
        }

        /// <inheritdoc cref="IChatService.PostWhisperMessage"/>
        public async Task PostWhisperMessage(Guid dungeonId, Guid? senderAvatarId, string recipientName, string message)      // TODO: In UML anpassen
        {
            Connection recipientConnection;
            string senderName;

            if (senderAvatarId is null)
            {
                if ((recipientConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeonId)) is null) return;
                senderName = "Dungeon Master";
            }
            else
            {
                Avatar recipientAvatar;
                try
                {
                    var avatars = await GameDbService.GetAll<Avatar>();
                    recipientAvatar = avatars.SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
                }
                catch (InvalidOperationException)
                {
                    return;
                }

                var senderAvatar = await GameDbService.Get<Avatar>(senderAvatarId.Value);

                if (senderAvatar is null || recipientAvatar is null ||
                    (recipientConnection = ConnectionService.GetConnectionByAvatarId(recipientAvatar.Id)) is null)
                    return;

                senderName = senderAvatar.Name;
            }

            await ChatHubContext.Clients.Client(recipientConnection.ChatConnectionId)
                .ReceiveChatMessage(senderName, message);
        }

        /// <inheritdoc cref="IChatService.PostGlobalMessage"/>
        public async Task PostGlobalMessage(Guid dungeonId, string message)
        {
            var avatars = await GameDbService.GetAll<Avatar>();
            var recipientAvatars = avatars.Where(x => x.Dungeon.Id == dungeonId && x.Status == Status.Approved).ToArray();

            var recipientChatConnectionIds = new List<string>();
            foreach (var avatar in recipientAvatars)
            {
                Connection recipientConnection;
                if ((recipientConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id)) is not null)
                    recipientChatConnectionIds.Add(recipientConnection.ChatConnectionId);
            }

            await ChatHubContext.Clients.Clients(recipientChatConnectionIds).ReceiveChatMessage("Dungeon Master", message);
        }
    }
}
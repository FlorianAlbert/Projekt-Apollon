using System;
using System.Collections.Generic;
using System.Linq;
using Apollon.Mud.Server.Domain.Interfaces.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Outbound.Hubs.Implementations;
using Apollon.Mud.Shared.HubContract;
using Microsoft.AspNetCore.SignalR;

namespace Apollon.Mud.Server.Domain.Implementations.Chat
{
    public class ChatService : IChatService
    {
        private IGameDBService GameDbService { get; }

        private IConnectionService ConnectionService { get; }

        private IHubContext<ChatHub, IClientChatHubContract> ChatHubContext { get; }

        public ChatService(IGameDBService gameDbService, IConnectionService connectionService, IHubContext<ChatHub, IClientChatHubContract> chatHubContext)
        {
            GameDbService = gameDbService;
            ConnectionService = connectionService;
            ChatHubContext = chatHubContext;
        }

        public void PostRoomMessage(Guid dungeonId, string senderName, string message)
        {
            IAvatar senderAvatar;
            try
            {
                senderAvatar = GameDbService.GetAll<IAvatar>()
                    .SingleOrDefault(x => x.Name == senderName && x.Dungeon.Id == dungeonId);
            }
            catch (InvalidOperationException)
            {
                return;
            }

            if (senderAvatar is null) return;

            var recipientChatConnectionIds = new List<string>();
            foreach (var inspectable in senderAvatar.CurrentRoom.Inspectables)
            {
                Connection recipientConnection;
                if (inspectable is IAvatar avatar && (recipientConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id)) is not null)
                    recipientChatConnectionIds.Add(recipientConnection.ChatConnectionId);
            }

            ChatHubContext.Clients.Clients(recipientChatConnectionIds).ReceiveChatMessage(senderName, message);
        }

        public void PostWhisperMessage(Guid dungeonId, string senderName, string recipientName, string message)
        {
            Connection recipientConnection;

            if (senderName == "Dungeon Master")
            {
                if ((recipientConnection = ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeonId)) is null) return;
            }
            else
            {
                IAvatar recipientAvatar;
                try
                {
                    recipientAvatar = GameDbService.GetAll<IAvatar>()
                        .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId);
                }
                catch (InvalidOperationException)
                {
                    return;
                }

                if (recipientAvatar is null ||
                    (recipientConnection = ConnectionService.GetConnectionByAvatarId(recipientAvatar.Id)) is null)
                    return;
            }

            ChatHubContext.Clients.Client(recipientConnection.ChatConnectionId)
                .ReceiveChatMessage(senderName, message);
        }

        public void PostGlobalMessage(Guid dungeonId, string message)
        {
            var recipientAvatars = GameDbService.GetAll<IAvatar>()
                .Where(x => x.Dungeon.Id == dungeonId).ToArray();

            var recipientChatConnectionIds = new List<string>();
            foreach (var avatar in recipientAvatars)
            {
                Connection recipientConnection;
                if ((recipientConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id)) is not null)
                    recipientChatConnectionIds.Add(recipientConnection.ChatConnectionId);
            }

            ChatHubContext.Clients.Clients(recipientChatConnectionIds).ReceiveChatMessage("Dungeon Master", message);
        }
    }
}
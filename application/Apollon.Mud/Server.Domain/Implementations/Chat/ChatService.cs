﻿using System;
using System.Collections.Generic;
using System.Linq;
using Apollon.Mud.Server.Domain.Interfaces.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Outbound.Hubs;
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

        public void PostRoomMessage(Guid dungeonId, Guid avatarId, string message)      // TODO: In UML anpassen
        {
            IAvatar senderAvatar;
            try
            {
                senderAvatar = GameDbService.Get<IAvatar>(avatarId);
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
                if (inspectable is IAvatar avatar && avatar.Status == Status.Approved && (recipientConnection = ConnectionService.GetConnectionByAvatarId(avatar.Id)) is not null)
                    recipientChatConnectionIds.Add(recipientConnection.ChatConnectionId);
            }

            ChatHubContext.Clients.Clients(recipientChatConnectionIds).ReceiveChatMessage(senderAvatar.Name, message);
        }

        public void PostWhisperMessage(Guid dungeonId, Guid? senderAvatarId, string recipientName, string message)      // TODO: In UML anpassen
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
                IAvatar recipientAvatar;
                try
                {
                    recipientAvatar = GameDbService.GetAll<IAvatar>()
                        .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
                }
                catch (InvalidOperationException)
                {
                    return;
                }

                var senderAvatar = GameDbService.Get<IAvatar>(senderAvatarId.Value);

                if (senderAvatar is null ||recipientAvatar is null ||
                    (recipientConnection = ConnectionService.GetConnectionByAvatarId(recipientAvatar.Id)) is null)
                    return;

                senderName = senderAvatar.Name;
            }

            ChatHubContext.Clients.Client(recipientConnection.ChatConnectionId)
                .ReceiveChatMessage(senderName, message);
        }

        public void PostGlobalMessage(Guid dungeonId, string message)
        {
            var recipientAvatars = GameDbService.GetAll<IAvatar>()
                .Where(x => x.Dungeon.Id == dungeonId && x.Status == Status.Approved).ToArray();

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
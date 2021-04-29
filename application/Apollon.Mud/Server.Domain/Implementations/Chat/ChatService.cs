using System;
using Apollon.Mud.Server.Domain.Interfaces.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Outbound.Hubs.Implementations;
using Microsoft.AspNetCore.SignalR;

namespace Apollon.Mud.Server.Domain.Implementations.Chat
{
    public class ChatService : IChatService
    {
        private IConnectionService ConnectionService { get; }

        private IHubContext<ChatHub> ChatHubContext { get; }

        public ChatService()
        {

        }

        public void PostRoomMessage(Guid dungeonId, string senderName, string message)
        {
            throw new NotImplementedException();
        }

        public void PostWhisperMessage(Guid dungeonId, string senderName, string recipientName, string message)
        {
            throw new NotImplementedException();
        }

        public void PostGlobalMessage(Guid dungeonId, string message)
        {
            throw new NotImplementedException();
        }
    }
}
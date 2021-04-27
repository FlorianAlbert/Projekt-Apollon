using System.Collections.Generic;
using Apollon.Mud.Server.Hubs.Interfaces;
using Apollon.Mud.Shared.HubContract;
using Microsoft.AspNetCore.SignalR;

namespace Apollon.Mud.Server.Hubs.Implementations
{
    public class ChatHub : Hub<IClientChatHubContract>, IChatHub
    {
        public void SendSingleChatMessage(string senderName, string connectionId, string message)
        {
            Clients.Client(connectionId).ReceiveChatMessage(senderName, message);
        }

        public void SendMultiChatMessage(string senderName, IEnumerable<string> connectionIds, string message)
        {
            Clients.Clients(connectionIds).ReceiveChatMessage(senderName, message);
        }
    }
}
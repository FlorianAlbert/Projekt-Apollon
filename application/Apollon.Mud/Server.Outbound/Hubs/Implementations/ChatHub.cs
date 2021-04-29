using System.Collections.Generic;
using Apollon.Mud.Server.Outbound.Hubs.Interfaces;
using Apollon.Mud.Shared.HubContract;
using Microsoft.AspNetCore.SignalR;

namespace Apollon.Mud.Server.Outbound.Hubs.Implementations
{
    public class ChatHub : Hub<IClientChatHubContract>, IChatHub
    {
        public void SendSingleChatMessage(string senderName, string connectionId, string message)
        {
            Clients.Client(connectionId).ReceiveChatMessage(senderName, message);
        }

        public void SendMultiChatMessage(string senderName, IEnumerable<string> connectionIds, string message)
        {
            Clients.Clients(connectionIds as IReadOnlyList<string>).ReceiveChatMessage(senderName, message);
        }
    }
}
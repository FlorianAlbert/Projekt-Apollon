using System.Collections.Generic;

namespace Apollon.Mud.Server.Hubs.Interfaces
{
    public interface IChatHub
    {
        void SendSingleChatMessage(string senderName, string connectionId, string message);

        void SendMultiChatMessage(string senderName, IEnumerable<string> connectionIds, string message);
    }
}
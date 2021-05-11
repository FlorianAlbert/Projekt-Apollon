using System.Threading.Tasks;

namespace Apollon.Mud.Shared.HubContract
{
    /// <summary>
    /// TODO Abhilfe
    /// </summary>
    public interface IClientChatHubContract
    {
        /// <summary>
        /// Receive a specific message from a specific sender.
        /// </summary>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        Task ReceiveChatMessage(string senderName, string message);
    }
}
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
        void ReceiveChatMessage(string senderName, string message);
    }
}
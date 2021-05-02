namespace Apollon.Mud.Shared.HubContract
{
    /// <summary>
    /// ToDo
    /// </summary>
    public interface IClientChatHubContract
    {
        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="senderName"></param>
        /// <param name="message"></param>
        void ReceiveChatMessage(string senderName, string message);
    }
}
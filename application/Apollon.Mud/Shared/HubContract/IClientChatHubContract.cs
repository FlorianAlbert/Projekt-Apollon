namespace Apollon.Mud.Shared.HubContract
{
    public interface IClientChatHubContract
    {
        void ReceiveChatMessage(string senderName, string message);
    }
}
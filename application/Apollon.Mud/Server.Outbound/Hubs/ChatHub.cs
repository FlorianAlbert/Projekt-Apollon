using Apollon.Mud.Shared.HubContract;
using Microsoft.AspNetCore.SignalR;

namespace Apollon.Mud.Server.Outbound.Hubs
{
    public class ChatHub : Hub<IClientChatHubContract>
    {

    }
}
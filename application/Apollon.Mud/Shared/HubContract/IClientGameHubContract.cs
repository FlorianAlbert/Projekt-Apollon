using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Shared.Dungeon.Avatar;
using Apollon.Mud.Shared.Game.DungeonMaster;

namespace Apollon.Mud.Shared.HubContract
{
    /// <summary>
    /// ToDo Abhilfe
    /// </summary>
    public interface IClientGameHubContract
    {
        /// <summary>
        /// Receive a specific message.
        /// </summary>
        /// <param name="message">The incoming game message</param>
        /// <returns></returns>
        Task ReceiveGameMessage(string message);

        /// <summary>
        /// Method for DungeonMaster to receive
        /// a SpecialAction Request
        /// </summary>
        /// <param name="dungeonMasterRequestDto">The incoming request</param>
        /// <returns></returns>
        Task ReceiveRequest(DungeonMasterRequestDto dungeonMasterRequestDto);

        /// <summary>
        /// Method for showing the avatar list, 
        /// which will list all avatars in the dungeon.
        /// </summary>
        /// <param name="avatars">The current list of Avatars</param>
        /// <returns></returns>
        Task ReceiveAvatarList(ICollection<AvatarDto> avatars);

        /// <summary>
        /// Method to list all the avatars the user
        /// can currently chat with
        /// </summary>
        /// <param name="chatPartners">The current list of Chat partners</param>
        /// <returns></returns>
        Task ReceiveChatPartnerList(ICollection<AvatarDto> chatPartners);

        /// <summary>
        /// Method to notify the client his avatar got kicked
        /// </summary>
        /// <returns></returns>
        Task NotifyKicked();
    }
}
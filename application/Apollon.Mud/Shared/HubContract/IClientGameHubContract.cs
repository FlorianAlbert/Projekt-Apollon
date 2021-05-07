using System.Collections.Generic;
using Apollon.Mud.Shared.Dungeon.Avatar;
using Apollon.Mud.Shared.Game.DungeonMaster;

namespace Apollon.Mud.Shared.HubContract
{
    /// <summary>
    /// ToDo
    /// </summary>
    public interface IClientGameHubContract
    {
        /// <summary>
        /// Receive a specific message.
        /// </summary>
        /// <param name="message"></param>
        void ReceiveGameMessage(string message);

        /// <summary>
        /// ToDo Abhilfe
        /// </summary>
        /// <param name="dungeonMasterRequestDto"></param>
        void ReceiveRequest(DungeonMasterRequestDto dungeonMasterRequestDto);

        /// <summary>
        /// Method for showing the avatar list, 
        /// which will list all avatars in the dungeon. ToDo Abhilfe
        /// </summary>
        /// <param name="avatars"></param>
        void ReceiveAvatarList(ICollection<AvatarDto> avatars);
    }
}
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
        /// ToDo
        /// </summary>
        /// <param name="message"></param>
        void ReceiveGameMessage(string message);

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="dungeonMasterRequestDto"></param>
        void ReceiveRequest(DungeonMasterRequestDto dungeonMasterRequestDto);

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="avatars"></param>
        void ReceiveAvatarList(ICollection<AvatarDto> avatars);
    }
}
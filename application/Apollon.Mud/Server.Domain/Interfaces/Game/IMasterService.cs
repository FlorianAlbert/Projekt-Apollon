using System;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Domain.Interfaces.Game
{
    /// <summary>
    /// Service to handle requests by DungeonMasters
    /// </summary>
    public interface IMasterService
    {
        /// <summary>
        /// Handles answered Requestable by DungeonMaster
        /// </summary>
        /// <param name="message">Message to forward to the player</param>
        /// <param name="avatarId">Id of the avatar that send the requestable</param>
        /// <param name="newHpValue">New Health Value of the avatar</param>
        /// <param name="dungeonId">DungeonId of the avatar that send the requestable</param>
        /// <returns></returns>
        Task ExecuteDungeonMasterRequestResponse(string message, Guid avatarId,
            int newHpValue, Guid dungeonId);

        /// <summary>
        /// Kicks a special avatar from the dungeon
        /// </summary>
        /// <param name="avatarId">Id of the avatar that gets kicked</param>
        /// <param name="dungeonId">DungeonId of the avatar that gets kicked</param>
        /// <returns></returns>
        Task KickAvatar(Guid avatarId, Guid dungeonId);

        /// <summary>
        /// Kicks all avatars of a dungeon
        /// </summary>
        /// <param name="dungeonId">DungeonId of the dungeon all avatars gets kicked of</param>
        /// <returns></returns>
        Task KickAllAvatars(Guid dungeonId);
    }
}
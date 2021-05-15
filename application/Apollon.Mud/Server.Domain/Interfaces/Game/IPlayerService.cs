using System;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Domain.Interfaces.Game
{
    /// <summary>
    /// Service class to handle player actions
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// Validates a command entered by a player
        /// </summary>
        /// <param name="avatarId">Id of the avatar that send the command</param>
        /// <param name="dungeonId">Id of the dungeon the avatar is in</param>
        /// <param name="message">Message the player entered</param>
        /// <returns></returns>
        Task ValidateCommand(Guid avatarId, Guid dungeonId, string message);
        
        Task NotifyAvatarLeftRoom(string avatarName, Guid dungeonId, Guid roomId);
        Task NotifyAvatarEnteredRoom(string avatarName, Guid dungeonId, Guid roomId);
        Task NotifyDungeonMasterEntering(Guid dungeonId);
        Task NotifyDungeonMasterLeaving(Guid dungeonId);
        Task NotifyAvatarEnteredDungeon(string avatarName, Guid dungeonId, Guid roomId);
        Task NotifyAvatarLeftDungeon(string avatarName, Guid dungeonId, Guid roomId);

        Task NotifyAvatarMovedToDefaultRoom(Guid avatarId, Guid dungeonId);

        Task<bool> EnterDungeon(Guid userId, Guid sessionId, string chatConnectionId, string gameConnectionId,
            Guid dungeonId, Guid avatarId);

        Task<bool> LeaveDungeon(Guid avatarId, Guid dungeonId);
    }
}
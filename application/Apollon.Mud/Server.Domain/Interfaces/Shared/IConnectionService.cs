using Apollon.Mud.Server.Model.Implementations;
using System;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Domain.Interfaces.Shared
{
    /// <summary>
    /// A Service to contain all active connections
    /// during a game, combining SignalR Connections, DungeonUsers,
    /// Dungeons and Avatars
    /// </summary>
    public interface IConnectionService
    {
        /// <summary>
        /// Method to deliver a connection based on UserId and SessionId
        /// </summary>
        /// <param name="userId">The userId of the user for which the connection is wanted for</param>
        /// <param name="sessionId">The sessionId of the game connection for which the connection is wanted for</param>
        /// <returns>The connection details of the asked session</returns>
        Connection GetConnection(Guid userId, Guid sessionId);

        /// <summary>
        /// Method to deliver a connection based on AvatarId
        /// </summary>
        /// <param name="avatarId">The avatarId for which the connection is wanted for</param>
        /// <returns>The connection details of the asked avatar</returns>
        Connection GetConnectionByAvatarId(Guid avatarId);

        /// <summary>
        /// Method to deliver a the Dungeon Master connection based on the dungeonId
        /// </summary>
        /// <param name="dungeonId">The dungeonId for which the Dungeon Master connection is wanted for</param>
        /// <returns>The Dungeon Master connection details of the asked dungeon</returns>
        Connection GetDungeonMasterConnectionByDungeonId(Guid dungeonId);

        /// <summary>
        /// Method to deliver a Dungeon Master connection based on UserId and SessionId
        /// </summary>
        /// <param name="userId">The userId of the user for which the Dungeon Master connection is wanted for</param>
        /// <param name="sessionId">The sessionId of the game connection for which the Dungeon Master connection is wanted for</param>
        /// <returns>The Dungeon Master connection details of the asked session</returns>
        Connection GetDungeonMasterConnection(Guid userId, Guid sessionId);

        /// <summary>
        /// Adds a new game connection the the ConnectionService
        /// </summary>
        /// <param name="userId">userId of the new connection</param>
        /// <param name="sessionId">sessionId of the new connection</param>
        /// <param name="chatConnectionId">SignalR chatConnectionId of the new connection</param>
        /// <param name="gameConnectionId">SignalR gameConnectionId of the new connection</param>
        /// <param name="dungeonId">dungeonId of the new connection</param>
        /// <param name="avatarId">avatarId of the new connection or null if the connection belongs to the Dungeon Master</param>
        void AddConnection(Guid userId, Guid sessionId, string chatConnectionId, string gameConnectionId,
            Guid dungeonId, Guid? avatarId);

        /// <summary>
        /// Removes an existing connection
        /// </summary>
        /// <param name="userId">userId of the connection that gets removed</param>
        /// <param name="sessionId">sessionId of the connection that gets removed</param>
        void RemoveConnection(Guid userId, Guid sessionId);

        /// <summary>
        /// Removes an existing connection based on its avatarId
        /// </summary>
        /// <param name="avatarId">The avatarId of the connection to remove</param>
        void RemoveConnectionByAvatarId(Guid avatarId);
    }
}
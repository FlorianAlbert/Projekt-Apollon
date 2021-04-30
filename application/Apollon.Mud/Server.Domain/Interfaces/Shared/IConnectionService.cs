using Apollon.Mud.Server.Model.Implementations;
using System;

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
        /// 
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        Connection GetConnectionByAvatarId(Guid avatarId);

        Connection GetDungeonMasterConnectionByDungeonId(Guid dungeonId);

        Connection GetDungeonMasterConnection(Guid userId, Guid sessionId);

        void AddConnection(Guid userId, Guid SessionId, string chatConnectionId, string gameConnectionId,
            Guid dungeonId, Guid? avatarId);

        void RemoveConnection(Guid userId, Guid sessionId);
    }
}
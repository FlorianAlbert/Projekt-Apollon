using Apollon.Mud.Server.Model.Implementations;
using System;

namespace Apollon.Mud.Server.Domain.Interfaces.Shared
{
    /// <summary>
    /// ToDo
    /// </summary>
    public interface IConnectionService
    {
        Connection GetConnection(Guid userId, Guid sessionId);

        Connection GetConnectionByAvatarId(Guid avatarId);

        Connection GetDungeonMasterConnectionByDungeonId(Guid dungeonId);

        Connection GetDungeonMasterConnection(Guid userId, Guid sessionId);

        void AddConnection(Guid userId, Guid SessionId, string chatConnectionId, string gameConnectionId,
            Guid dungeonId, Guid? avatarId);

        void RemoveConnection(Guid userId, Guid sessionId);
    }
}
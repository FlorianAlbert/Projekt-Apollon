using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;

namespace Apollon.Mud.Server.Domain.Implementations.Shared
{
    /// <inheritdoc cref="IConnectionService"/>
    public class ConnectionService : IConnectionService
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<string, Connection>> _Connections;
        private ConcurrentDictionary<string, ConcurrentDictionary<string, Connection>> Connections => _Connections ??= new ConcurrentDictionary<string, ConcurrentDictionary<string, Connection>>();

        /// <inheritdoc cref="IConnectionService.GetConnection"/>
        public Connection GetConnection(Guid userId, Guid sessionId)
        {
            Connection connection;
            try
            {
                connection = Connections[userId.ToString()][sessionId.ToString()];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

            return connection;
        }

        /// <inheritdoc cref="IConnectionService.GetConnectionByAvatarId"/>
        public Connection GetConnectionByAvatarId(Guid avatarId)
        {
            Connection connection;
            try
            {
                connection = Connections.Values.SelectMany(x => x.Values.Where(c => !c.IsDungeonMaster))
                    .SingleOrDefault(x => x.AvatarId == avatarId);
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            return connection;
        }

        /// <inheritdoc cref="IConnectionService.GetDungeonMasterConnectionByDungeonId"/>
        public Connection GetDungeonMasterConnectionByDungeonId(Guid dungeonId)
        {
            Connection dungeonMaster;
            try
            {
                dungeonMaster = Connections.Values.SelectMany(x => x.Values.Where(c => c.IsDungeonMaster))
                    .SingleOrDefault(x => x.DungeonId == dungeonId);
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            return dungeonMaster;
        }

        /// <inheritdoc cref="IConnectionService.GetDungeonMasterConnection"/>
        public Connection GetDungeonMasterConnection(Guid userId, Guid sessionId)
        {
            Connection connection;
            try
            {
                connection = Connections[userId.ToString()][sessionId.ToString()];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

            return !connection.IsDungeonMaster ? null : connection;
        }

        /// <inheritdoc cref="IConnectionService.AddConnection"/>
        public void AddConnection(Guid userId, Guid sessionId, string chatConnectionId, string gameConnectionId, Guid dungeonId,
            Guid? avatarId)
        {
            if (!Connections.ContainsKey(userId.ToString())) Connections.TryAdd(userId.ToString(), new ConcurrentDictionary<string, Connection>());

            if (Connections[userId.ToString()].ContainsKey(sessionId.ToString())) return;

            var connection = new Connection
            {
                AvatarId = avatarId,
                DungeonId = dungeonId,
                ChatConnectionId = chatConnectionId,
                GameConnectionId = gameConnectionId
            };
            Connections[userId.ToString()].TryAdd(sessionId.ToString(), connection);
        }

        /// <inheritdoc cref="IConnectionService.RemoveConnection"/>
        public void RemoveConnection(Guid userId, Guid sessionId)
        {
            if (!Connections.ContainsKey(userId.ToString())) return;

            if (!Connections[userId.ToString()].ContainsKey(sessionId.ToString())) return;

            Connections[userId.ToString()].TryRemove(sessionId.ToString(), out _);

            if (Connections[userId.ToString()].IsEmpty) Connections.Remove(userId.ToString(), out _);
        }

        /// <inheritdoc cref="IConnectionService.RemoveConnectionByAvatarId"/>
        public void RemoveConnectionByAvatarId(Guid avatarId)
        {
            var sessionId = Connections.Values.SelectMany(x => x.Where(c => c.Value.AvatarId == avatarId))
                .FirstOrDefault().Key;

            if (sessionId is null) return;

            var userId = Connections
                .FirstOrDefault(x => x.Value.Keys.Contains(sessionId)).Key;

            if (userId is null) return;

            Connections[userId].TryRemove(sessionId, out _);

            if (Connections[userId].IsEmpty) Connections.Remove(userId, out _);
        }
    }
}
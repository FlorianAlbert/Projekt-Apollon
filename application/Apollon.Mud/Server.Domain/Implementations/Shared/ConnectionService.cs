﻿using System;
using System.Collections.Generic;
using System.Linq;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;

namespace Apollon.Mud.Server.Domain.Implementations.Shared
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class ConnectionService: IConnectionService
    {
        private Dictionary<string, Dictionary<string, Connection>> _Connections;
        private Dictionary<string, Dictionary<string, Connection>> Connections => _Connections ??= new Dictionary<string, Dictionary<string, Connection>>();

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

        public Connection GetConnectionByAvatarId(Guid avatarId)
        {
            Connection connection;
            try
            {
                connection = Connections.Values.SelectMany(x => x.Values.Where(x => x.AvatarId != null))
                    .SingleOrDefault(x => x.AvatarId == avatarId);
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            return connection;
        }

        public Connection GetDungeonMasterConnectionByDungeonId(Guid dungeonId)
        {
            Connection dungeonMaster;
            try
            {
                dungeonMaster = Connections.Values.SelectMany(x => x.Values.Where(x => x.AvatarId == null))
                    .SingleOrDefault(x => x.DungeonId == dungeonId);
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            return dungeonMaster;
        }

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

            return connection.AvatarId is not null ? null : connection;
        }

        public void AddConnection(Guid userId, Guid sessionId, string chatConnectionId, string gameConnectionId, Guid dungeonId,
            Guid? avatarId)
        {
            if (!Connections.ContainsKey(userId.ToString())) Connections.Add(userId.ToString(), new Dictionary<string, Connection>());

            if (Connections[userId.ToString()].ContainsKey(sessionId.ToString())) return;

            var connection = new Connection
            {
                AvatarId = avatarId,
                DungeonId = dungeonId,
                ChatConnectionId = chatConnectionId,
                GameConnectionId = gameConnectionId
            };
            Connections[userId.ToString()].Add(sessionId.ToString(), connection);
        }

        public void RemoveConnection(Guid userId, Guid sessionId)
        {
            if (!Connections.ContainsKey(userId.ToString())) return;

            if (!Connections[userId.ToString()].ContainsKey(sessionId.ToString())) return;

            Connections[userId.ToString()].Remove(sessionId.ToString());

            if (Connections[userId.ToString()].Count == 0) Connections.Remove(userId.ToString());
        }
    }
}
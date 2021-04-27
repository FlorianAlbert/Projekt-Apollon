using System;
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
                connection = Connections.SingleOrDefault(x => x.Key == userId.ToString()).Value?
                    .SingleOrDefault(x => x.Key == sessionId.ToString()).Value;
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            return connection;
        }

        public Connection GetConnectionByAvatarId(Guid avatarId)
        {
            foreach (var userConnections in Connections.Values)
            {
                foreach (var connection in userConnections.Values)
                {
                    if (connection.AvatarId != null && connection.AvatarId == avatarId) return connection;
                }
            }

            return null;
        }

        public Connection GetDungeonMasterConnectionByDungeonId(Guid dungeonId)
        {
            var dungeonMasters = Connections.Values.SelectMany(x => x.Values.Where(x => x.AvatarId == null));
            return dungeonMasters.SingleOrDefault(x => x.DungeonId == dungeonId);
        }

        public Connection GetDungeonMasterConnection(Guid userId, Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public void AddConnection(Guid userId, Guid SessionId, string chatConnectionId, string gameConnectionId, Guid dungeonId,
            Guid? avatarId)
        {
            throw new NotImplementedException();
        }

        public void RemoveConnection(Guid userId, Guid sessionId)
        {
            throw new NotImplementedException();
        }
    }
}
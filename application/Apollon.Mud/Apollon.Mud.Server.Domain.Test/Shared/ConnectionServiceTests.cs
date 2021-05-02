using System;
using System.Collections.Generic;
using System.Linq;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;

namespace Apollon.Mud.Server.Domain.Test.Shared
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class ConnectionServiceTests: IConnectionService
    {
        [Fact]        
        public void NotFound_GetConnection_Fails()
        {

        }

        [Fact]
        public void ConnectionFound_GetConnection_Succeds()
        {

        }

        [Fact]
        public void InvalidOperation_GetConnectionByAvatarId_Fails()
        {

        }

        [Fact]
        public void Connection_GetConnectionByAvatarId_Succeds()
        {

        }
   
        [Fact]
        public void InvalidOperation_GetDungeonMasterConnectionByDungeonId()
        {

        }

        [Fact]
        public void DungeonMaster_GetDungeonMasterConnectionByDungeonId_Succeds()
        {

        }

        [Fact]
        public void KeyNotFound_GetDungeonMasterConnection()
        {

        }

        [Fact]
        public void Connection_GetDungeonMasterConnection_Succeds()
        {

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
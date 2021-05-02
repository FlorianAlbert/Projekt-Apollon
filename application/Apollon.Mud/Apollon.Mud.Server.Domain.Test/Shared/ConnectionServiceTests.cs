using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.Shared
{
    public class ConnectionServiceTests
    {
        private IFixture _Fixture;

        public ConnectionServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

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
        
        [Fact]
        public void DoesNotContainKey_AddConnection_Fails()
        {

        }

        [Fact]
        public void ContainKey_AddConnection_Succeds()
        {

        }
    
        [Fact]
        public void DoesNotContainKey_RemoveConnection_Fails()
        {

        }

        [Fact]
        public void WrongKey_RemoveConnection_Fails()
        {

        }

        [Fact]
        public void Remove_RemoveConnection_Succeds()
        {

        }
    }
}
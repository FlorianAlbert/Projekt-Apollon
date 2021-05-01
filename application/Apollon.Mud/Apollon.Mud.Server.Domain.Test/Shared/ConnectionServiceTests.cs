using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Domain.Implementations.Shared;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.Shared
{
    [ExcludeFromCodeCoverage]
    public class ConnectionServiceTests
    {
        private readonly Fixture _Fixture;

        private readonly ConnectionService _ConnectionService;

        private readonly Guid[] _UserIds = new Guid[12];
        private readonly Guid[] _SessionIds = new Guid[12];
        private readonly Guid[] _DungeonIds = new Guid[12];
        private readonly Guid?[] _AvatarIds = new Guid?[12];

        public ConnectionServiceTests()
        {
            _Fixture = new Fixture();

            _ConnectionService = new ConnectionService();

            for (var i = 0; i < 8; i++)
            {
                _UserIds[i] = _Fixture.Create<Guid>();
                _SessionIds[i] = _Fixture.Create<Guid>();
                _DungeonIds[i] = _Fixture.Create<Guid>();
                _AvatarIds[i] = _Fixture.Create<Guid>();

                _ConnectionService.AddConnection(_UserIds[i], _SessionIds[i], _Fixture.Create<string>(), _Fixture.Create<string>(), _DungeonIds[i], _AvatarIds[i]);
            }

            _UserIds[8] = _Fixture.Create<Guid>();
            _SessionIds[8] = _Fixture.Create<Guid>();
            _DungeonIds[8] = _Fixture.Create<Guid>();
            _AvatarIds[8] = null;

            _ConnectionService.AddConnection(_UserIds[8], _SessionIds[8], _Fixture.Create<string>(), _Fixture.Create<string>(), _DungeonIds[8], _AvatarIds[8]);


            _UserIds[9] = _Fixture.Create<Guid>();
            _SessionIds[9] = _Fixture.Create<Guid>();
            _DungeonIds[9] = _Fixture.Create<Guid>();
            _AvatarIds[9] = _AvatarIds[7];

            _ConnectionService.AddConnection(_UserIds[9], _SessionIds[9], _Fixture.Create<string>(), _Fixture.Create<string>(), _DungeonIds[9], _AvatarIds[9]);


            _UserIds[10] = _Fixture.Create<Guid>();
            _SessionIds[10] = _Fixture.Create<Guid>();
            _DungeonIds[10] = _Fixture.Create<Guid>();
            _AvatarIds[10] = null;

            _ConnectionService.AddConnection(_UserIds[10], _SessionIds[10], _Fixture.Create<string>(), _Fixture.Create<string>(), _DungeonIds[10], _AvatarIds[10]);


            _UserIds[11] = _Fixture.Create<Guid>();
            _SessionIds[11] = _Fixture.Create<Guid>();
            _DungeonIds[11] = _DungeonIds[10];
            _AvatarIds[11] = null;

            _ConnectionService.AddConnection(_UserIds[11], _SessionIds[11], _Fixture.Create<string>(), _Fixture.Create<string>(), _DungeonIds[11], _AvatarIds[11]);
        }

        [Fact]
        public void GetConnection_MissingKey_Fails()
        {
            var userId = _Fixture.Create<Guid>();
            var sessionId = _Fixture.Create<Guid>();

            var result =_ConnectionService.GetConnection(userId, sessionId);

            result.Should().BeNull();
        }

        [Theory]
        [AutoData]
        public void GetConnection_Succeeds([Range(0, 7)]int index)
        {
            var result = _ConnectionService.GetConnection(_UserIds[index], _SessionIds[index]);

            result.Should().NotBeNull();
        }

        [Fact]
        public void GetConnectionByAvatarId_MissingAvatarId_Fails()
        {
            var avatarId = _Fixture.Create<Guid>();

            var result = _ConnectionService.GetConnectionByAvatarId(avatarId);

            result.Should().BeNull();
        }

        [Fact]
        public void GetConnectionByAvatarId_DulicateAvatarId_Fails()
        {
            var result = _ConnectionService.GetConnectionByAvatarId(_AvatarIds[9].GetValueOrDefault());

            result.Should().BeNull();
        }

        [Theory]
        [AutoData]
        public void GetConnectionByAvatarId_Succeeds([Range(0, 6)]int index)
        {
            var result = _ConnectionService.GetConnectionByAvatarId(_AvatarIds[index].GetValueOrDefault());

            result.Should().NotBeNull();
        }

        [Fact]
        public void GetDungeonMasterConnectionByDungeonId_MissingDungeonId_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();

            var result = _ConnectionService.GetDungeonMasterConnectionByDungeonId(dungeonId);

            result.Should().BeNull();
        }

        [Fact]
        public void GetDungeonMasterConnectionByDungeonId_TwoDungeonMasters_Fails()
        {
            var result = _ConnectionService.GetDungeonMasterConnectionByDungeonId(_DungeonIds[11]);

            result.Should().BeNull();
        }

        [Fact]
        public void GetDungeonMasterConnectionByDungeonId_Succeeds()
        {
            var result = _ConnectionService.GetDungeonMasterConnectionByDungeonId(_DungeonIds[8]);

            result.Should().NotBeNull();
        }

        [Fact]
        public void GetDungeonMasterConnection_MissingKey_Fails()
        {
            var userId = _Fixture.Create<Guid>();
            var sessionId = _Fixture.Create<Guid>();

            var result = _ConnectionService.GetDungeonMasterConnection(userId, sessionId);

            result.Should().BeNull();
        }

        [Theory]
        [AutoData]
        public void GetDungeonMasterConnection_UserIdSessionIdIsNotDungeonMaster_Fails([Range(0, 7)] int index)
        {
            var result = _ConnectionService.GetDungeonMasterConnection(_UserIds[index], _SessionIds[index]);

            result.Should().BeNull();
        }

        [Fact]
        public void GetDungeonMasterConnection_Succeeds()
        {
            var result = _ConnectionService.GetDungeonMasterConnection(_UserIds[8], _SessionIds[8]);

            result.Should().NotBeNull();
        }

        [Theory]
        [AutoData]
        public void AddConnection_ConnectionAlreadyExists_Fails([Range(0, 11)] int index)
        {
            var newChatConnectionId = _Fixture.Create<string>();
            var newGameConnectionId = _Fixture.Create<string>();
            var newDungeonId = _Fixture.Create<Guid>();
            var newAvatarId = _Fixture.Create<Guid>();

            _ConnectionService.AddConnection(_UserIds[index], _SessionIds[index], newChatConnectionId, newGameConnectionId, newDungeonId, newAvatarId);
            var result = _ConnectionService.GetConnection(_UserIds[index], _SessionIds[index]);

            result.Should().NotBeNull();
            result.ChatConnectionId.Should().NotBe(newChatConnectionId);
            result.GameConnectionId.Should().NotBe(newGameConnectionId);
            result.DungeonId.Should().NotBe(newDungeonId);
            result.DungeonId.Should().Be(_DungeonIds[index]);
            result.AvatarId.Should().NotBe(newAvatarId);
            result.AvatarId.Should().Be(_AvatarIds[index]);
        }

        [Fact]
        public void AddConnection_Succeeds()
        {
            var newUserId = _Fixture.Create<Guid>();
            var newSessionId = _Fixture.Create<Guid>();
            var newChatConnectionId = _Fixture.Create<string>();
            var newGameConnectionId = _Fixture.Create<string>();
            var newDungeonId = _Fixture.Create<Guid>();
            var newAvatarId = _Fixture.Create<Guid>();

            _ConnectionService.AddConnection(newUserId, newSessionId, newChatConnectionId, newGameConnectionId, newDungeonId, newAvatarId);
            var result = _ConnectionService.GetConnection(newUserId, newSessionId);

            result.Should().NotBeNull();
            result.ChatConnectionId.Should().Be(newChatConnectionId);
            result.GameConnectionId.Should().Be(newGameConnectionId);
            result.DungeonId.Should().Be(newDungeonId);
            result.AvatarId.Should().Be(newAvatarId);
        }

        [Theory]
        [AutoData]
        public void RemoveConnection_Succeeds([Range(0, 11)] int index)
        {
            _ConnectionService.RemoveConnection(_UserIds[index], _SessionIds[index]);
            var result = _ConnectionService.GetConnection(_UserIds[index], _SessionIds[index]);

            result.Should().BeNull();
        }
    }
}
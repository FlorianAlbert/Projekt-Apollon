using System;
using System.Collections.Generic;
using Apollon.Mud.Server.Domain.Implementations.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Shared.HubContract;
using AutoFixture;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.Chat
{
    public class ChatServiceTests
    {
        private Fixture _Fixture;

        public ChatServiceTests()
        {
            _Fixture = new Fixture();
        }

        [Fact]
        public void PostRoomMessage_GameDbServiceThrowsException_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var gameDbService = Substitute.For<IGameDBService>();
            gameDbService.Get<IAvatar>(avatarId).Throws<InvalidOperationException>();

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostRoomMessage(dungeonId, avatarId, message);

            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.DidNotReceive().Clients.Clients(Arg.Any<IReadOnlyList<string>>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public void PostRoomMessage_GameDbServiceReturnsNull_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var gameDbService = Substitute.For<IGameDBService>();
            gameDbService.Get<IAvatar>(avatarId).Returns(null as IAvatar);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostRoomMessage(dungeonId, avatarId, message);

            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.DidNotReceive().Clients.Clients(Arg.Any<IReadOnlyList<string>>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public void PostRoomMessage_Succeeds()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();

            var firstAvatarId = _Fixture.Create<Guid>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var inspectableInRoom = Substitute.For<IInspectable>();
            var firstAvatarInRoom = Substitute.For<IAvatar>();
            firstAvatarInRoom.Status.Returns(Status.Approved);
            firstAvatarInRoom.Id.Returns(firstAvatarId);
            var secondAvatarInRoom = Substitute.For<IAvatar>();
            secondAvatarInRoom.Status.Returns(Status.Pending);
            var inspectableList = _Fixture.Build<List<IInspectable>>().Do(x =>
            {
                x.Add(inspectableInRoom);
                x.Add(firstAvatarInRoom);
            }).Create();

            var chatConnectionId = _Fixture.Create<string>();

            var connection = _Fixture.Build<Connection>()
                .With(x => x.AvatarId, avatarId)
                .With(x => x.ChatConnectionId, chatConnectionId)
                .Create();

            connectionService.GetConnectionByAvatarId(firstAvatarId).Returns(connection);

            var avatar = Substitute.For<IAvatar>();
            avatar.CurrentRoom.Inspectables.Returns(inspectableList);
            var avatarName = _Fixture.Create<string>();
            avatar.Name.Returns(avatarName);

            var gameDbService = Substitute.For<IGameDBService>();
            gameDbService.Get<IAvatar>(avatarId).Returns(avatar);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostRoomMessage(dungeonId, avatarId, message);

            connectionService.Received().GetConnectionByAvatarId(firstAvatarId);
            hubContext.Received().Clients.Clients(Arg.Is<List<string>>(x => x.Contains(chatConnectionId))).ReceiveChatMessage(avatarName, message);
        }

        [Fact]
        public void PostWhisperMessage_SenderAvatarIdIsNull_ConnectionServiceReturnsNull_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetDungeonMasterConnectionByDungeonId(dungeonId).Returns(null as Connection);

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var gameDbService = Substitute.For<IGameDBService>();

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(dungeonId, avatarId, recipientName, message);

            gameDbService.DidNotReceive().Get<IAvatar>(Arg.Any<Guid>());
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.DidNotReceive().Clients.Clients(Arg.Any<IReadOnlyList<string>>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public void PostWhisperMessage_SenderAvatarIdIsNull_Succeeds()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var chatConnectionId = _Fixture.Create<string>();

            var connection = _Fixture.Build<Connection>()
                .With(x => x.ChatConnectionId, chatConnectionId)
                .With(x => x.DungeonId, dungeonId)
                .Create();

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetDungeonMasterConnectionByDungeonId(dungeonId).Returns(connection);

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var gameDbService = Substitute.For<IGameDBService>();

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(dungeonId, null, recipientName, message);

            gameDbService.DidNotReceive().Get<IAvatar>(Arg.Any<Guid>());
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.Received().Clients.Client(chatConnectionId).ReceiveChatMessage("Dungeon Master", message);
        }
    }
}

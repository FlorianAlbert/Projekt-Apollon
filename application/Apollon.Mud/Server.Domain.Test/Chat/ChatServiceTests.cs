using System;
using System.Collections.Generic;
using System.Linq;
using Apollon.Mud.Server.Domain.Implementations.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Shared.HubContract;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.Chat
{
    public class ChatServiceTests
    {
        private IFixture _Fixture;

        public ChatServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public void PostRoomMessage_GameDbServiceReturnsNull_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.Get<Avatar>(avatarId).Returns(null as Avatar);

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

            var inspectableInRoom = Substitute.For<Inspectable>();
            var firstAvatarInRoom = Substitute.For<Avatar>();
            firstAvatarInRoom.Status.Returns(Status.Approved);
            firstAvatarInRoom.Id.Returns(firstAvatarId);
            var secondAvatarInRoom = Substitute.For<Avatar>();
            secondAvatarInRoom.Status.Returns(Status.Pending);
            var inspectableList = _Fixture.Build<List<Inspectable>>().Do(x =>
            {
                x.Add(inspectableInRoom);
                x.Add(firstAvatarInRoom);
                x.Add(secondAvatarInRoom);
            }).Create();

            var chatConnectionId = _Fixture.Create<string>();

            var connection = _Fixture.Build<Connection>()
                .With(x => x.AvatarId, avatarId)
                .With(x => x.ChatConnectionId, chatConnectionId)
                .Create();

            connectionService.GetConnectionByAvatarId(firstAvatarId).Returns(connection);

            var avatar = Substitute.For<Avatar>();
            avatar.CurrentRoom.Inspectables.Returns(inspectableList);
            var avatarName = _Fixture.Create<string>();
            avatar.Name.Returns(avatarName);

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.Get<Avatar>(avatarId).Returns(avatar);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostRoomMessage(dungeonId, avatarId, message);

            connectionService.Received().GetConnectionByAvatarId(firstAvatarId);
            hubContext.Received().Clients.Clients(Arg.Is<List<string>>(x => x.Contains(chatConnectionId) && x.Count == 1)).ReceiveChatMessage(avatarName, message);
        }

        [Fact]
        public void PostWhisperMessage_SenderAvatarIdIsNull_ConnectionServiceReturnsNull_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetDungeonMasterConnectionByDungeonId(dungeonId).Returns(null as Connection);

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var gameDbService = Substitute.For<IGameDbService>();

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(dungeonId, null, recipientName, message);

            gameDbService.DidNotReceive().Get<Avatar>(Arg.Any<Guid>());
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
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

            var gameDbService = Substitute.For<IGameDbService>();

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(dungeonId, null, recipientName, message);

            gameDbService.DidNotReceive().Get<Avatar>(Arg.Any<Guid>());
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.Received().Clients.Client(chatConnectionId).ReceiveChatMessage("Dungeon Master", message);
        }

        [Fact]
        public void PostWhisperMessage_GameDbServiceThrowsException_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved)
                .Throws<InvalidOperationException>();

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(dungeonId, avatarId, recipientName, message);

            gameDbService.DidNotReceive().Get<Avatar>(Arg.Any<Guid>());
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public void PostWhisperMessage_GameDbServiceReturnsNullForRecipient_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var returnArray = _Fixture.Create<Avatar[]>();
            returnArray.SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved)
                .Returns(null as Avatar);

            var senderAvatar = Substitute.For<Avatar>();

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().Returns(returnArray);
            gameDbService.Get<Avatar>(avatarId).Returns(senderAvatar);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(dungeonId, avatarId, recipientName, message);

            gameDbService.Received().Get<Avatar>(avatarId);
            gameDbService.Received().GetAll<Avatar>()
                .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public void PostWhisperMessage_GameDbServiceReturnsNullForSender_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();
            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.Get<Avatar>(avatarId).Returns(null as Avatar);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(dungeonId, avatarId, recipientName, message);

            gameDbService.Received().Get<Avatar>(avatarId);
            gameDbService.Received().GetAll<Avatar>()
                .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public void PostWhisperMessage_ConnectionServiceReturnsNull_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var senderAvatar = Substitute.For<Avatar>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var recipientAvatarId = _Fixture.Create<Guid>();

            var recipientAvatar = Substitute.For<Avatar>();
            recipientAvatar.Id.Returns(recipientAvatarId);
            recipientAvatar.Name.Returns(recipientName);
            recipientAvatar.Dungeon.Id.Returns(dungeonId);
            recipientAvatar.Status.Returns(Status.Approved);

            var returnArray = new Avatar[1];
            returnArray[0] = recipientAvatar;

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().Returns(returnArray);
            gameDbService.Get<Avatar>(avatarId).Returns(senderAvatar);

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetConnectionByAvatarId(recipientAvatarId).Returns(null as Connection);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(dungeonId, avatarId, recipientName, message);

            gameDbService.Received().Get<Avatar>(avatarId);
            gameDbService.Received().GetAll<Avatar>()
                .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.Received().GetConnectionByAvatarId(recipientAvatarId);
            hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public void PostWhisperMessage_SenderAvatarIdIsNotNull_Succeeds()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();
            
            var chatConnectionId = _Fixture.Create<string>();
            var senderAvatarName = _Fixture.Create<string>();

            var senderAvatar = Substitute.For<Avatar>();
            senderAvatar.Name.Returns(senderAvatarName);

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var recipientAvatarId = _Fixture.Create<Guid>();

            var recipientAvatar = Substitute.For<Avatar>();
            recipientAvatar.Id.Returns(recipientAvatarId);
            recipientAvatar.Name.Returns(recipientName);
            recipientAvatar.Dungeon.Id.Returns(dungeonId);
            recipientAvatar.Status.Returns(Status.Approved);

            var returnArray = new Avatar[1];
            returnArray[0] = recipientAvatar;

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().Returns(returnArray);
            gameDbService.Get<Avatar>(avatarId).Returns(senderAvatar);

            var connection = _Fixture.Build<Connection>()
                .With(x => x.AvatarId, recipientAvatarId)
                .With(x => x.ChatConnectionId, chatConnectionId)
                .With(x => x.DungeonId, dungeonId)
                .Create();

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetConnectionByAvatarId(recipientAvatarId).Returns(connection);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(dungeonId, avatarId, recipientName, message);

            gameDbService.Received().Get<Avatar>(avatarId);
            gameDbService.Received().GetAll<Avatar>()
                .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.Received().GetConnectionByAvatarId(recipientAvatarId);
            hubContext.Received().Clients.Client(chatConnectionId).ReceiveChatMessage(senderAvatarName, message);
        }

        [Fact]
        public void PostGlobalMessage_Succeeds()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();

            var chatConnectionId = _Fixture.Create<string>();

            var avatar1Id = _Fixture.Create<Guid>();
            var avatar2Id = _Fixture.Create<Guid>();

            var avatar1 = Substitute.For<Avatar>();
            avatar1.Id.Returns(avatar1Id);

            var avatar2 = Substitute.For<Avatar>();
            avatar2.Id.Returns(avatar2Id);

            var returnArray = new Avatar[2];
            returnArray[0] = avatar1;
            returnArray[1] = avatar2;

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>()
                .Where(x => x.Dungeon.Id == dungeonId && x.Status == Status.Approved).ToArray().Returns(returnArray);
            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var connection = _Fixture.Build<Connection>()
                .With(x => x.AvatarId, avatar1Id)
                .With(x => x.ChatConnectionId, chatConnectionId)
                .With(x => x.DungeonId, dungeonId)
                .Create();

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetConnectionByAvatarId(avatar1Id).Returns(connection);
            connectionService.GetConnectionByAvatarId(avatar2Id).Returns(null as Connection);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostGlobalMessage(dungeonId, message);

            hubContext.Received().Clients.Clients(Arg.Is<List<string>>(x => x.Contains(chatConnectionId) && x.Count == 1)).ReceiveChatMessage("Dungeon Master", message);
        }
    }
}

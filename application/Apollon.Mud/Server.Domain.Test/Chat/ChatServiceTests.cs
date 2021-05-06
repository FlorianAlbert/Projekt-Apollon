using System;
using System.Collections.Generic;
using System.Linq;
using Apollon.Mud.Server.Domain.Implementations.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
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
            _Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _Fixture.Behaviors.Remove(b));
            _Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var inspectableInRoom = _Fixture.Create<Inspectable>();
            var firstAvatarInRoom = new Avatar();
            firstAvatarInRoom.Status = Status.Approved;
            var secondAvatarInRoom = new Avatar();
            secondAvatarInRoom.Status = Status.Pending;

            var chatConnectionId = _Fixture.Create<string>();

            var connection = _Fixture.Build<Connection>()
                .With(x => x.AvatarId, avatarId)
                .With(x => x.ChatConnectionId, chatConnectionId)
                .Create();

            connectionService.GetConnectionByAvatarId(firstAvatarInRoom.Id).Returns(connection);

            var avatarName = _Fixture.Create<string>();
            var avatar = new Avatar()
            {
                Name = avatarName
            };
            var room = new Room(string.Empty, string.Empty);
            room.Inspectables.Add(firstAvatarInRoom);
            room.Inspectables.Add(secondAvatarInRoom);
            room.Inspectables.Add(inspectableInRoom);

            avatar.CurrentRoom = room;

            

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.Get<Avatar>(avatarId).Returns(avatar);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostRoomMessage(dungeonId, avatarId, message);

            connectionService.Received().GetConnectionByAvatarId(firstAvatarInRoom.Id);
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

            var senderAvatar = new Avatar();

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

            var senderAvatar = new Avatar();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var recipientAvatarId = _Fixture.Create<Guid>();

            var recipientAvatar = new Avatar();
            recipientAvatar.Status = Status.Approved;
            recipientAvatar.Dungeon = new Dungeon(string.Empty, string.Empty, string.Empty);

            var returnArray = new Avatar[1];
            returnArray[0] = recipientAvatar;

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().Returns(returnArray);
            gameDbService.Get<Avatar>(avatarId).Returns(senderAvatar);

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetConnectionByAvatarId(recipientAvatar.Id).Returns(null as Connection);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(recipientAvatar.Dungeon.Id, avatarId, recipientAvatar.Name, message);

            gameDbService.Received().Get<Avatar>(avatarId);
            gameDbService.Received().GetAll<Avatar>()
                .SingleOrDefault(x => x.Name == recipientAvatar.Name && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.Received().GetConnectionByAvatarId(recipientAvatar.Id);
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

            var senderAvatar = new Avatar();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var recipientAvatarId = _Fixture.Create<Guid>();

            var recipientAvatar = new Avatar();

            recipientAvatar.Dungeon = new Dungeon(string.Empty, string.Empty, string.Empty);
            recipientAvatar.Status = Status.Approved;

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
            connectionService.GetConnectionByAvatarId(recipientAvatar.Id).Returns(connection);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostWhisperMessage(recipientAvatar.Dungeon.Id, avatarId, recipientAvatar.Name, message);

            gameDbService.Received().Get<Avatar>(avatarId);
            gameDbService.Received().GetAll<Avatar>()
                .SingleOrDefault(x => x.Name == recipientAvatar.Name && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.Received().GetConnectionByAvatarId(recipientAvatar.Id);
            hubContext.Received().Clients.Client(chatConnectionId).ReceiveChatMessage(senderAvatar.Name, message);
        }

        [Fact]
        public void PostGlobalMessage_Succeeds()
        {
            _Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _Fixture.Behaviors.Remove(b));
            _Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var dungeonId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();

            var chatConnectionId = _Fixture.Create<string>();

            var avatar1 = new Avatar();
            avatar1.Dungeon = new Dungeon(string.Empty, string.Empty, string.Empty);

            var avatar2 = new Avatar();
            avatar2.Dungeon = new Dungeon(string.Empty, string.Empty, string.Empty);

            var returnArray = new Avatar[2];
            returnArray[0] = avatar1;
            returnArray[1] = avatar2;

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>()
                .Where(x => x.Dungeon.Id == dungeonId && x.Status == Status.Approved).ToArray().Returns(returnArray);
            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var connection = _Fixture.Build<Connection>()
                .With(x => x.AvatarId, avatar1.Id)
                .With(x => x.ChatConnectionId, chatConnectionId)
                .With(x => x.DungeonId, dungeonId)
                .Create();

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetConnectionByAvatarId(avatar1.Id).Returns(connection);
            connectionService.GetConnectionByAvatarId(avatar2.Id).Returns(null as Connection);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            chatService.PostGlobalMessage(dungeonId, message);

            hubContext.Received().Clients.Clients(Arg.Is<List<string>>(x => x.Contains(chatConnectionId) && x.Count == 1)).ReceiveChatMessage("Dungeon Master", message);
        }
    }
}

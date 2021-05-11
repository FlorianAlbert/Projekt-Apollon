using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task PostRoomMessage_GameDbServiceReturnsNull_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.Get<Avatar>(avatarId).Returns(null as Avatar);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            await chatService.PostRoomMessage(dungeonId, avatarId, message);

            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            await hubContext.DidNotReceive().Clients.Clients(Arg.Any<IReadOnlyList<string>>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public async Task PostRoomMessage_Succeeds()
        {
            _Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _Fixture.Behaviors.Remove(b));
            _Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var firstAvatarInRoom = new Avatar {Status = Status.Approved};
            var secondAvatarInRoom = new Avatar {Status = Status.Pending};

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
            room.Avatars.Add(firstAvatarInRoom);
            room.Avatars.Add(secondAvatarInRoom);

            avatar.CurrentRoom = room;

            

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.Get<Avatar>(avatarId).Returns(avatar);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            await chatService.PostRoomMessage(dungeonId, avatarId, message);

            connectionService.Received().GetConnectionByAvatarId(firstAvatarInRoom.Id);
            await hubContext.Received().Clients.Clients(Arg.Is<List<string>>(x => x.Contains(chatConnectionId) && x.Count == 1)).ReceiveChatMessage(avatarName, message);
        }

        [Fact]
        public async Task PostWhisperMessage_SenderAvatarIdIsNull_ConnectionServiceReturnsNull_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetDungeonMasterConnectionByDungeonId(dungeonId).Returns(null as Connection);

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var gameDbService = Substitute.For<IGameDbService>();

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            await chatService.PostWhisperMessage(dungeonId, null, recipientName, message);

            await gameDbService.DidNotReceive().Get<Avatar>(Arg.Any<Guid>());
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            await hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public async Task PostWhisperMessage_SenderAvatarIdIsNull_Succeeds()
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

            await chatService.PostWhisperMessage(dungeonId, null, recipientName, message);

            await gameDbService.DidNotReceive().Get<Avatar>(Arg.Any<Guid>());
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            await hubContext.Received().Clients.Client(chatConnectionId).ReceiveChatMessage("Dungeon Master", message);
        }

        /*[Fact] ToDo anpassen
        public async Task PostWhisperMessage_GameDbServiceThrowsException_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var avatarListMock = Substitute.For<ICollection<Avatar>>();

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().Returns(avatarListMock);

            avatarListMock.SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved)
                .Throws<InvalidOperationException>();

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            await chatService.PostWhisperMessage(dungeonId, avatarId, recipientName, message);

            await gameDbService.DidNotReceive().Get<Avatar>(Arg.Any<Guid>());
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }*/

        [Fact]
        public async Task PostWhisperMessage_GameDbServiceReturnsNullForRecipient_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var connectionService = Substitute.For<IConnectionService>();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var returnArray = new Avatar[0];

            var senderAvatar = new Avatar();

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().Returns(returnArray);
            gameDbService.Get<Avatar>(avatarId).Returns(senderAvatar);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            await chatService.PostWhisperMessage(dungeonId, avatarId, recipientName, message);

            await gameDbService.Received().Get<Avatar>(avatarId);
            gameDbService.Received().GetAll<Avatar>()
                .Result
                .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            await hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public async Task PostWhisperMessage_GameDbServiceReturnsNullForSender_Fails()
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

            await chatService.PostWhisperMessage(dungeonId, avatarId, recipientName, message);

            await gameDbService.Received().Get<Avatar>(avatarId);
            gameDbService.Received().GetAll<Avatar>()
                .Result
                .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.DidNotReceive().GetConnectionByAvatarId(Arg.Any<Guid>());
            await hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public async Task PostWhisperMessage_ConnectionServiceReturnsNull_Fails()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();

            var senderAvatar = new Avatar();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var recipientAvatar = new Avatar();
            recipientAvatar.Status = Status.Approved;
            recipientAvatar.Dungeon = new Dungeon(string.Empty, string.Empty, string.Empty);

            var avatarList = new List<Avatar>{recipientAvatar};

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().Returns(avatarList); 
            gameDbService.Get<Avatar>(avatarId).Returns(senderAvatar);

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetConnectionByAvatarId(recipientAvatar.Id).Returns(null as Connection);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            await chatService.PostWhisperMessage(recipientAvatar.Dungeon.Id, avatarId, recipientAvatar.Name, message);

            await gameDbService.Received().Get<Avatar>(avatarId);
            (await gameDbService.Received().GetAll<Avatar>())
                .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.Received().GetConnectionByAvatarId(recipientAvatar.Id);
            await hubContext.DidNotReceive().Clients.Client(Arg.Any<string>()).ReceiveChatMessage(Arg.Any<string>(), message);
        }

        [Fact]
        public async Task PostWhisperMessage_SenderAvatarIdIsNotNull_Succeeds()
        {
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();
            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();
            
            var chatConnectionId = _Fixture.Create<string>();
            var senderAvatarName = _Fixture.Create<string>();

            var senderAvatar = new Avatar();

            var hubContext = Substitute.For<IHubContext<ChatHub, IClientChatHubContract>>();

            var recipientAvatar = new Avatar();

            recipientAvatar.Dungeon = new Dungeon(string.Empty, string.Empty, string.Empty);
            recipientAvatar.Status = Status.Approved;

            var avatarList = new List<Avatar>(){recipientAvatar};

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().Returns(avatarList);
            gameDbService.Get<Avatar>(avatarId).Returns(senderAvatar);

            var connection = _Fixture.Build<Connection>()
                .With(x => x.AvatarId, recipientAvatar.Id)
                .With(x => x.ChatConnectionId, chatConnectionId)
                .With(x => x.DungeonId, dungeonId)
                .Create();

            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetConnectionByAvatarId(recipientAvatar.Id).Returns(connection);

            var chatService = new ChatService(gameDbService, connectionService, hubContext);

            await chatService.PostWhisperMessage(recipientAvatar.Dungeon.Id, avatarId, recipientAvatar.Name, message);

            await gameDbService.Received().Get<Avatar>(avatarId);
            gameDbService.Received().GetAll<Avatar>()
                .Result
                .SingleOrDefault(x => x.Name == recipientName && x.Dungeon.Id == dungeonId && x.Status == Status.Approved);
            connectionService.Received().GetConnectionByAvatarId(recipientAvatar.Id);
            await hubContext.Received().Clients.Client(chatConnectionId).ReceiveChatMessage(senderAvatarName, message);
        }

        [Fact]
        public async Task PostGlobalMessage_Succeeds()
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

            var avatars = new List<Avatar>() {avatar1, avatar2};

            var gameDbService = Substitute.For<IGameDbService>();
            gameDbService.GetAll<Avatar>().Returns(avatars);
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

            await chatService.PostGlobalMessage(dungeonId, message);

            await hubContext.Received().Clients.Clients(Arg.Is<List<string>>(x => x.Contains(chatConnectionId) && x.Count == 1)).ReceiveChatMessage("Dungeon Master", message);
        }
    }
}

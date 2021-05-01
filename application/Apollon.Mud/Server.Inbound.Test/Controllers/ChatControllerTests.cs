using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Inbound.Controllers;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Shared.Game.Chat;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Inbound.Test.Controllers
{
    public class ChatControllerTests
    {
        private IFixture _Fixture;

        public ChatControllerTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public async Task PostRoomMessage_UserIdClaimIsNull_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var sessionIdClaim = new Claim("SessionId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> {sessionIdClaim});

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService);
            chatController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostRoomMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostRoomMessage_SessionIdClaimIsNull_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var userIdClaim = new Claim("UserId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { userIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService);
            chatController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostRoomMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostRoomMessage_UserIdClaimIsNotParseable_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var userIdClaim = new Claim("UserId", string.Empty);
            var sessionIdClaim = new Claim("SessionId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { userIdClaim, sessionIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService);
            chatController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostRoomMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostRoomMessage_SessionIdClaimIsNotParseable_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var sessionIdClaim = new Claim("SessionId", string.Empty);
            var userIdClaim = new Claim("UserId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { userIdClaim, sessionIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService);
            chatController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostRoomMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostRoomMessage_ConnectionIsDungeonMaster_Fails()
        {
            var userId = _Fixture.Create<Guid>();
            var sessionId = _Fixture.Create<Guid>();

            var sessionIdClaim = new Claim("SessionId", sessionId.ToString());
            var userIdClaim = new Claim("UserId", userId.ToString());

            var connection = _Fixture.Build<Connection>()
                .Without(x => x.AvatarId)
                .Create();

            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetConnection(userId, sessionId).Returns(connection);

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { userIdClaim, sessionIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostRoomMessage(messageDto);

            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task PostRoomMessage_Succeeds()
        {
            var userId = _Fixture.Create<Guid>();
            var sessionId = _Fixture.Create<Guid>();
            var dungeonId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();

            var sessionIdClaim = new Claim("SessionId", sessionId.ToString());
            var userIdClaim = new Claim("UserId", userId.ToString());

            var connection = _Fixture.Build<Connection>()
                .With(x => x.DungeonId, dungeonId)
                .With(x => x.AvatarId, avatarId)
                .Create();

            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();
            connectionService.GetConnection(userId, sessionId).Returns(connection);

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { userIdClaim, sessionIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            var message = _Fixture.Create<string>();
            var messageDto = _Fixture.Build<ChatMessageDto>()
                .With(x => x.Message, message)
                .Create();

            var result = await chatController.PostRoomMessage(messageDto);

            result.Should().BeOfType<OkResult>();
            chatService.Received().PostRoomMessage(dungeonId, avatarId, message);
        }
    }
}
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
        private readonly IFixture _Fixture;

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

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
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

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
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

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
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

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
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
            connectionService.Received().GetConnection(userId, sessionId);
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
            connectionService.Received().GetConnection(userId, sessionId);
            chatService.Received().PostRoomMessage(dungeonId, avatarId, message);
        }

        [Fact]
        public async Task PostWhisperMessage_UserIdClaimIsNull_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var sessionIdClaim = new Claim("SessionId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { sessionIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostWhisperMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostWhisperMessage_SessionIdClaimIsNull_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var userIdClaim = new Claim("UserId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { userIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostWhisperMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostWhisperMessage_UserIdClaimIsNotParseable_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var userIdClaim = new Claim("UserId", string.Empty);
            var sessionIdClaim = new Claim("SessionId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { userIdClaim, sessionIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostWhisperMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostWhisperMessage_SessionIdClaimIsNotParseable_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var sessionIdClaim = new Claim("SessionId", string.Empty);
            var userIdClaim = new Claim("UserId", Guid.NewGuid().ToString());

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

            var result = await chatController.PostWhisperMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostWhisperMessage_WithAvatarIdNotNull_Succeeds()
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
            var recipientName = _Fixture.Create<string>();
            var messageDto = _Fixture.Build<ChatMessageDto>()
                .With(x => x.Message, message)
                .With(x => x.RecipientName, recipientName)
                .Create();

            var result = await chatController.PostWhisperMessage(messageDto);

            result.Should().BeOfType<OkResult>();
            connectionService.Received().GetConnection(userId, sessionId);
            chatService.Received().PostWhisperMessage(dungeonId, avatarId, recipientName, message);
        }

        [Fact]
        public async Task PostWhisperMessage_WithAvatarIdNull_Succeeds()
        {
            var userId = _Fixture.Create<Guid>();
            var sessionId = _Fixture.Create<Guid>();
            var dungeonId = _Fixture.Create<Guid>();

            var sessionIdClaim = new Claim("SessionId", sessionId.ToString());
            var userIdClaim = new Claim("UserId", userId.ToString());

            var connection = _Fixture.Build<Connection>()
                .With(x => x.DungeonId, dungeonId)
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

            var message = _Fixture.Create<string>();
            var recipientName = _Fixture.Create<string>();
            var messageDto = _Fixture.Build<ChatMessageDto>()
                .With(x => x.Message, message)
                .With(x => x.RecipientName, recipientName)
                .Create();

            var result = await chatController.PostWhisperMessage(messageDto);

            result.Should().BeOfType<OkResult>();
            connectionService.Received().GetConnection(userId, sessionId);
            chatService.Received().PostWhisperMessage(dungeonId, null, recipientName, message);
        }

        [Fact]
        public async Task PostGlobalMessage_UserIdClaimIsNull_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var sessionIdClaim = new Claim("SessionId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { sessionIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostGlobalMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostGlobalMessage_SessionIdClaimIsNull_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var userIdClaim = new Claim("UserId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { userIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostGlobalMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostGlobalMessage_UserIdClaimIsNotParseable_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var userIdClaim = new Claim("UserId", string.Empty);
            var sessionIdClaim = new Claim("SessionId", Guid.NewGuid().ToString());

            var claimsPrincipal = Substitute.For<ClaimsPrincipal>();
            claimsPrincipal.Claims.Returns(new List<Claim> { userIdClaim, sessionIdClaim });

            var httpContext = _Fixture.Create<HttpContext>();
            httpContext.User = claimsPrincipal;

            var chatController = new ChatController(chatService, connectionService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostGlobalMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostGlobalMessage_SessionIdClaimIsNotParseable_Fails()
        {
            var chatService = Substitute.For<IChatService>();
            var connectionService = Substitute.For<IConnectionService>();

            var sessionIdClaim = new Claim("SessionId", string.Empty);
            var userIdClaim = new Claim("UserId", Guid.NewGuid().ToString());

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

            var result = await chatController.PostGlobalMessage(messageDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostGlobalMessage_ConnectionIsNotDungeonMaster_Fails()
        {
            var userId = _Fixture.Create<Guid>();
            var sessionId = _Fixture.Create<Guid>();
            var avatarId = _Fixture.Create<Guid>();

            var sessionIdClaim = new Claim("SessionId", sessionId.ToString());
            var userIdClaim = new Claim("UserId", userId.ToString());

            var connection = _Fixture.Build<Connection>()
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

            var messageDto = _Fixture.Create<ChatMessageDto>();

            var result = await chatController.PostGlobalMessage(messageDto);

            result.Should().BeOfType<ForbidResult>();
            connectionService.Received().GetConnection(userId, sessionId);
        }

        [Fact]
        public async Task PostGlobalMessage_Succeeds()
        {
            var userId = _Fixture.Create<Guid>();
            var sessionId = _Fixture.Create<Guid>();
            var dungeonId = _Fixture.Create<Guid>();

            var sessionIdClaim = new Claim("SessionId", sessionId.ToString());
            var userIdClaim = new Claim("UserId", userId.ToString());

            var connection = _Fixture.Build<Connection>()
                .With(x => x.DungeonId, dungeonId)
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

            var message = _Fixture.Create<string>();
            var messageDto = _Fixture.Build<ChatMessageDto>()
                .With(x => x.Message, message)
                .Create();

            var result = await chatController.PostGlobalMessage(messageDto);

            result.Should().BeOfType<OkResult>();
            connectionService.Received().GetConnection(userId, sessionId);
            chatService.Received().PostGlobalMessage(dungeonId, message);
        }
    }
}
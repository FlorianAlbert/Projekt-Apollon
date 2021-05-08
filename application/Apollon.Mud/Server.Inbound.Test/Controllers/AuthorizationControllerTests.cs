using System;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Inbound.Controllers;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Shared.Dungeon.User;
using Apollon.Mud.Shared.UserManagement.Authorization;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Inbound.Test.Controllers
{
    public class AuthorizationControllerTests
    {
        private readonly IFixture _Fixture;
        public AuthorizationControllerTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public async Task UnauthorizedLogin_Fails()
        {
            var emailMock = _Fixture.Create<string>();
            var passwordMock = _Fixture.Create<string>();

            var loginResultMock = _Fixture.Build<LoginResult>()
                .With(x => x.Status, LoginResultStatus.Unauthorized)
                .Without(x => x.User)
                .Create();

            var authorizationRequestDtoMock = _Fixture.Build<AuthorizationRequestDto>()
                .With(x => x.UserEmail, emailMock)
                .With(x => x.Password, passwordMock)
                .Create();

            var authorizationServiceMock = Substitute.For<IAuthorizationService>();
            authorizationServiceMock.Login(emailMock, passwordMock).Returns(loginResultMock);


            var authorizationController = new AuthorizationController(authorizationServiceMock);
            var result = await authorizationController.Login(authorizationRequestDtoMock);


            await authorizationServiceMock.Received().Login(emailMock, passwordMock);
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task BadRequestLogin_Fails()
        {
            var emailMock = _Fixture.Create<string>();
            var passwordMock = _Fixture.Create<string>();

            var loginResultMock = _Fixture.Build<LoginResult>()
                .With(x => x.Status, LoginResultStatus.BadRequest)
                .Without(x => x.User)
                .Create();

            var authorizationRequestDtoMock = _Fixture.Build<AuthorizationRequestDto>()
                .With(x => x.UserEmail, emailMock)
                .With(x => x.Password, passwordMock)
                .Create();

            var authorizationServiceMock = Substitute.For<IAuthorizationService>();
            authorizationServiceMock.Login(emailMock, passwordMock).Returns(loginResultMock);


            var authorizationController = new AuthorizationController(authorizationServiceMock);
            var result = await authorizationController.Login(authorizationRequestDtoMock);


            await authorizationServiceMock.Received().Login(emailMock, passwordMock);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Login_Succeeds()
        {
            var emailMock = _Fixture.Create<string>();
            var passwordMock = _Fixture.Create<string>();
            var tokenMock = _Fixture.Create<string>();
            var dungeonUserIdMock = Guid.NewGuid();

            var dungeonUserMock = _Fixture.Build<DungeonUser>()
                .With(x => x.LastActive, DateTime.Now)
                .With(x => x.Email, emailMock)
                .With(x => x.EmailConfirmed, true)
                .With(x => x.Id, dungeonUserIdMock.ToString)
                .Without(x => x.DungeonMasterDungeons)
                .Without(x => x.DungeonOwnerDungeons)
                .Without(x => x.CurrentDungeonMasterDungeons)
                .Without(x => x.BlackListDungeons)
                .Without(x => x.WhiteListDungeons)
                .Without(x => x.OpenRequestDungeons)
                .Without(x => x.Avatars)
                .Create();

            var dungeonUserDtoMock = _Fixture.Build<DungeonUserDto>()
                .With(x => x.LastActive, dungeonUserMock.LastActive)
                .With(x => x.Email, dungeonUserMock.Email)
                .With(x => x.EmailConfirmed, dungeonUserMock.EmailConfirmed)
                .With(x => x.Id, dungeonUserIdMock)
                .Create();

            var loginResultMock = _Fixture.Build<LoginResult>()
                .With(x => x.Token, tokenMock)
                .With(x => x.User, dungeonUserMock)
                .With(x => x.Status, LoginResultStatus.OK)
                .Create();

            var authorizationRequestDtoMock = _Fixture.Build<AuthorizationRequestDto>()
                .With(x => x.UserEmail, emailMock)
                .With(x => x.Password, passwordMock)
                .Create();

            var authorizationResponseDtoMock = _Fixture.Build<AuthorizationResponseDto>()
                .With(x => x.Token, tokenMock)
                .With(x => x.DungeonUserDto, dungeonUserDtoMock)
                .Create();

            var authorizationServiceMock = Substitute.For<IAuthorizationService>();
            authorizationServiceMock.Login(emailMock, passwordMock).Returns(loginResultMock);


            var authorizationController = new AuthorizationController(authorizationServiceMock);
            var result = await authorizationController.Login(authorizationRequestDtoMock);


            await authorizationServiceMock.Received().Login(emailMock, passwordMock);
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            okResult.Value.Should().Be(authorizationResponseDtoMock);
        }

    }
}
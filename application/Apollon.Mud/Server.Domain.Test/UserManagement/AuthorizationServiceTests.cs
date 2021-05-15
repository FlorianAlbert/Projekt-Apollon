using System;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.User;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.UserManagement
{
    public class AuthorizationServiceTests
    {
        private readonly IFixture _Fixture;

        public AuthorizationServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public async void Login_FalseEmail_Fails()
        {
            var emailMock = _Fixture.Create<string>();
            var secretMock = _Fixture.Create<string>();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUserByEmail(emailMock).Returns((DungeonUser) null);

            var userManagerMock = Substitute.For<UserManager<DungeonUser>>(
                Substitute.For<IUserStore<DungeonUser>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            var signInManagerMock = Substitute.For<SignInManager<DungeonUser>>(
                userManagerMock,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<DungeonUser>>(),
                null,
                null,
                null,
                null);

            var configurationMock = Substitute.For<IConfiguration>();


            var authorizationService = new AuthorizationService(userDbServiceMock, signInManagerMock, userManagerMock, configurationMock);
            var loginResult = await authorizationService.Login(emailMock, secretMock);


            await userDbServiceMock.Received().GetUserByEmail(emailMock);
            loginResult.Status.Should().Be(LoginResultStatus.BadRequest);
        }

        [Fact]
        public async void Login_FalseEmailAndSecret_Fails()
        {
            var emailMock = _Fixture.Create<string>();
            var secretMock = _Fixture.Create<string>();
            var dungeonUserMock = _Fixture.Build<DungeonUser>()
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUserByEmail(emailMock).Returns(dungeonUserMock);

            var userManagerMock = Substitute.For<UserManager<DungeonUser>>(
                Substitute.For<IUserStore<DungeonUser>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            var signInManagerMock = Substitute.For<SignInManager<DungeonUser>>(
                userManagerMock,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<DungeonUser>>(),
                null,
                null,
                null,
                null);
            signInManagerMock.CheckPasswordSignInAsync(dungeonUserMock, secretMock, false).Returns(SignInResult.Failed);


            var configurationMock = Substitute.For<IConfiguration>();


            var authorizationService = new AuthorizationService(userDbServiceMock, signInManagerMock, userManagerMock, configurationMock);
            var loginResult = await authorizationService.Login(emailMock, secretMock);


            await userDbServiceMock.Received().GetUserByEmail(emailMock);
            await signInManagerMock.Received().CheckPasswordSignInAsync(dungeonUserMock, secretMock, false);
            loginResult.Status.Should().Be(LoginResultStatus.Unauthorized);
        }

        [Fact]
        public async void Login_EmailAndSecretCorrect_Success()
        {
            var emailMock = _Fixture.Create<string>();
            var secretMock = _Fixture.Create<string>();
            var dungeonUserMock = _Fixture.Build<DungeonUser>()
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUserByEmail(emailMock).Returns(dungeonUserMock);

            var userManagerMock = Substitute.For<UserManager<DungeonUser>>(
                Substitute.For<IUserStore<DungeonUser>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            userManagerMock.IsInRoleAsync(dungeonUserMock, Enum.GetName(Roles.Admin)).Returns(false);
            userManagerMock.IsInRoleAsync(dungeonUserMock, Enum.GetName(Roles.Player)).Returns(true);

            var signInManagerMock = Substitute.For<SignInManager<DungeonUser>>(
                userManagerMock,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<DungeonUser>>(),
                null,
                null,
                null,
                null);
            signInManagerMock.CheckPasswordSignInAsync(dungeonUserMock, secretMock, false).Returns(SignInResult.Success);


            var configurationMock = Substitute.For<IConfiguration>();
            configurationMock["JwtBearer:TokenSecret"].Returns("abcdefghijklmnopqrstuvwxyz");

            var authorizationService = new AuthorizationService(userDbServiceMock, signInManagerMock, userManagerMock, configurationMock);
            var loginResult = await authorizationService.Login(emailMock, secretMock);


            await userDbServiceMock.Received().GetUserByEmail(emailMock);
            await signInManagerMock.Received().CheckPasswordSignInAsync(dungeonUserMock, secretMock, false);
            loginResult.Status.Should().Be(LoginResultStatus.OK);
            loginResult.Token.Should().BeOfType<string>().And.NotBeEmpty();
            loginResult.User.Should().Be(dungeonUserMock);
        }
    }
}
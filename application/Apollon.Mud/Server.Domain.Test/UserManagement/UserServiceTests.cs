using System;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.User;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.UserManagement
{
    public class UserServiceTests
    {
        //ToDo Tests
        private IFixture _Fixture;

        public UserServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public async Task RequestUserRegistration_CreateUserFails_Fails()
        {
            var emailMock = _Fixture.Create<string>();
            var passwordMock = _Fixture.Create<string>(); 
            const bool adminRegisteredMock = true;
            var tokenMock = Guid.NewGuid();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.CreateUser(
                    Arg.Is<DungeonUser>(x => x.UserName == emailMock && x.Email == emailMock),
                    passwordMock, 
                    adminRegisteredMock)
                .Returns(false);

            var tokenServiceMock = Substitute.For<ITokenService>();
            tokenServiceMock.GenerateNewConfirmationToken().Returns(tokenMock);

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.RequestUserRegistration(emailMock, passwordMock);


            await userDbServiceMock.Received().CreateUser(
                Arg.Is<DungeonUser>(x => x.UserName == emailMock && x.Email == emailMock),
                passwordMock,
                adminRegisteredMock);
            tokenServiceMock.DidNotReceive().GenerateNewConfirmationToken();
            result.Should().Be(false);
        }

        [Fact]
        public async Task RequestUserRegistration_CreateUserSucceeds_Success()
        {
            var emailMock = _Fixture.Create<string>();
            var passwordMock = _Fixture.Create<string>();
            const bool adminRegisteredMock = true;
            var tokenMock = Guid.NewGuid();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.CreateUser(
                    Arg.Is<DungeonUser>(x => x.UserName == emailMock && x.Email == emailMock),
                    passwordMock,
                    adminRegisteredMock)
                .Returns(true);

            var tokenServiceMock = Substitute.For<ITokenService>();
            tokenServiceMock.GenerateNewConfirmationToken().Returns(tokenMock);

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.RequestUserRegistration(emailMock, passwordMock);


            await userDbServiceMock.Received().CreateUser(
                Arg.Is<DungeonUser>(x => x.UserName == emailMock && x.Email == emailMock),
                passwordMock,
                adminRegisteredMock);
            tokenServiceMock.Received().GenerateNewConfirmationToken();
            await emailServiceMock.Received().SendEmail(emailMock, Arg.Any<string>(), "Bestätigung deiner Email.");
            result.Should().Be(true);
        }

        [Fact]
        public async Task ConfirmUserRegistration_UserNull_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = Guid.NewGuid().ToString();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns((DungeonUser) null);

            var tokenServiceMock = Substitute.For<ITokenService>();

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmUserRegistration(userIdMock, tokenMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            result.Should().Be(false);
        }

        [Fact]
        public async Task ConfirmUserRegistration_ParseToken_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = _Fixture.Create<string>();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);

            var tokenServiceMock = Substitute.For<ITokenService>();

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmUserRegistration(userIdMock, tokenMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            result.Should().Be(false);
        }

        [Fact]
        public async Task ConfirmUserRegistration_CheckToken_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = Guid.NewGuid();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);

            var tokenServiceMock = Substitute.For<ITokenService>();
            tokenServiceMock.CheckConfirmationToken(tokenMock).Returns(false);

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmUserRegistration(userIdMock, tokenMock.ToString());


            await userDbServiceMock.Received().GetUser(userIdMock);
            tokenServiceMock.Received().CheckConfirmationToken(tokenMock);
            result.Should().Be(false);
        }

        [Fact]
        public async Task ConfirmUserRegistration_ConfirmEmail_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = Guid.NewGuid();
            var confirmationTokenMock = Guid.NewGuid().ToString();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);
            userDbServiceMock.GetEmailConfirmationToken(userMock).Returns(confirmationTokenMock);
            userDbServiceMock.ConfirmEmail(userMock, confirmationTokenMock).Returns(false);

            var tokenServiceMock = Substitute.For<ITokenService>();
            tokenServiceMock.CheckConfirmationToken(tokenMock).Returns(true);

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmUserRegistration(userIdMock, tokenMock.ToString());


            await userDbServiceMock.Received().GetUser(userIdMock);
            await userDbServiceMock.Received().GetEmailConfirmationToken(userMock);
            await userDbServiceMock.Received().ConfirmEmail(userMock, confirmationTokenMock);
            tokenServiceMock.Received().CheckConfirmationToken(tokenMock);
            result.Should().Be(false);
        }

        [Fact]
        public async Task ConfirmUserRegistration_Success()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = Guid.NewGuid();
            var confirmationTokenMock = Guid.NewGuid().ToString();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);
            userDbServiceMock.GetEmailConfirmationToken(userMock).Returns(confirmationTokenMock);
            userDbServiceMock.ConfirmEmail(userMock, confirmationTokenMock).Returns(true);

            var tokenServiceMock = Substitute.For<ITokenService>();
            tokenServiceMock.CheckConfirmationToken(tokenMock).Returns(true);

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmUserRegistration(userIdMock, tokenMock.ToString());


            await userDbServiceMock.Received().GetUser(userIdMock);
            await userDbServiceMock.Received().GetEmailConfirmationToken(userMock);
            await userDbServiceMock.Received().ConfirmEmail(userMock, confirmationTokenMock);
            tokenServiceMock.Received().CheckConfirmationToken(tokenMock);
            result.Should().Be(true);
        }

        [Fact]
        public async Task RequestPasswordReset_UserNull_Fails()
        {
            var resetTokenMock = Guid.NewGuid();
            var userMailMock = _Fixture.Create<string>();
            var userMock = (DungeonUser) null;
                var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUserByEmail(userMailMock).Returns(userMock);

            var tokenServiceMock = Substitute.For<ITokenService>();
            tokenServiceMock.GenerateNewResetToken().Returns(resetTokenMock);

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.RequestPasswordReset(userMailMock);


            await userDbServiceMock.Received().GetUserByEmail(userMailMock);
            tokenServiceMock.DidNotReceive().GenerateNewResetToken();
            result.Should().Be(false);
        }

        [Fact]
        public async Task RequestPasswordReset_Success()
        {
            var userMailMock = _Fixture.Create<string>();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Email, userMailMock)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUserByEmail(userMailMock).Returns(userMock);

            var tokenServiceMock = Substitute.For<ITokenService>();

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.RequestPasswordReset(userMailMock);


            await userDbServiceMock.Received().GetUserByEmail(userMailMock);
            await emailServiceMock.Received().SendEmail(userMailMock, Arg.Any<string>(), "Rücksetzung deines Passworts.");
            await userDbServiceMock.Received().GetUserByEmail(userMailMock);
            tokenServiceMock.Received().GenerateNewResetToken();
            result.Should().Be(true);
        }

        [Fact]
        public async Task ConfirmPasswordReset_UserNull_Fails()
        {
            var passwordMock = _Fixture.Create<string>();
            var tokenMock = string.Empty;
            var userIdMock = Guid.NewGuid();
            var userMock = (DungeonUser)null;

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);

            var tokenServiceMock = Substitute.For<ITokenService>();

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmPasswordReset(userIdMock, tokenMock, passwordMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            result.Should().Be(false);
        }

        [Fact]
        public async Task ConfirmPasswordReset_ParseGuid_Fails()
        {
            var passwordMock = _Fixture.Create<string>();
            var tokenMock = string.Empty;
            var userIdMock = Guid.NewGuid();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);

            var tokenServiceMock = Substitute.For<ITokenService>();

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmPasswordReset(userIdMock, tokenMock, passwordMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            tokenServiceMock.DidNotReceive().CheckResetToken(Arg.Any<Guid>());
            result.Should().Be(false);
        }

        [Fact]
        public async Task ConfirmPasswordReset_CheckToken_Fails()
        {
            var passwordMock = _Fixture.Create<string>();
            var tokenMock = Guid.NewGuid();
            var userIdMock = Guid.NewGuid();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);

            var tokenServiceMock = Substitute.For<ITokenService>();
            tokenServiceMock.CheckResetToken(tokenMock).Returns(false);

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmPasswordReset(userIdMock, tokenMock.ToString(), passwordMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            tokenServiceMock.Received().CheckResetToken(tokenMock);
            result.Should().Be(false);
        }

        [Fact]
        public async Task ConfirmPasswordReset_ResetPassword_Fails()
        {
            var passwordMock = _Fixture.Create<string>();
            var tokenMock = Guid.NewGuid();
            var userIdMock = Guid.NewGuid();
            var resetTokenMock = _Fixture.Create<string>();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);
            userDbServiceMock.GetResetToken(userMock).Returns(resetTokenMock);
            userDbServiceMock.ResetPassword(userMock,resetTokenMock,passwordMock).Returns(false);

            var tokenServiceMock = Substitute.For<ITokenService>();
            tokenServiceMock.CheckResetToken(tokenMock).Returns(true);

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmPasswordReset(userIdMock, tokenMock.ToString(), passwordMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            tokenServiceMock.Received().CheckResetToken(tokenMock);
            await userDbServiceMock.Received().GetResetToken(userMock);
            await userDbServiceMock.Received().ResetPassword(userMock, resetTokenMock, passwordMock);
            result.Should().Be(false);
        }

        [Fact]
        public async Task ConfirmPasswordReset_ResetPassword_Success()
        {
            var passwordMock = _Fixture.Create<string>();
            var tokenMock = Guid.NewGuid();
            var userIdMock = Guid.NewGuid();
            var resetTokenMock = _Fixture.Create<string>();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);
            userDbServiceMock.GetResetToken(userMock).Returns(resetTokenMock);
            userDbServiceMock.ResetPassword(userMock, resetTokenMock, passwordMock).Returns(true);

            var tokenServiceMock = Substitute.For<ITokenService>();
            tokenServiceMock.CheckResetToken(tokenMock).Returns(true);

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ConfirmPasswordReset(userIdMock, tokenMock.ToString(), passwordMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            tokenServiceMock.Received().CheckResetToken(tokenMock);
            await userDbServiceMock.Received().GetResetToken(userMock);
            await userDbServiceMock.Received().ResetPassword(userMock, resetTokenMock, passwordMock);
            result.Should().Be(true);
        }

        [Fact]
        public async Task ChangePassword_UserNull_Fails()
        {
            var newPasswordMock = _Fixture.Create<string>();
            var oldPassworMock = _Fixture.Create<string>();
            var userIdMock = Guid.NewGuid();
            var resetTokenMock = _Fixture.Create<string>();
            var userMock = (DungeonUser) null;

            var userDbServiceMock = Substitute.For<IUserDbService>();
            userDbServiceMock.GetUser(userIdMock).Returns(userMock);

            var tokenServiceMock = Substitute.For<ITokenService>();

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ChangePassword(userIdMock, oldPassworMock, newPasswordMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            result.Should().Be(false);
        }

        [Fact]
        public async Task ChangePassword_UpdateUser_Fails()
        {
            var newPasswordMock = _Fixture.Create<string>();
            var oldPassworMock = _Fixture.Create<string>();
            var userIdMock = Guid.NewGuid();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();

            userDbServiceMock.GetUser(userIdMock).Returns(userMock);
            userDbServiceMock.UpdateUser(userMock, oldPassworMock, newPasswordMock).Returns(false);

            var tokenServiceMock = Substitute.For<ITokenService>();

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ChangePassword(userIdMock, oldPassworMock, newPasswordMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            await userDbServiceMock.Received().UpdateUser(userMock, oldPassworMock, newPasswordMock);
            result.Should().Be(false);
        }

        [Fact]
        public async Task ChangePassword_UpdateUser_Success()
        {
            var newPasswordMock = _Fixture.Create<string>();
            var oldPassworMock = _Fixture.Create<string>();
            var userIdMock = Guid.NewGuid();
            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Id, userIdMock.ToString)
                .Create();

            var userDbServiceMock = Substitute.For<IUserDbService>();

            userDbServiceMock.GetUser(userIdMock).Returns(userMock);
            userDbServiceMock.UpdateUser(userMock, oldPassworMock, newPasswordMock).Returns(true);

            var tokenServiceMock = Substitute.For<ITokenService>();

            var emailServiceMock = Substitute.For<IEmailService>();


            var userService = new UserService(emailServiceMock, userDbServiceMock, (IGameDbService)null, tokenServiceMock);
            var result = await userService.ChangePassword(userIdMock, oldPassworMock, newPasswordMock);


            await userDbServiceMock.Received().GetUser(userIdMock);
            await userDbServiceMock.Received().UpdateUser(userMock, oldPassworMock, newPasswordMock);
            result.Should().Be(true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Inbound.Controllers;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Shared.Dungeon.User;
using Apollon.Mud.Shared.UserManagement.Password;
using Apollon.Mud.Shared.UserManagement.Registration;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Inbound.Test.Controllers
{
    public class UserControllerTest
    {
        //ToDo Test
        private IFixture _Fixture;

        public UserControllerTest()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public async Task RegistrateUser_RegistrationRequestDtoNull_Fails()
        {
            var registrationRequestDtoMock = (RegistrationRequestDto) null;
            
            var userServiceMock = Substitute.For<IUserService>();
            

            var userController = new UserController(userServiceMock);
            var result = await userController.RegistrateUser(registrationRequestDtoMock);


            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task RegistrateUser_RegistrationRequestBadArguments_Fails()
        {
            var passwordMock = _Fixture.Create<string>();
            var emailMock = _Fixture.Create<string>();

            var registrationRequestDtoMock = _Fixture.Build<RegistrationRequestDto>()
                .With(x => x.Password, passwordMock)
                .With(x => x.UserEmail, emailMock)
                .Create();

            var userServiceMock = Substitute.For<IUserService>(); 
            userServiceMock.RequestUserRegistration(emailMock, passwordMock).Returns(false);


            var userController = new UserController(userServiceMock);
            var result = await userController.RegistrateUser(registrationRequestDtoMock);


            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task RegistrateUser_RegistrationRequestCorrect_Succeeds()
        {
            var passwordMock = _Fixture.Create<string>();
            var emailMock = _Fixture.Create<string>();

            var registrationRequestDtoMock = _Fixture.Build<RegistrationRequestDto>()
                .With(x => x.Password, passwordMock)
                .With(x => x.UserEmail, emailMock)
                .Create();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.RequestUserRegistration(emailMock, passwordMock).Returns(true);


            var userController = new UserController(userServiceMock);
            var result = await userController.RegistrateUser(registrationRequestDtoMock);


            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ConfirmUserRegistration_TokenNull_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = (string) null;

            var userServiceMock = Substitute.For<IUserService>();


            var userController = new UserController(userServiceMock);
            var result = await userController.ConfirmUserRegistration(userIdMock, tokenMock);


            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ConfirmUserRegistration_CredentialsIncorrect_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = _Fixture.Create<string>();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.ConfirmUserRegistration(userIdMock, tokenMock).Returns(false);


            var userController = new UserController(userServiceMock);
            var result = await userController.ConfirmUserRegistration(userIdMock, tokenMock);


            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ConfirmUserRegistration_CredentialsCorrect_Succeeds()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = _Fixture.Create<string>();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.ConfirmUserRegistration(userIdMock, tokenMock).Returns(true);


            var userController = new UserController(userServiceMock);
            var result = await userController.ConfirmUserRegistration(userIdMock, tokenMock);


            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task DeleteUser_UserDoesntExists_Fails()
        {
            var userIdMock = Guid.NewGuid();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.DeleteUser(userIdMock).Returns(false);


            var userController = new UserController(userServiceMock);
            var result = await userController.DeleteUser(userIdMock);


            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteUser_UserExists_Succeeds()
        {
            var userIdMock = Guid.NewGuid();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.DeleteUser(userIdMock).Returns(true);


            var userController = new UserController(userServiceMock);
            var result = await userController.DeleteUser(userIdMock);


            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task GetAllUsers_UserListNull_Fails()
        {
            var userListMock = (ICollection<DungeonUser>) null;

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.GetAllUsers().Returns(userListMock);


            var userController = new UserController(userServiceMock);
            var result = await userController.GetAllUsers();


            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GetAllUsers_UserListEmpty_Fails()
        {
            var userListMock = new List<DungeonUser>();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.GetAllUsers().Returns(userListMock);


            var userController = new UserController(userServiceMock);
            var result = await userController.GetAllUsers();


            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GetAllUsers_UserListNotEmpty_Succeeds()
        {
            var emailMock = _Fixture.Create<string>();
            var lastActiveMock = DateTime.Now;
            var emailConfirmedMock = true;
            var userIdMock = Guid.NewGuid();

            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Email, emailMock)
                .With(x => x.EmailConfirmed, emailConfirmedMock)
                .With(x => x.LastActive, lastActiveMock)
                .With(x => x.Id, userIdMock.ToString)
                .Without(x => x.DungeonMasterDungeons)
                .Without(x => x.DungeonOwnerDungeons)
                .Without(x => x.CurrentDungeonMasterDungeons)
                .Without(x => x.BlackListDungeons)
                .Without(x => x.WhiteListDungeons)
                .Without(x => x.OpenRequestDungeons)
                .Without(x => x.Avatars)
                .Create();

            var userListMock = new List<DungeonUser>() {userMock};

            var userDtoMock = _Fixture.Build<DungeonUserDto>()
                .With(x => x.Email, emailMock)
                .With(x => x.EmailConfirmed, emailConfirmedMock)
                .With(x => x.LastActive, lastActiveMock)
                .With(x => x.Id, userIdMock)
                .Create();

            var userDtoListMock = new List<DungeonUserDto>() {userDtoMock};

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.GetAllUsers().Returns(userListMock);


            var userController = new UserController(userServiceMock);
            var result = await userController.GetAllUsers();


            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var dungeonUserDtoList = okResult.Value as IEnumerable<DungeonUserDto>;
            Assert.NotNull(dungeonUserDtoList);
            Assert.Contains(userDtoMock, dungeonUserDtoList);
        }

        [Fact]
        public async Task GetAllUsers_UserNull_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var userMock = (DungeonUser) null;

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.GetUser(userIdMock).Returns(userMock);


            var userController = new UserController(userServiceMock);
            var result = await userController.GetUser(userIdMock);


            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GetAUser_UserNotNull_Succeeds()
        {
            var emailMock = _Fixture.Create<string>();
            var lastActiveMock = DateTime.Now;
            var emailConfirmedMock = true;
            var userIdMock = Guid.NewGuid();

            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Email, emailMock)
                .With(x => x.EmailConfirmed, emailConfirmedMock)
                .With(x => x.LastActive, lastActiveMock)
                .With(x => x.Id, userIdMock.ToString)
                .Without(x => x.DungeonMasterDungeons)
                .Without(x => x.DungeonOwnerDungeons)
                .Without(x => x.CurrentDungeonMasterDungeons)
                .Without(x => x.BlackListDungeons)
                .Without(x => x.WhiteListDungeons)
                .Without(x => x.OpenRequestDungeons)
                .Without(x => x.Avatars)
                .Create();

            var userDtoMock = _Fixture.Build<DungeonUserDto>()
                .With(x => x.Email, emailMock)
                .With(x => x.EmailConfirmed, emailConfirmedMock)
                .With(x => x.LastActive, lastActiveMock)
                .With(x => x.Id, userIdMock)
                .Create();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.GetUser(userIdMock).Returns(userMock);


            var userController = new UserController(userServiceMock);
            var result = await userController.GetUser(userIdMock);


            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            okResult.Value.Should().Be(userDtoMock);
        }

        [Fact]
        public async Task RequestPasswordReset_RequestPasswordResetDtoNull_Fails()
        {
            var requestPasswordResetDto = (RequestPasswordResetDto) null;

            var userServiceMock = Substitute.For<IUserService>();

            var userController = new UserController(userServiceMock);
            var result = await userController.RequestPasswordReset(requestPasswordResetDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task RequestPasswordReset_NoCorrectEmail_Fails()
        {
            var emaiMock = _Fixture.Create<string>();
            var requestPasswordResetDto = _Fixture.Build<RequestPasswordResetDto>()
                .With(x => x.UserEmail, emaiMock)
                .Create();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.RequestPasswordReset(emaiMock).Returns(false);

            var userController = new UserController(userServiceMock);
            var result = await userController.RequestPasswordReset(requestPasswordResetDto);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task RequestPasswordReset_CorrectEmail_Succeeds()
        {
            var emaiMock = _Fixture.Create<string>();
            var requestPasswordResetDto = _Fixture.Build<RequestPasswordResetDto>()
                .With(x => x.UserEmail, emaiMock)
                .Create();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.RequestPasswordReset(emaiMock).Returns(true);

            var userController = new UserController(userServiceMock);
            var result = await userController.RequestPasswordReset(requestPasswordResetDto);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ConfirmPasswordReset_RequestPasswordResetDtoNull_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var passwortResetConfirmationDto = (PasswortResetConfirmationDto)null;

            var userServiceMock = Substitute.For<IUserService>();

            var userController = new UserController(userServiceMock);
            var result = await userController.ConfirmPasswordReset(passwortResetConfirmationDto, userIdMock);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ConfirmPasswordReset_NoCorrectEmail_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = Guid.NewGuid();
            var newPasswordMock = _Fixture.Create<string>();

            var passwortResetConfirmationDto = _Fixture.Build<PasswortResetConfirmationDto>()
                .With(x => x.Token, tokenMock.ToString)
                .With(x => x.NewPassword, newPasswordMock)
                .Create(); 

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.ConfirmPasswordReset(userIdMock, tokenMock.ToString(), newPasswordMock).Returns(false);

            var userController = new UserController(userServiceMock);
            var result = await userController.ConfirmPasswordReset(passwortResetConfirmationDto, userIdMock);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ConfirmPasswordReset_CorrectEmail_Succeeds()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = Guid.NewGuid();
            var newPasswordMock = _Fixture.Create<string>();

            var passwortResetConfirmationDto = _Fixture.Build<PasswortResetConfirmationDto>()
                .With(x => x.Token, tokenMock.ToString)
                .With(x => x.NewPassword, newPasswordMock)
                .Create();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.ConfirmPasswordReset(userIdMock, tokenMock.ToString(), newPasswordMock).Returns(true);

            var userController = new UserController(userServiceMock);
            var result = await userController.ConfirmPasswordReset(passwortResetConfirmationDto, userIdMock);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ChangePassword_RequestPasswordResetDtoNull_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var changePasswordDto = (ChangePasswordDto)null;

            var userServiceMock = Substitute.For<IUserService>();

            var userController = new UserController(userServiceMock);
            var result = await userController.ChangePassword(changePasswordDto, userIdMock);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ChangePassword_NoCorrectEmail_Fails()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = Guid.NewGuid();
            var newPasswordMock = _Fixture.Create<string>();
            var oldPasswordMock = _Fixture.Create<string>();

            var changePasswordDto = _Fixture.Build<ChangePasswordDto>()
                .With(x => x.OldPassword, oldPasswordMock)
                .With(x => x.NewPassword, newPasswordMock)
                .Create();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.ChangePassword(userIdMock, oldPasswordMock, newPasswordMock).Returns(false);

            var userController = new UserController(userServiceMock);
            var result = await userController.ChangePassword(changePasswordDto, userIdMock);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ChangePassword_CorrectEmail_Succeeds()
        {
            var userIdMock = Guid.NewGuid();
            var tokenMock = Guid.NewGuid();
            var newPasswordMock = _Fixture.Create<string>();
            var oldPasswordMock = _Fixture.Create<string>();

            var changePasswordDto = _Fixture.Build<ChangePasswordDto>()
                .With(x => x.OldPassword, oldPasswordMock)
                .With(x => x.NewPassword, newPasswordMock)
                .Create();

            var userServiceMock = Substitute.For<IUserService>();
            userServiceMock.ChangePassword(userIdMock, oldPasswordMock, newPasswordMock).Returns(true);

            var userController = new UserController(userServiceMock);
            var result = await userController.ChangePassword(changePasswordDto, userIdMock);

            result.Should().BeOfType<OkResult>();
        }
    }
}
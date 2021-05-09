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

            var userMock = _Fixture.Build<DungeonUser>()
                .With(x => x.Email, emailMock)
                .With(x => x.UserName, emailMock)
                .Create();


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
            result.Should().Be(false);
        }
    }
}
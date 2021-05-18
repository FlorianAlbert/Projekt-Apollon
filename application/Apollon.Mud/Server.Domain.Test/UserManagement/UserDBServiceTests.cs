using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations.User;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.UserManagement
{
    
    public class UserDbServiceTests
    {
        private readonly IFixture _Fixture;

        public UserDbServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-42)]
        public async Task IsAdminRegistered_NoAdmin(int adminsCount)
        {
            var dungeonUserListMock = Substitute.For<IList<DungeonUser>>();
            dungeonUserListMock.Count.Returns(adminsCount);

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

            userManagerMock.GetUsersInRoleAsync(Roles.Admin.ToString()).Returns(dungeonUserListMock);

            var dbServiceMock = Substitute.For<IGameDbService>();


            var userDbService = new UserDbService(userManagerMock, dbServiceMock);
            var isAdminRegistered = await userDbService.IsAdminRegistered();

            
            await userManagerMock.Received().GetUsersInRoleAsync(Roles.Admin.ToString());
            Assert.False(isAdminRegistered);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(42)]
        public async Task IsAdminRegistered_AdminRegistered(int adminsCount)
        {
            var dungeonUserListMock = Substitute.For<IList<DungeonUser>>();
            dungeonUserListMock.Count.Returns(adminsCount);

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
            userManagerMock.GetUsersInRoleAsync(Roles.Admin.ToString()).Returns(dungeonUserListMock);

            var dbServiceMock = Substitute.For<IGameDbService>();


            var userDbService = new UserDbService(userManagerMock, dbServiceMock);
            var isAdminRegistered = await userDbService.IsAdminRegistered();


            await userManagerMock.Received().GetUsersInRoleAsync(Roles.Admin.ToString());
            Assert.True(isAdminRegistered);
        }

        [Fact]
        public async Task CreateUser_UserCreation_Failed()
        {
            var userName = _Fixture.Create<string>();
            var password = _Fixture.Create<string>();
            var dungeonUserMock = _Fixture
                .Build<DungeonUser>()
                .With(x => x.UserName, userName)
                .Create();
            var identInResult = IdentityResult.Failed();

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
            userManagerMock.CreateAsync(dungeonUserMock, password).Returns(identInResult);

            var dbServiceMock = Substitute.For<IGameDbService>();


            var userDbService = new UserDbService(userManagerMock, dbServiceMock);
            var userCreation = await userDbService.CreateUser(dungeonUserMock, password);


            await userManagerMock.Received().CreateAsync(dungeonUserMock, password);
            Assert.False(userCreation);
        }

        [Fact]
        public async Task CreateUser_AddPlayer_Failed()
        {
            var userName = _Fixture.Create<string>();
            var password = _Fixture.Create<string>();
            var dungeonUserMock = _Fixture
                .Build<DungeonUser>()
                .With(x => x.UserName, userName)
                .Create();

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
            userManagerMock.CreateAsync(dungeonUserMock, password).Returns(IdentityResult.Success);
            userManagerMock.AddToRoleAsync(dungeonUserMock, Roles.Player.ToString()).Returns(IdentityResult.Failed());
            userManagerMock.DeleteAsync(dungeonUserMock).Returns(IdentityResult.Success);

            var dbServiceMock = Substitute.For<IGameDbService>();


            var userDbService = new UserDbService(userManagerMock, dbServiceMock);
            var userCreation = await userDbService.CreateUser(dungeonUserMock, password);


            await userManagerMock.Received().CreateAsync(dungeonUserMock, password);
            await userManagerMock.Received().AddToRoleAsync(dungeonUserMock, Roles.Player.ToString());
            await userManagerMock.Received().DeleteAsync(dungeonUserMock);
            Assert.False(userCreation);
        }

        [Fact]
        public async Task CreateUser_AddAdmin_Failed()
        {
            var userName = _Fixture.Create<string>();
            var password = _Fixture.Create<string>();
            var dungeonUserMock = _Fixture
                .Build<DungeonUser>()
                .With(x => x.UserName, userName)
                .Create();

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
            userManagerMock.CreateAsync(dungeonUserMock, password).Returns(IdentityResult.Success);
            userManagerMock.AddToRoleAsync(dungeonUserMock, Roles.Player.ToString()).Returns(IdentityResult.Success);
            userManagerMock.AddToRoleAsync(dungeonUserMock, Roles.Admin.ToString()).Returns(IdentityResult.Failed());
            userManagerMock.DeleteAsync(dungeonUserMock).Returns(IdentityResult.Success);

            var dbServiceMock = Substitute.For<IGameDbService>();


            var userDbService = new UserDbService(userManagerMock, dbServiceMock);
            var userCreation = await userDbService.CreateUser(dungeonUserMock, password, true);


            await userManagerMock.Received().CreateAsync(dungeonUserMock, password);
            await userManagerMock.Received().AddToRoleAsync(dungeonUserMock, Roles.Player.ToString());
            await userManagerMock.Received().AddToRoleAsync(dungeonUserMock, Roles.Admin.ToString());
            await userManagerMock.Received().DeleteAsync(dungeonUserMock);
            Assert.False(userCreation);
        }

        [Fact]
        public async Task CreateUser_AsPlayer_Success()
        {
            var userName = _Fixture.Create<string>();
            var password = _Fixture.Create<string>();
            var dungeonUserMock = _Fixture
                .Build<DungeonUser>()
                .With(x => x.UserName, userName)
                .Create();

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
            userManagerMock.CreateAsync(dungeonUserMock, password).Returns(IdentityResult.Success);
            userManagerMock.AddToRoleAsync(dungeonUserMock, Roles.Player.ToString()).Returns(IdentityResult.Success);

            var dbServiceMock = Substitute.For<IGameDbService>();


            var userDbService = new UserDbService(userManagerMock, dbServiceMock);
            var userCreation = await userDbService.CreateUser(dungeonUserMock, password);


            await userManagerMock.Received().CreateAsync(dungeonUserMock, password);
            await userManagerMock.Received().AddToRoleAsync(dungeonUserMock, Roles.Player.ToString());
            Assert.True(userCreation);
        }

        [Fact]
        public async Task CreateUser_AsAdmin_Success()
        {
            var userName = _Fixture.Create<string>();
            var password = _Fixture.Create<string>();
            var dungeonUserMock = _Fixture
                .Build<DungeonUser>()
                .With(x => x.UserName, userName)
                .Create();

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
            userManagerMock.CreateAsync(dungeonUserMock, password).Returns(IdentityResult.Success);
            userManagerMock.AddToRoleAsync(dungeonUserMock, Roles.Player.ToString()).Returns(IdentityResult.Success);
            userManagerMock.AddToRoleAsync(dungeonUserMock, Roles.Admin.ToString()).Returns(IdentityResult.Success);

            var dbServiceMock = Substitute.For<IGameDbService>();


            var userDbService = new UserDbService(userManagerMock, dbServiceMock);
            var userCreation = await userDbService.CreateUser(dungeonUserMock, password, true);


            await userManagerMock.Received().CreateAsync(dungeonUserMock, password);
            await userManagerMock.Received().AddToRoleAsync(dungeonUserMock, Roles.Player.ToString());
            await userManagerMock.Received().AddToRoleAsync(dungeonUserMock, Roles.Admin.ToString());
            Assert.True(userCreation);
        }
    }
}
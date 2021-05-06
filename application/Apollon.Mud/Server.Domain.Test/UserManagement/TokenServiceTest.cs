using System;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.UserManagement
{
    public class TokenServiceTest
    {
        private IFixture _Fixture;

        public TokenServiceTest()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        //Because of the circular dependency of the methods and private "stores" there is no way to test the methods independent
        [Fact] 
        public void Confirmation_AllInOne()
        {
            var tokenService = new TokenService();
            var emptyGuid = new Guid();


            var confirmationToken = tokenService.GenerateNewConfirmationToken();


            Assert.True(tokenService.CheckConfirmationToken(confirmationToken));
            Assert.False(tokenService.CheckConfirmationToken(confirmationToken));
            Assert.False(tokenService.CheckConfirmationToken(emptyGuid));
            Assert.NotEqual(confirmationToken, emptyGuid);
        }

        //Because of the circular dependency of the methods and private "stores" there is no way to test the methods independent
        [Fact]
        public void Reset_AllInOne()
        {
            var tokenService = new TokenService();
            var emptyGuid = new Guid();


            var resetToken = tokenService.GenerateNewResetToken();


            Assert.True(tokenService.CheckResetToken(resetToken));
            Assert.False(tokenService.CheckResetToken(resetToken));
            Assert.False(tokenService.CheckResetToken(emptyGuid));
            Assert.NotEqual(resetToken, emptyGuid);
        }
    }
}
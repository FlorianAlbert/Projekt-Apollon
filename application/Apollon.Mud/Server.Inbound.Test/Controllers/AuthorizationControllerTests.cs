using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Inbound.Test.Controllers
{
    public class AuthorizationControllerTests
    {
        private IFixture _Fixture;
        public AuthorizationControllerTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public void UnauthorizedLogin_Fails()
        {
            var authorizationRequestDto = _Fixture.Create<AuthorizationRequestDto>();
            authorizationRequestDto.UserEmail.Returns("");

            var _authorizationService = Substitute.For<IAuthorizationService>();


        }

        [Fact]
        public void BadRequestLogin_Fails()
        {

        }

        [Fact]
        public void Login_Succeds()
        {

        }

    }
}
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
        public void FirstTest()
        {

        }

    }
}
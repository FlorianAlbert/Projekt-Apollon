using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace Apollon.Mud.Server.Test.Controllers
{
    public class AuthorizationControllerTests
    {
        private IFixture _Fixture;
        public AuthorizationControllerTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

    }
}
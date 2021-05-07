using AutoFixture;
using AutoFixture.AutoNSubstitute;

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
    }
}
using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.UserManagement
{
    
    public class UserDbServiceTests
    {
        //ToDo Tests
        private IFixture _Fixture;

        public UserDbServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }
    }
}
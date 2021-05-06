using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.UserManagement
{
    public class AuthorizationServiceTests
    {
        //ToDo Tests
        private IFixture _Fixture;

        public AuthorizationServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }
    }
}
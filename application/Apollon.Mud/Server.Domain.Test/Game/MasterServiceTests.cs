using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.Game
{
    public class MasterServiceTests
    {
        //ToDo Tests
        private IFixture _Fixture;

        public MasterServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }
    }
}
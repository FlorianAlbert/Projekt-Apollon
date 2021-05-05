using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.Shared
{
    public class GameDbServiceTests
    {
        //ToDo Test
        private IFixture _Fixture;

        public GameDbServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }
    }
}
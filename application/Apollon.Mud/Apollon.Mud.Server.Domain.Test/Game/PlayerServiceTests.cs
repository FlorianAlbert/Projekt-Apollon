using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.Game
{
    public class PlayerServiceTests
    {
        //ToDo Test
        private IFixture _Fixture;

        public PlayerServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }
    }
}
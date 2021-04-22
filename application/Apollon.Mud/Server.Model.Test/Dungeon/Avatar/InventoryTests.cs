using Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Apollon.Mud.Server.Model.Test.Dungeon.Avatar
{
    public class InventoryTests
    {
        private Fixture _Fixture;

        public InventoryTests()
        {
            _Fixture = new Fixture();
        }

        [Fact]
        public void Add_WeightSumUnderMaximum_Success()
        {
            var takeableMock = Substitute.For<ITakeable>();
            takeableMock.Weight.Returns(23);
            var inventory = _Fixture.Create<Inventory>();

            inventory.Add(takeableMock);

            inventory.Should().Contain(takeableMock);
        }

        [Fact]
        public void Add_WeightSumOverMaximum_Fail()
        {
            var takeableMock = Substitute.For<ITakeable>();
            takeableMock.Weight.Returns(150);
            var inventory = _Fixture.Create<Inventory>();

            inventory.Add(takeableMock);

            inventory.Should().BeEmpty(); ;
        }

        [Fact]
        public void Add_TakeableIsNull_Fail()
        {
            var inventory = _Fixture.Create<Inventory>();

            inventory.Add(null);

            inventory.Should().BeEmpty();
        }
    }
}

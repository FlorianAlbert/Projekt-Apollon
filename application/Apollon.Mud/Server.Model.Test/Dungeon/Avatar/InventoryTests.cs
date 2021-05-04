using Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
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

        [Theory]
        [InlineData(100)]
        [InlineData(73)]
        [InlineData(0)]
        public void Add_WeightSumUnderMaximum_Success(int newWeight)
        {
            var takeableMock = Substitute.For<ITakeable>();
            takeableMock.Weight.Returns(newWeight);
            var inventory = _Fixture.Create<Inventory>();

            inventory.Add(takeableMock);

            inventory.Should().Contain(takeableMock);
        }

        [Theory]
        [InlineData(0, 146)]
        [InlineData(63, 72)]
        [InlineData(100, 4)]
        public void Add_WeightSumOverMaximum_Fail(int alreadyContainingWeight, int newWeight)
        {
            var takeableMock = Substitute.For<ITakeable>();
            takeableMock.Weight.Returns(newWeight);
            var inventory = Substitute.For<IInventory>();
            inventory.WeightSum.Returns(alreadyContainingWeight);

            inventory.Add(takeableMock);

            inventory.Should().BeEmpty();
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

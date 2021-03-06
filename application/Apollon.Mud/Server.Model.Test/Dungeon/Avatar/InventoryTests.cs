
using System.Collections.Generic;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Shared.Implementations.Dungeons;
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
            var takeableMock = _Fixture.Build<Takeable>()
                .Without(x => x.Dungeon)
                .With(x => x.Weight, newWeight)
                .Create();
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
            var takeableMock = _Fixture.Build<Takeable>()
                .Without(x => x.Dungeon)
                .With(x => x.Weight, newWeight)
                .With(x => x.Status, Status.Approved)
                .Create();

            var inventoryItemMock = _Fixture.Build<Takeable>()
                .Without(x => x.Dungeon)
                .With(x => x.Weight, alreadyContainingWeight)
                .With(x => x.Status, Status.Approved)
                .Create();

            var inventory = new Inventory(new List<Takeable>() {inventoryItemMock});


            inventory.Add(takeableMock);

            
            inventory.WeightSum.Should().Be(alreadyContainingWeight);
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

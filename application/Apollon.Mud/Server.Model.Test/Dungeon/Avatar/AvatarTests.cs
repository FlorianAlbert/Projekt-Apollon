using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Races;
using AutoFixture;
using Xunit;
using NSubstitute;
using FluentAssertions;
using AutoFixture.AutoNSubstitute;

namespace Apollon.Mud.Server.Model.Test.Dungeon.Avatar
{
    public class AvatarTests
    {
        private IFixture _Fixture;
        public AvatarTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Theory]
        [InlineData(22, 21, 43)]
        [InlineData(24, 23, 47)]
        [InlineData(125, 55, 180)]
        public void MaxHealth_SumRaceAndClass_Success(int raceHealth, int classHealth, int expectedSum)
        {
            var raceMock = Substitute.For<Race>();
            raceMock.DefaultHealth.Returns(raceHealth);
            var classMock = Substitute.For<Class>();
            classMock.DefaultHealth.Returns(classHealth);
            var avatar = _Fixture.Build<Implementations.Dungeons.Avatars.Avatar>().With(x => x.ChosenRace, raceMock).With(x => x.ChosenClass, classMock).Create();

            var result = avatar.MaxHealth;

            result.Should().Be(expectedSum);
        }


        [Theory]
        [InlineData(22, 21, 20, 63)]
        [InlineData(24, 23, 22, 69)]
        [InlineData(125, 55, 100, 280)]
        public void Damage_SumRaceAndClassWithWeaponNotNull_Success(int raceDamage, int classDamage, int weaponBoost, int expectedSum)
        {
            //var raceMock = Substitute.For<Race>();
            var raceMock = _Fixture.Create<Race>();
            raceMock.DefaultDamage = raceDamage;
            //var classMock = Substitute.For<Class>();
            var classMock = _Fixture.Create<Class>();
            classMock.DefaultDamage = classDamage;
            //var weaponMock = Substitute.For<Usable>();
            var weaponMock = _Fixture.Create<Usable>();
            weaponMock.DamageBoost = weaponBoost;

            var avatar = _Fixture.Build<Implementations.Dungeons.Avatars.Avatar>()
                                 .With(x => x.ChosenRace, raceMock)
                                 .With(x => x.ChosenClass, classMock)
                                 .With(x => x.HoldingItem, weaponMock)
                                 .Create();

            var result = avatar.Damage;

            result.Should().Be(expectedSum);
        }

        [Theory]
        [InlineData(21, 20, 41)]
        [InlineData(23, 22, 45)]
        [InlineData(55, 100, 155)]
        public void Damage_SumRaceAndClassWithWeaponNull_Success(int raceDamage, int classDamage, int expectedSum)
        {
            var raceMock = Substitute.For<Race>();
            raceMock.DefaultDamage.Returns(raceDamage);
            var classMock = Substitute.For<Class>();
            classMock.DefaultDamage.Returns(classDamage);
            var avatar = _Fixture.Build<Implementations.Dungeons.Avatars.Avatar>().With(x => x.ChosenRace, raceMock).With(x => x.ChosenClass, classMock).With(x => x.HoldingItem, null as Usable).Create();

            var result = avatar.Damage;

            result.Should().Be(expectedSum);
        }


        [Theory]
        [InlineData(22, 21, 20, 63)]
        [InlineData(24, 23, 22, 69)]
        [InlineData(125, 55, 100, 280)]
        public void Protection_SumRaceAndClassWithArmorNotNull_Success(int raceProtection, int classProtection, int protectionBoost, int expectedSum)
        {
            var raceMock = Substitute.For<Race>();
            raceMock.DefaultProtection.Returns(raceProtection);
            var classMock = Substitute.For<Class>();
            classMock.DefaultProtection.Returns(classProtection);
            var protectionMock = Substitute.For<Wearable>();
            protectionMock.ProtectionBoost.Returns(protectionBoost);
            var avatar = _Fixture.Build<Implementations.Dungeons.Avatars.Avatar>().With(x => x.ChosenRace, raceMock).With(x => x.ChosenClass, classMock).With(x => x.Armor, protectionMock).Create();

            var result = avatar.Protection;

            result.Should().Be(expectedSum);
        }

        [Theory]
        [InlineData(21, 20, 41)]
        [InlineData(23, 22, 45)]
        [InlineData(55, 100, 155)]
        public void Protection_SumRaceAndClassWithArmorNull_Success(int raceProtection, int classProtection, int expectedSum)
        {
            var raceMock = _Fixture.Create<Race>();
            raceMock.DefaultProtection = raceProtection;
            var classMock = _Fixture.Create<Class>();
            classMock.DefaultProtection = classProtection;
            var avatar = _Fixture.Build<Implementations.Dungeons.Avatars.Avatar>().With(x => x.ChosenRace, raceMock).With(x => x.ChosenClass, classMock).With(x => x.Armor, null as Wearable).Create();

            var result = avatar.Protection;

            result.Should().Be(expectedSum);
        }
        
        /*
        [Fact]
        public void AddItemToInventoryWithNotNull_Success()
        {
            var takeableMock = Substitute.For<ITakeable>();
            var inventoryMock = Substitute.For<IInventory>();
            inventoryMock.Contains(takeableMock).Returns(true);
            var avatar = _Fixture.Build<Implementations.Dungeon.Avatar.Avatar>().With(x => x.Inventory, inventoryMock).Create();

            var result = avatar.AddItemToInventory(takeableMock);

            Received.InOrder(() =>
            {
                inventoryMock.Add(takeableMock);
                inventoryMock.Contains(takeableMock);
            });

            result.Should().Be(true);
        }

        [Fact]
        public void AddItemToInventoryWithNull_Fail()
        {
            var inventoryMock = Substitute.For<IInventory>();
            inventoryMock.Contains(null).Returns(false);
            var avatar = _Fixture.Build<Implementations.Dungeon.Avatar.Avatar>().With(x => x.Inventory, inventoryMock).Create();

            var result = avatar.AddItemToInventory(null);

            inventoryMock.DidNotReceive().Add(Arg.Any<ITakeable>());
            inventoryMock.Received(1).Contains(null);
            result.Should().Be(false);
        }

        [Fact]
        public void ConsumeItem_Success()
        {
            var consumableMock = Substitute.For<IConsumable>();
            var inventoryMock = Substitute.For<IInventory>();
            inventoryMock.FirstOrDefault(Arg.Any<Func<ITakeable, bool>>()).Returns(consumableMock);
            var avatar = _Fixture.Build<Implementations.Dungeon.Avatar.Avatar>().With(x => x.Inventory, inventoryMock).Create();
            
            var consumeResult = avatar.ConsumeItem(consumableMock.Name);

            consumeResult.Should().Be(consumableMock.EffectDescription);
        }

        [Fact]
        public void ConsumeItem_ItemNotConsumable_Fail()
        {
            var takeableMock = Substitute.For<ITakeable>();
            var inventoryMock = Substitute.For<IInventory>();
            inventoryMock.FirstOrDefault(x => x.Name == takeableMock.Name).Returns(takeableMock);
            var avatar = _Fixture.Build<Implementations.Dungeon.Avatar.Avatar>().With(x => x.Inventory, inventoryMock).Create();

            var consumeResult = avatar.ConsumeItem(takeableMock.Name);

            consumeResult.Should().Be("Dieses Item kannst du nicht konsumieren.");
        }

        [Fact]
        public void ConsumeItem_ItemNotInInventory_Fail()
        {
            var itemName = _Fixture.Create<string>();
            var inventoryMock = Substitute.For<IInventory>();
            inventoryMock.FirstOrDefault(Arg.Any<Func<ITakeable, bool>>()).Returns(null as ITakeable);
            var avatar = _Fixture.Build<Implementations.Dungeon.Avatar.Avatar>().With(x => x.Inventory, inventoryMock).Create();

            var consumeResult = avatar.ConsumeItem(itemName);

            consumeResult.Should().Be("Dieses Item befindet sich nicht in deinem Inventar.");
            Received.InOrder(() =>
            {
                inventoryMock.FirstOrDefault(Arg.Any<Func<ITakeable, bool>>());
            });
        }

        [Fact]
        public void ThrowAway_Success()
        {
            var takeableMock = Substitute.For<ITakeable>();
            var inventoryMock = Substitute.For<IInventory>();
            inventoryMock.FirstOrDefault(Arg.Any<Func<ITakeable, bool>>()).Returns(takeableMock);
            var avatar = _Fixture.Build<Implementations.Dungeon.Avatar.Avatar>().With(x => x.Inventory, inventoryMock).Create();

            var throwResult = avatar.ThrowAway(takeableMock.Name);

            throwResult.Should().BeSameAs(takeableMock);
        }

        [Fact]
        public void ThrowAway_ItemNotInInventory_Fail()
        {
            var itemName = _Fixture.Create<string>();
            var inventoryMock = Substitute.For<IInventory>();
            inventoryMock.FirstOrDefault(Arg.Any<Func<ITakeable, bool>>()).Returns(null as ITakeable);
            var avatar = _Fixture.Build<Implementations.Dungeon.Avatar.Avatar>().With(x => x.Inventory, inventoryMock).Create();

            var throwResult = avatar.ThrowAway(itemName);

            throwResult.Should().BeNull();
        } */
    }
}
using System;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Races;
using Apollon.Mud.Shared.Dungeon.Avatar;
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
            var raceMock = _Fixture.Build<Race>()
                .With(x => x.DefaultHealth, raceHealth)
                .Without(x => x.Dungeon)
                .Create();
            raceMock.DefaultHealth = raceHealth;
            var classMock = _Fixture.Build<Class>()
                .With(x => x.DefaultHealth, classHealth)
                .Without(x => x.Dungeon)
                .Create();
            classMock.DefaultHealth = classHealth;
            var avatar = new Implementations.Dungeons.Avatars.Avatar(string.Empty, raceMock, classMock, Gender.Divers)
            {
                Dungeon = new Implementations.Dungeons.Dungeon(string.Empty, string.Empty, string.Empty)
            };

            var result = avatar.MaxHealth;

            result.Should().Be(expectedSum);
        }


        [Theory]
        [InlineData(22, 21, 20, 63)]
        [InlineData(24, 23, 22, 69)]
        [InlineData(125, 55, 100, 280)]
        public void Damage_SumRaceAndClassWithWeaponNotNull_Success(int raceDamage, int classDamage, int weaponBoost, int expectedSum)
        {
            var raceMock = _Fixture.Build<Race>()
                .With(x => x.DefaultDamage, raceDamage)
                .Without(x => x.Dungeon)
                .Create();
            var classMock = _Fixture.Build<Class>()
                .With(x => x.DefaultDamage, classDamage)
                .Without(x => x.Dungeon)
                .Create();
            var weaponMock = _Fixture.Build<Usable>()
                .With(x => x.DamageBoost, weaponBoost)
                .Without(x => x.Dungeon)
                .Create();

            var avatar = new Implementations.Dungeons.Avatars.Avatar(string.Empty, raceMock, classMock, Gender.Divers)
            {
                Dungeon = new Implementations.Dungeons.Dungeon(string.Empty, string.Empty, string.Empty),
                HoldingItem = weaponMock
            };

            var result = avatar.Damage;

            result.Should().Be(expectedSum);
        }

        [Theory]
        [InlineData(21, 20, 41)]
        [InlineData(23, 22, 45)]
        [InlineData(55, 100, 155)]
        public void Damage_SumRaceAndClassWithWeaponNull_Success(int raceDamage, int classDamage, int expectedSum)
        {
            var raceMock = _Fixture.Build<Race>()
                .With(x => x.DefaultDamage, raceDamage)
                .Without(x => x.Dungeon)
                .Create();
            var classMock = _Fixture.Build<Class>()
                .With(x => x.DefaultDamage, classDamage)
                .Without(x => x.Dungeon)
                .Create();
            var avatar = new Implementations.Dungeons.Avatars.Avatar(string.Empty, raceMock, classMock, Gender.Divers)
            {
                Dungeon = new Implementations.Dungeons.Dungeon(string.Empty, string.Empty, string.Empty)
            };

            var result = avatar.Damage;

            result.Should().Be(expectedSum);
        }


        [Theory]
        [InlineData(22, 21, 20, 63)]
        [InlineData(24, 23, 22, 69)]
        [InlineData(125, 55, 100, 280)]
        public void Protection_SumRaceAndClassWithArmorNotNull_Success(int raceProtection, int classProtection, int protectionBoost, int expectedSum)
        {
            var raceMock = _Fixture.Build<Race>()
                .With(x => x.DefaultProtection, raceProtection)
                .Without(x => x.Dungeon)
                .Create();
            var classMock = _Fixture.Build<Class>()
                .With(x => x.DefaultProtection, classProtection)
                .Without(x => x.Dungeon)
                .Create();
            var protectionMock = _Fixture.Build<Wearable>()
                .With(x => x.ProtectionBoost, protectionBoost)
                .Without(x => x.Dungeon)
                .Create();

            var avatar = new Implementations.Dungeons.Avatars.Avatar(string.Empty, raceMock, classMock, Gender.Divers)
            {
                Dungeon = new Implementations.Dungeons.Dungeon(string.Empty, string.Empty, string.Empty),
                Armor = protectionMock
            };

            var result = avatar.Protection;

            result.Should().Be(expectedSum);
        }

        [Theory]
        [InlineData(21, 20, 41)]
        [InlineData(23, 22, 45)]
        [InlineData(55, 100, 155)]
        public void Protection_SumRaceAndClassWithArmorNull_Success(int raceProtection, int classProtection, int expectedSum)
        {
            var raceMock = _Fixture.Build<Race>()
                .With(x => x.DefaultProtection, raceProtection)
                .Without(x => x.Dungeon)
                .Create();
            var classMock = _Fixture.Build<Class>()
                .With(x => x.DefaultProtection, classProtection)
                .Without(x => x.Dungeon)
                .Create();

            var avatar = new Implementations.Dungeons.Avatars.Avatar(string.Empty, raceMock, classMock, Gender.Divers)
            {
                Dungeon = new Implementations.Dungeons.Dungeon(string.Empty, string.Empty, string.Empty)
            };

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
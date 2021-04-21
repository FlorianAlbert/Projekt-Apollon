using AutoFixture;
using System;
using Xunit;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar;
using NSubstitute;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Wearable;
using FluentAssertions;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon;
using Apollon.Mud.Server.Model.Implementations.User;
using AutoFixture.AutoNSubstitute;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;

namespace Apollon.Mud.Server.Model.Test
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
            var raceMock = Substitute.For<IRace>();
            raceMock.DefaultHealth.Returns(raceHealth);
            var classMock = Substitute.For<IClass>();
            classMock.DefaultHealth.Returns(classHealth);
            var avatar = _Fixture.Build<Avatar>().With(x => x.Race, raceMock).With(x => x.Class, classMock).Create();

            var result = avatar.MaxHealth;

            result.Should().Be(expectedSum);
        }


        [Theory]
        [InlineData(22, 21, 20, 63)]
        [InlineData(24, 23, 22, 69)]
        [InlineData(125, 55, 100, 280)]
        public void Damage_SumRaceAndClassWithWeaponNotNull_Success(int raceDamage, int classDamage, int weaponBoost, int expectedSum)
        {
            var raceMock = Substitute.For<IRace>();
            raceMock.DefaultDamage.Returns(raceDamage);
            var classMock = Substitute.For<IClass>();
            classMock.DefaultDamage.Returns(classDamage);
            var weaponMock = Substitute.For<IUsable>();
            weaponMock.DamageBoost.Returns(weaponBoost);
            var avatar = _Fixture.Build<Avatar>().With(x => x.Race, raceMock).With(x => x.Class, classMock).With(x => x.HoldingItem, weaponMock).Create();

            var result = avatar.Damage;

            result.Should().Be(expectedSum);
        }

        [Theory]
        [InlineData(21, 20, 41)]
        [InlineData(23, 22, 45)]
        [InlineData(55, 100, 155)]
        public void Damage_SumRaceAndClassWithWeaponNull_Success(int raceDamage, int classDamage, int expectedSum)
        {
            var raceMock = Substitute.For<IRace>();
            raceMock.DefaultDamage.Returns(raceDamage);
            var classMock = Substitute.For<IClass>();
            classMock.DefaultDamage.Returns(classDamage);
            var avatar = _Fixture.Build<Avatar>().With(x => x.Race, raceMock).With(x => x.Class, classMock).With(x => x.HoldingItem, null as IUsable).Create();

            var result = avatar.Damage;

            result.Should().Be(expectedSum);
        }


        [Theory]
        [InlineData(22, 21, 20, 63)]
        [InlineData(24, 23, 22, 69)]
        [InlineData(125, 55, 100, 280)]
        public void Protection_SumRaceAndClassWithArmorNotNull_Success(int raceProtection, int classProtection, int protectionBoost, int expectedSum)
        {
            var raceMock = Substitute.For<IRace>();
            raceMock.DefaultProtection.Returns(raceProtection);
            var classMock = Substitute.For<IClass>();
            classMock.DefaultProtection.Returns(classProtection);
            var protectionMock = Substitute.For<IWearable>();
            protectionMock.ProtectionBoost.Returns(protectionBoost);
            var avatar = _Fixture.Build<Avatar>().With(x => x.Race, raceMock).With(x => x.Class, classMock).With(x => x.Armor, protectionMock).Create();

            var result = avatar.Protection;

            result.Should().Be(expectedSum);
        }

        [Theory]
        [InlineData(21, 20, 41)]
        [InlineData(23, 22, 45)]
        [InlineData(55, 100, 155)]
        public void Protection_SumRaceAndClassWithArmorNull_Success(int raceProtection, int classProtection, int expectedSum)
        {
            var raceMock = Substitute.For<IRace>();
            raceMock.DefaultProtection.Returns(raceProtection);
            var classMock = Substitute.For<IClass>();
            classMock.DefaultProtection.Returns(classProtection);   
            var avatar = _Fixture.Build<Avatar>().With(x => x.Race, raceMock).With(x => x.Class, classMock).With(x => x.Armor, null as IWearable).Create();

            var result = avatar.Protection;

            result.Should().Be(expectedSum);
        }

        [Fact]
        public void AddItemToInventoryWithNotNull_Success()
        {
            var takeableMock = Substitute.For<ITakeable>();
            var avatar = _Fixture.Create<Avatar>();

            avatar.AddItemToInventory 

        }

        [Fact]
        public void AddItemToInventoryWithNull_Fail()
        {

        }

        [Theory]
        [InlineData()]

        public void ConsumeItem_Success()
        {

        }
    }
}

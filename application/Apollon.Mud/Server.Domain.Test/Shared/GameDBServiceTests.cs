using System;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.DbContext;
using Apollon.Mud.Server.Domain.Implementations.Shared;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Apollon.Mud.Server.Domain.Test.Shared
{
    public class GameDbServiceTests
    {
        //ToDo DungeonDbContext kann nicht gemockt werden...
        private IFixture _Fixture;

        public GameDbServiceTests()
        {
            _Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }
    }
}
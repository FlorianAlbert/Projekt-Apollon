
using System;
using System.Collections.Generic;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Races;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.Domain.DbContext
{
    public class DungeonDbContext : IdentityDbContext<DungeonUser>
    {
        public DungeonDbContext(DbContextOptions<DungeonDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dungeon> Dungeons { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<Race> Races { get; set; }

        public DbSet<Avatar> Avatars { get; set; }

        public DbSet<Npc> Npcs { get; set; }

        public DbSet<Requestable> SpecialActions { get; set; }

        public DbSet<Consumable> Consumables { get; set; }

        public DbSet<Usable> Usables { get; set; }

        public DbSet<Wearable> Wearables { get; set; }

        public DbSet<Takeable> Takeables { get; set; }

        public DbSet<Inspectable> Inspectables { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Dungeon>()
                .HasKey(e => e.Id);
            builder.Entity<Dungeon>()
                .HasMany(e => e.BlackList);
            builder.Entity<Dungeon>()
                .HasMany(e => e.WhiteList);
            builder.Entity<Dungeon>()
                .HasMany(e => e.DungeonMasters);
            builder.Entity<Dungeon>()
                .HasMany(e => e.OpenRequests);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredRaces);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredClasses);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredInspectables);    // TODO: evtl falsch, prüfen!
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredRequestables);
            builder.Entity<Dungeon>()
                .HasMany(e => e.RegisteredAvatars)
                .WithOne(x => x.Dungeon);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredRooms);
            builder.Entity<Dungeon>()
                .HasOne(e => e.CurrentDungeonMaster);
            builder.Entity<Dungeon>()
                .HasOne(e => e.DefaultRoom);
            builder.Entity<Dungeon>()
                .HasOne(e => e.DungeonOwner);
            builder.Entity<Dungeon>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Room>()
                .HasKey(e => e.Id);
            builder.Entity<Room>()
                .HasMany(e => e.Inspectables);
            builder.Entity<Room>()
                .HasMany(e => e.SpecialActions);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborEast)
                .WithOne(x => x.NeighborWest);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborSouth)
                .WithOne(x => x.NeighborNorth);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborWest)
                .WithOne(x => x.NeighborEast);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborNorth)
                .WithOne(x => x.NeighborSouth);
            builder.Entity<Room>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Class>()
                .HasKey(e => e.Id);
            builder.Entity<Class>()
                .HasMany(e => e.StartInventory);
            builder.Entity<Class>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Race>()
                .HasKey(e => e.Id);
            builder.Entity<Race>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Inspectable>()
                .HasKey(e => e.Id); 
            builder.Entity<Inspectable>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Avatar>()
                .HasMany(e => e.Inventory);
            builder.Entity<Avatar>()
                .HasOne(e => e.Armor);
            builder.Entity<Avatar>()
                .HasOne(e => e.ChosenClass);
            builder.Entity<Avatar>()
                .HasOne(e => e.CurrentRoom)
                .WithMany(x => x.Inspectables as ICollection<Avatar>);  // CHECK: könnte schief gehen - denke des ist nicht richtig -Etienne
            builder.Entity<Avatar>()
                .HasOne(e => e.Dungeon)
                .WithMany(x => x.RegisteredAvatars);
            builder.Entity<Avatar>()
                .HasOne(e => e.HoldingItem);
            builder.Entity<Avatar>()
                .HasOne(e => e.Owner);
            builder.Entity<Avatar>()
                .HasOne(e => e.ChosenRace);
            builder.Entity<Avatar>()
                .Property(x => x.ChosenGender)
                .HasConversion(
                    x => (int) x,
                    x => (Gender) x);
            builder.Entity<Avatar>()
                .Property(x => x.Status)
                .HasConversion(
                    x => (int) x,
                    x => (Status) x);
            builder.Entity<Avatar>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Requestable>()
                .HasKey(e => e.Id);
            builder.Entity<Requestable>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Takeable>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Usable>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Wearable>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Consumable>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Npc>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));


            base.OnModelCreating(builder);
        }
    }
}
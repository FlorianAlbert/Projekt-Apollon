using System;
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
using Apollon.Mud.Shared.Dungeon.Avatar;
using Apollon.Mud.Shared.Implementations.Dungeons;
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
            builder.Entity<DungeonUser>()
                .HasMany(x => x.BlackListDungeons)
                .WithMany(x => x.BlackList);
            builder.Entity<DungeonUser>()
                .HasMany(x => x.WhiteListDungeons)
                .WithMany(x => x.WhiteList);
            builder.Entity<DungeonUser>()
                .HasMany(x => x.DungeonMasterDungeons)
                .WithMany(x => x.DungeonMasters);
            builder.Entity<DungeonUser>()
                .HasMany(x => x.DungeonOwnerDungeons)
                .WithOne(x => x.DungeonOwner)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<DungeonUser>()
                .HasMany(x => x.CurrentDungeonMasterDungeons)
                .WithOne(x => x.CurrentDungeonMaster)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<DungeonUser>()
                .HasMany(x => x.OpenRequestDungeons)
                .WithMany(x => x.OpenRequests);
            builder.Entity<DungeonUser>()
                .HasMany(x => x.Avatars)
                .WithOne(x => x.Owner)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Dungeon>()
                .HasKey(e => e.Id);
            builder.Entity<Dungeon>()
                .HasMany(e => e.BlackList)
                .WithMany(x => x.BlackListDungeons);
            builder.Entity<Dungeon>()
                .HasMany(e => e.WhiteList)
                .WithMany(x => x.WhiteListDungeons);
            builder.Entity<Dungeon>()
                .HasMany(e => e.DungeonMasters)
                .WithMany(x => x.DungeonMasterDungeons);
            builder.Entity<Dungeon>()
                .HasMany(e => e.OpenRequests)
                .WithMany(x => x.OpenRequestDungeons);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredRaces)
                .WithOne(x => x.Dungeon)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredClasses)
                .WithOne(x => x.Dungeon)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredInspectables)
                .WithOne(x => x.Dungeon)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredRequestables)
                .WithOne(x => x.Dungeon)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Dungeon>()
                .HasMany(e => e.RegisteredAvatars)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredRooms)
                .WithOne(x => x.Dungeon)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Dungeon>()
                .HasOne(e => e.CurrentDungeonMaster)
                .WithMany(x => x.CurrentDungeonMasterDungeons)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Dungeon>()
                .HasOne(e => e.DungeonOwner)
                .WithMany(x => x.DungeonOwnerDungeons)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Dungeon>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Room>()
                .HasKey(e => e.Id);
            builder.Entity<Room>()
                .HasMany(e => e.Inspectables)
                .WithMany(x => x.Rooms);
            builder.Entity<Room>()
                .HasMany(e => e.SpecialActions)
                .WithMany(x => x.Rooms);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborEast)
                .WithOne(x => x.NeighborWest)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborSouth)
                .WithOne(x => x.NeighborNorth)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborWest)
                .WithOne(x => x.NeighborEast)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborNorth)
                .WithOne(x => x.NeighborSouth)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Room>()
                .HasOne(x => x.Dungeon)
                .WithMany(x => x.ConfiguredRooms)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Room>()
                .HasOne<Dungeon>()
                .WithOne(x => x.DefaultRoom)
                .HasForeignKey<Dungeon>(x => x.DefaultRoomId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Room>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Class>()
                .HasKey(e => e.Id);
            builder.Entity<Class>()
                .HasMany(e => e.StartInventory)
                .WithMany(x => x.Classes);
            builder.Entity<Class>()
                .HasOne(x => x.Dungeon)
                .WithMany(x => x.ConfiguredClasses)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Class>()
                .HasMany(x => x.Avatars)
                .WithOne(x => x.ChosenClass)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Class>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Race>()
                .HasKey(e => e.Id);
            builder.Entity<Race>()
                .HasMany(x => x.Avatars)
                .WithOne(x => x.ChosenRace)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Race>()
                .HasOne(x => x.Dungeon)
                .WithMany(x => x.ConfiguredRaces)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Race>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Inspectable>()
                .HasKey(e => e.Id);
            builder.Entity<Inspectable>()
                .HasMany(x => x.Rooms)
                .WithMany(x => x.Inspectables);
            builder.Entity<Inspectable>()
                .HasOne(x => x.Dungeon)
                .WithMany(x => x.ConfiguredInspectables)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Inspectable>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Avatar>()
                .HasMany(e => e.Inventory)
                .WithMany(x => x.InventoryAvatars);
            builder.Entity<Avatar>()
                .HasOne(e => e.Armor)
                .WithMany(x => x.ArmorAvatars)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Avatar>()
                .HasOne(e => e.ChosenClass)
                .WithMany(x => x.Avatars)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Avatar>()
                .HasOne(e => e.CurrentRoom);
            builder.Entity<Avatar>()
                .HasOne(e => e.HoldingItem)
                .WithMany(x => x.HoldingItemAvatars)
                .OnDelete(DeleteBehavior.SetNull);
            builder.Entity<Avatar>()
                .HasOne(e => e.Owner)
                .WithMany(x => x.Avatars)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Avatar>()
                .HasOne(e => e.ChosenRace)
                .WithMany(x => x.Avatars)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Avatar>()
                .HasOne<Dungeon>()
                .WithMany(x => x.RegisteredAvatars)
                .OnDelete(DeleteBehavior.Cascade);
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
                .HasOne(x => x.Dungeon)
                .WithMany(x => x.ConfiguredRequestables)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Requestable>()
                .HasMany(x => x.Rooms)
                .WithMany(x => x.SpecialActions);
            builder.Entity<Requestable>()
                .Property(x => x.Id)
                .HasConversion(
                    x => x.ToString(),
                    x => Guid.Parse(x));

            builder.Entity<Takeable>()
                .HasMany(x => x.Classes)
                .WithMany(x => x.StartInventory); 
            builder.Entity<Takeable>()
                .HasMany(x => x.HoldingItemAvatars)
                .WithOne(x => x.HoldingItem)
                .OnDelete(DeleteBehavior.SetNull); 
            builder.Entity<Takeable>()
                .HasMany(x => x.InventoryAvatars)
                .WithMany(x => x.Inventory);
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
                .HasMany(x => x.ArmorAvatars)
                .WithOne(x => x.Armor)
                .OnDelete(DeleteBehavior.SetNull);
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
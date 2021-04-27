using Apollon.Mud.Server.Model.Implementations.Dungeon;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Class;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Npc;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Race;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Requestable;
using Apollon.Mud.Server.Model.Implementations.Dungeon.Room;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.DbContext
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
                .HasMany(e => e.RegisteredAvatars);
            builder.Entity<Dungeon>()
                .HasMany(e => e.ConfiguredRooms);
            builder.Entity<Dungeon>()
                .HasOne(e => e.CurrentDungeonMaster);
            builder.Entity<Dungeon>()
                .HasOne(e => e.DefaultRoom);
            builder.Entity<Dungeon>()
                .HasOne(e => e.DungeonOwner);

            builder.Entity<Room>()
                .HasKey(e => e.Id);
            builder.Entity<Room>()
                .HasMany(e => e.Inspectables);
            builder.Entity<Room>()
                .HasMany(e => e.SpecialActions);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborEast);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborSouth);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborWest);
            builder.Entity<Room>()
                .HasOne(e => e.NeighborNorth);

            builder.Entity<Class>()
                .HasKey(e => e.Id);
            builder.Entity<Class>()
                .HasMany(e => e.StartInventory);

            builder.Entity<Race>()
                .HasKey(e => e.Id);

            builder.Entity<Avatar>()
                .HasKey(e => e.Id);
            builder.Entity<Avatar>()
                .HasMany(e => e.Inventory);
            builder.Entity<Avatar>()
                .HasOne(e => e.Armor);
            builder.Entity<Avatar>()
                .HasOne(e => e.Class);
            builder.Entity<Avatar>()
                .HasOne(e => e.CurrentRoom);
            builder.Entity<Avatar>()
                .HasOne(e => e.Dungeon);
            builder.Entity<Avatar>()
                .HasOne(e => e.HoldingItem);
            builder.Entity<Avatar>()
                .HasOne(e => e.Owner);
            builder.Entity<Avatar>()
                .HasOne(e => e.Race);

            builder.Entity<Npc>()
                .HasKey(e => e.Id);

            builder.Entity<Requestable>()
                .HasKey(e => e.Id);

            builder.Entity<Consumable>()
                .HasKey(e => e.Id);

            builder.Entity<Usable>()
                .HasKey(e => e.Id);

            builder.Entity<Wearable>()
                .HasKey(e => e.Id);

            builder.Entity<Takeable>()
                .HasKey(e => e.Id);

            builder.Entity<Inspectable>()
                .HasKey(e => e.Id);


            base.OnModelCreating(builder);
        }
    }
}
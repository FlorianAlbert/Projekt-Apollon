using Apollon.Mud.Server.Model.Implementations.Dungeon;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.Interfaces.Dungeon;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.DbContext
{
    // TODO: Genauer diskutieren

    public class DungeonDbContext : IdentityDbContext<DungeonUser>
    {
        public DungeonDbContext(DbContextOptions<DungeonDbContext> options)
            : base(options)
        {
        }

        private DbSet<Dungeon> Dungeons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Dungeon>()
                .HasKey(e => e.Id);
            builder.Entity<Dungeon>()
                .HasMany(e => e.BlackList);

            base.OnModelCreating(builder);
        }
    }
}
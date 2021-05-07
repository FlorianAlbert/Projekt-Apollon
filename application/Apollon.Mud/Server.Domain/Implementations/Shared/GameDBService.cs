using Apollon.Mud.Server.Domain.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.DbContext;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.Domain.Implementations.Shared
{
    /// <inheritdoc cref="IGameDbService"/>
    public class GameDbService: IGameDbService
    {
        #region member
        /// <summary>
        /// The dbContext to work with the db.
        /// </summary>
        private readonly DungeonDbContext _dungeonDbContext;
        #endregion


        public GameDbService(DungeonDbContext dungeonDbContext)
        {
            _dungeonDbContext = dungeonDbContext;
        }

        #region methods
        /// <inheritdoc cref="IGameDbService.ValidateUserIsDungeonMaster"/>
        public async Task<bool> ValidateUserIsDungeonMaster(Guid dungeonId, Guid userId)
        {
            await using var transaction = await _dungeonDbContext.Database.BeginTransactionAsync();

            var dungeon = await _dungeonDbContext.Dungeons.FindAsync(dungeonId);
            if (dungeon is null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            var isDungeonMaster = dungeon.DungeonMasters.Any(x => x.Id == userId.ToString());
            await transaction.CommitAsync();
            return isDungeonMaster;
        }

        /// <inheritdoc cref="IGameDbService.Delete{T}"/>
        async Task<bool> IGameDbService.Delete<T>(Guid id)
        {
            await using var transaction = await _dungeonDbContext.Database.BeginTransactionAsync();

            var dbSet = GetDbSet<T>();
            if (dbSet is null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            var toDelete = await dbSet.FindAsync(id);
            if (toDelete is null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            dbSet.Remove(toDelete);
            await transaction.CommitAsync();
            return true;
        }

        /// <inheritdoc cref="IGameDbService.Get{T}"/>
        async Task<T> IGameDbService.Get<T>(Guid id)
        {
            await using var transaction = await _dungeonDbContext.Database.BeginTransactionAsync();

            var dbSet = GetDbSet<T>();
            if (dbSet is null)
            {
                await transaction.RollbackAsync();
                return null;
            }

            var resultItem = await dbSet.FindAsync(id);
            await transaction.CommitAsync();
            return resultItem;
        }

        /// <inheritdoc cref="IGameDbService.GetAll{T}"/>
        async Task<ICollection<T>> IGameDbService.GetAll<T>()
        {
            await using var transaction = await _dungeonDbContext.Database.BeginTransactionAsync();

            var dbSet = GetDbSet<T>();
            if (dbSet is null)
            {
                await transaction.RollbackAsync();
                return new List<T>();
            }

            var resultList = await dbSet.ToListAsync();
            await transaction.CommitAsync();
            return resultList;
        }

        /// <inheritdoc cref="IGameDbService.NewOrUpdate{T}"/>
        async Task<bool> IGameDbService.NewOrUpdate<T>(T item)
        {
            if (item is null) return false;
            await using var transaction = await _dungeonDbContext.Database.BeginTransactionAsync();

            var dbSet = GetDbSet<T>();
            if (dbSet is null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            dbSet.Update(item);
            await transaction.CommitAsync();
            return true;
        }

        /// <inheritdoc cref="IGameDbService.DeleteAllFromUser"/>
        public async Task<bool> DeleteAllFromUser(Guid userId)
        {
            await using var transaction = await _dungeonDbContext.Database.BeginTransactionAsync();
            var dungeonDbSet = GetDbSet<Dungeon>();
            var avatarDbSet = GetDbSet<Avatar>();

            if (dungeonDbSet is null || avatarDbSet is null)
            {
                await transaction.RollbackAsync();
                return false;
            }

            var userDungeons = dungeonDbSet.AsQueryable().Where(x => x.DungeonOwner.Id == userId.ToString());
            var usersAvatars = avatarDbSet.AsQueryable().Where(x => x.Owner.Id == userId.ToString());

            dungeonDbSet.RemoveRange(userDungeons);
            avatarDbSet.RemoveRange(usersAvatars);

            await transaction.CommitAsync();
            return true;
        }

        /// <summary>
        /// Returns the right dbSet based on the generic E or null if no dbSet is suitable.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        internal DbSet<E> GetDbSet<E>() where E : class, IApprovable
        {
            if (_dungeonDbContext.Dungeons is DbSet<E> dungeons) return dungeons;
            if (_dungeonDbContext.Avatars is DbSet<E> avatars) return avatars;
            if (_dungeonDbContext.Races is DbSet<E> races) return races;
            if (_dungeonDbContext.Classes is DbSet<E> classes) return classes;
            if (_dungeonDbContext.Rooms is DbSet<E> rooms) return rooms;
            if (_dungeonDbContext.Consumables is DbSet<E> consumables) return consumables;
            if (_dungeonDbContext.Usables is DbSet<E> usables) return usables;
            if (_dungeonDbContext.Wearables is DbSet<E> wearables) return wearables;
            if (_dungeonDbContext.Npcs is DbSet<E> npcs) return npcs;
            if (_dungeonDbContext.SpecialActions is DbSet<E> specialActions) return specialActions;
            if (_dungeonDbContext.Takeables is DbSet<E> takeables) return takeables;
            if (_dungeonDbContext.Inspectables is DbSet<E> inspectables) return inspectables;
            return null;
        }
        #endregion

    }
}
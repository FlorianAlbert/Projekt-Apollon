using Apollon.Mud.Server.Domain.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.DbContext;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Shared.HubContract;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.Domain.Implementations.Shared
{
    /// <inheritdoc cref="IGameDbService"/>
    public class GameDbService: IGameDbService
    {
        /// <summary>
        /// The connection service to get the connection of an user.
        /// </summary>
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// The hub to send an user a message.
        /// </summary>
        private readonly IHubContext<GameHub, IClientGameHubContract> _hubContext;

        /// <summary>
        /// The dbContext to work with the db.
        /// </summary>
        private readonly DungeonDbContext _dungeonDbContext;

        public GameDbService(IConnectionService connectionService, IHubContext<GameHub, IClientGameHubContract> hubContext,
            DungeonDbContext dungeonDbContext)
        {
            _connectionService = connectionService;
            _hubContext = hubContext;
            _dungeonDbContext = dungeonDbContext;
        }

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

        async Task<ICollection<T>> IGameDbService.GetAll<T>()
        {
            await using var transaction = await _dungeonDbContext.Database.BeginTransactionAsync();

            var dbSet = GetDbSet<T>();
            if (dbSet is null)
            {
                await transaction.RollbackAsync();
                return new List<T>();//ToDo passt die leere Liste oder muss unterschieden werden, ob die List wirklich leer ist oder es sie nicht gibt?!
            }

            var resultList = await dbSet.ToListAsync();
            await transaction.CommitAsync();
            return resultList;
        }

        async Task<bool> IGameDbService.NewOrUpdate<T>(T item, Guid dungeonId)
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

            var dungeonMasterConnection = _connectionService.GetDungeonMasterConnectionByDungeonId(dungeonId);
            if (dungeonMasterConnection is not null) //A Dungeon Master is currently logged in.
            {
                _hubContext.Clients.Client(dungeonMasterConnection.ChatConnectionId).ReceiveGameMessage("The dungeon was updated.");
            }
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
        private DbSet<E> GetDbSet<E>() where E: class, IApprovable
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
    }
}
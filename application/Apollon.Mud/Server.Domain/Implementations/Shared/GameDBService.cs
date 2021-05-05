using Apollon.Mud.Server.Domain.Interfaces.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.DbContext;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Outbound.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.Domain.Implementations.Shared
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class GameDbService: IGameDbService
    {
        /// <summary>
        /// ToDo
        /// </summary>
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// ToDo
        /// </summary>
        private readonly IHubContext<GameHub> _hubContext;

        /// <summary>
        /// ToDo
        /// </summary>
        private readonly DungeonDbContext _dungeonDbContext;

        public GameDbService(IConnectionService connectionService, IHubContext<GameHub> hubContext,
            DungeonDbContext dungeonDbContext)
        {
            _connectionService = connectionService;
            _hubContext = hubContext;
            _dungeonDbContext = dungeonDbContext;
        }

        public async Task<bool> ValidateUserIsDungeonMaster(Guid dungeonId, Guid userId)
        {
            await using var transaction = await _dungeonDbContext.Database.BeginTransactionAsync();
            var dungeon = await _dungeonDbContext.Dungeons.FindAsync(dungeonId);
            return dungeon.DungeonMasters.Any(x => x.Id == userId.ToString());
        }

        async Task<bool> IGameDbService.Delete<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        async Task<T> IGameDbService.Get<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        async Task<ICollection<T>> IGameDbService.GetAll<T>()
        {
            throw new NotImplementedException();
        }

        async Task<bool> IGameDbService.NewOrUpdate<T>(T item)
        {
            //ToDO wenn Dungeon geupdated, benachrichte CurrentDM
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAllFromUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        private DbSet<E> GetDbSet<E>() where E: class, IApprovable
        {
            if (_dungeonDbContext.Dungeons is DbSet<E> dungeons) return dungeons;
            if (_dungeonDbContext.Avatars is DbSet<E> avatars) return avatars;
            if (_dungeonDbContext.Races is DbSet<E> races) return races;
            if (_dungeonDbContext.Classes is DbSet<E> classes) return classes;
            if (_dungeonDbContext.Rooms is DbSet<E> rooms) return rooms;
            if (_dungeonDbContext.Inspectables is DbSet<E> inspectables) return inspectables;
            if (_dungeonDbContext.Takeables is DbSet<E> takeables) return takeables;
            if (_dungeonDbContext.Consumables is DbSet<E> consumables) return consumables;
            if (_dungeonDbContext.Usables is DbSet<E> usables) return usables;
            if (_dungeonDbContext.Wearables is DbSet<E> wearables) return wearables;
            if (_dungeonDbContext.Npcs is DbSet<E> npcs) return npcs;
            if (_dungeonDbContext.SpecialActions is DbSet<E> specialActions) return specialActions;
            return null;
        }
    }
}
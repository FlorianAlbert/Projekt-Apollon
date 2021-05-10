using Apollon.Mud.Shared.Dungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IDungeonService
    {
        /// <summary>
        /// TODO
        /// </summary>
        public HttpClient HttpClient { get;}

        /// <summary>
        /// TODO
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonDto"></param>
        /// <returns></returns>
        Task<Guid> CreateNewDungeon(DungeonDto dungeonDto);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonDto"></param>
        /// <returns></returns>
        Task<DungeonDto> UpdateDungeon(DungeonDto dungeonDto);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<bool> DeleteDungeon(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        Task<ICollection<DungeonDto>> GetAllDungeons();

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        Task<ICollection<DungeonDto>> GetAllDungeonsForUser();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<DungeonDto> GetDungeon(Guid dungeonId);
    }
}

using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
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
    public interface ITakeableService
    {
        /// <summary>
        /// TODO
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="takeableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewTakeable(TakeableDto takeableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="takeableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<TakeableDto> UpdateTakeable(TakeableDto takeableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="takeableId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        /// 
        Task<bool> DeleteTakeable(Guid dungeonId, Guid takeableId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<TakeableDto>> GetAllTakeables(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="takeableId"></param>
        /// <returns></returns>
        Task<TakeableDto> GetTakeable(Guid dungeonId, Guid takeableId);
    }
}

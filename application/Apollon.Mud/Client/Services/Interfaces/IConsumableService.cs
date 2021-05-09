using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
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
    public interface IConsumableService
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
        /// <param name="consumableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewConsumable(ConsumableDto consumableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="consumableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ConsumableDto> UpdateConsumable(ConsumableDto consumableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="consumableId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        /// 
        Task<bool> DeleteConsumable(Guid dungeonId, Guid consumableId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<ConsumableDto>> GetAllConsumables(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="consumableId"></param>
        /// <returns></returns>
        Task<ConsumableDto> GetConsumable(Guid dungeonId, Guid consumableId);
    }
}

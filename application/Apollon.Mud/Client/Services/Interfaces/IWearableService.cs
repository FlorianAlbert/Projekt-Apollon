using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
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
    public interface IWearableService
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
        /// <param name="wearableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewWearable(WearableDto wearableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="wearableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<WearableDto> UpdateWearable(WearableDto wearableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="wearableId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<bool> DeleteWearable(Guid dungeonId, Guid wearableId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<WearableDto>> GetAllWearables(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="wearableId"></param>
        /// <returns></returns>
        Task<WearableDto> GetWearable(Guid dungeonId, Guid wearableId);
    }
}

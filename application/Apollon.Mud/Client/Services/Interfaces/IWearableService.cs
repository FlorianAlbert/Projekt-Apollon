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
    /// Interface to provide CRUD Functions for Dungeon Takeables
    /// </summary>
    public interface IWearableService
    {
        /// <summary>
        /// The Rest Http Client injected into the class
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Creates Cancellation Tokens for each Http Request
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="wearableDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the wearable</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        Task<Guid> CreateNewWearable(WearableDto wearableDto, Guid dungeonId);

        /// <summary>
        /// Updates the given wearable in the Database
        /// </summary>
        /// <param name="wearableDto">The wearable with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the wearable</param>
        /// <returns>The old wearable in case the Database transaction failed, otherwise null</returns>
        Task<WearableDto> UpdateWearable(WearableDto wearableDto, Guid dungeonId);

        /// <summary>
        /// Deletes the given wearable in the Database
        /// </summary>
        /// <param name="wearableId">The id of the wearable to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the wearable</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<bool> DeleteWearable(Guid dungeonId, Guid wearableId);

        /// <summary>
        /// Gets all wearables of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested wearables</param>
        /// <returns>A Collection of the requested wearables, otherwise null</returns>
        Task<ICollection<WearableDto>> GetAllWearables(Guid dungeonId);

        /// <summary>
        /// Gets one wearable of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested wearable</param>
        /// <param name="wearableId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        Task<WearableDto> GetWearable(Guid dungeonId, Guid wearableId);
    }
}

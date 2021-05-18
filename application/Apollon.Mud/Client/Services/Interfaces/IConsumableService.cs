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
    /// Interface to provide CRUD Functions for Dungeon Consumables
    /// </summary>
    public interface IConsumableService
    {
        /// <summary>
        /// The HttpClient injected into the Service
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// The Cancellation Token Source used by the Service
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Sends the given consumableDto to the consumableController with the associated Dungeon and persists it in the Database
        /// </summary>
        /// <param name="consumableDto">The consumable to persist</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the consumable</param>
        /// <returns>The Guid of the created consumable if successfull, otherwise an empty Guid</returns>
        Task<Guid> CreateNewConsumable(ConsumableDto consumableDto, Guid dungeonId);

        /// <summary>
        /// Updates a consumable in the database
        /// </summary>
        /// <param name="consumableDto">The consumable to be updated</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the Consumable</param>
        /// <returns>null if successfull, otherwise the old Consumable to write it back</returns>
        Task<ConsumableDto> UpdateConsumable(ConsumableDto consumableDto, Guid dungeonId);

        /// <summary>
        /// Calls the consumableController to delete the consumable of the Dungeon from the Database
        /// </summary>
        /// <param name="consumableId">The ID of the consumable to delete</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the consumable</param>
        /// <returns>True if successfull, otherwise false</returns>
        Task<bool> DeleteConsumable(Guid dungeonId, Guid consumableId);

        /// <summary>
        /// Receives all consumables associated with the Dungeon from the Database as consumableDtos
        /// </summary>
        /// <param name="dungeonId">The Dungeon thats consumables are wanted</param>
        /// <returns>A collection of Dungeon-consumables if successfull, otherwise null</returns>
        Task<ICollection<ConsumableDto>> GetAllConsumables(Guid dungeonId);

        /// <summary>
        /// Gets a single consumable from the Database
        /// </summary>
        /// <param name="dungeonId">The ID of the Dungeon associated with the Dungeon</param>
        /// <param name="consumableId">The ID of the wanted Dungeon</param>
        /// <returns>An consumableDto if succesfull, otherwise null</returns>
        Task<ConsumableDto> GetConsumable(Guid dungeonId, Guid consumableId);
    }
}

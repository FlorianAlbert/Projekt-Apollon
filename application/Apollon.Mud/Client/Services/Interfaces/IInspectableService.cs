using Apollon.Mud.Shared.Dungeon.Inspectable;
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
    public interface IInspectableService
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
        /// <param name="inspectableDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the inspectable</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        Task<Guid> CreateNewInspectable(InspectableDto inspectableDto, Guid dungeonId);

        /// <summary>
        /// Updates the given inspectable in the Database
        /// </summary>
        /// <param name="inspectableDto">The inspectable with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the inspectable</param>
        /// <returns>The old inspectable in case the Database transaction failed, otherwise null</returns>
        Task<InspectableDto> UpdateInspectable(InspectableDto inspectableDto, Guid dungeonId);

        /// <summary>
        /// Deletes the given inspectable in the Database
        /// </summary>
        /// <param name="inspectableId">The id of the inspectable to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the inspectable</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<bool> DeleteInspectable(Guid dungeonId, Guid inspectableId);

        /// <summary>
        /// Gets all inspectables of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested inspectables</param>
        /// <returns>A Collection of the requested inspectables, otherwise null</returns>
        Task<ICollection<InspectableDto>> GetAllInspectables(Guid dungeonId);

        /// <summary>
        /// Gets one inspectable of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested inspectable</param>
        /// <param name="inspectableId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        Task<InspectableDto> GetInspectable(Guid dungeonId, Guid inspectableId);
    }
}

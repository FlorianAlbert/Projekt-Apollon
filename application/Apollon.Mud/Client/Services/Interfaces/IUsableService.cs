using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
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
    public interface IUsableService
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
        /// <param name="usableDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the usable</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        Task<Guid> CreateNewUsable(UsableDto usableDto, Guid dungeonId);

        /// <summary>
        /// Updates the given usable in the Database
        /// </summary>
        /// <param name="usableDto">The usable with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the usable</param>
        /// <returns>The old usable in case the Database transaction failed, otherwise null</returns>
        Task<UsableDto> UpdateUsable(UsableDto usableDto, Guid dungeonId);

        /// <summary>
        /// Deletes the given usable in the Database
        /// </summary>
        /// <param name="usableId">The id of the usable to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the usable</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<bool> DeleteUsable(Guid dungeonId, Guid usableId);

        /// <summary>
        /// Gets all usables of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested usables</param>
        /// <returns>A Collection of the requested usables, otherwise null</returns>
        Task<ICollection<UsableDto>> GetAllUsables(Guid dungeonId);

        /// <summary>
        /// Gets one usable of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested usable</param>
        /// <param name="usableId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        Task<UsableDto> GetUsable(Guid dungeonId, Guid usableId);
    }
}

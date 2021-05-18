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
    /// Interface to provide CRUD Functions for Dungeon Takeables
    /// </summary>
    public interface ITakeableService
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
        /// Updates the given takeable in the Database
        /// </summary>
        /// <param name="takeableDto">The takeable with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the takeable</param>
        /// <returns>The old takeable in case the Database transaction failed, otherwise null</returns>
        Task<Guid> CreateNewTakeable(TakeableDto takeableDto, Guid dungeonId);

        /// <summary>
        /// Deletes the given takeable in the Database
        /// </summary>
        /// <param name="takeableId">The id of the takeable to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the takeable</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<TakeableDto> UpdateTakeable(TakeableDto takeableDto, Guid dungeonId);

        /// <summary>
        /// Gets all takeables of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested takeables</param>
        /// <returns>A Collection of the requested takeables, otherwise null</returns>
        Task<bool> DeleteTakeable(Guid dungeonId, Guid takeableId);

        /// <summary>
        /// Gets all takeables of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested takeables</param>
        /// <returns>A Collection of the requested takeables, otherwise null</returns>
        Task<ICollection<TakeableDto>> GetAllTakeables(Guid dungeonId);

        /// <summary>
        /// Gets one takeable of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested takeable</param>
        /// <param name="takeableId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        Task<TakeableDto> GetTakeable(Guid dungeonId, Guid takeableId);
    }
}

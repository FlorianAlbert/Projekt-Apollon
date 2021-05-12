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
    /// Interface to provide CRUD Functions for Dungeon Consumables
    /// </summary>
    public interface IDungeonService
    {
        /// <summary>
        /// The Rest Http Client injected into the class
        /// </summary>
        public HttpClient HttpClient { get;}

        /// <summary>
        /// Creates Cancellation Tokens for each Http Request
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="dungeonDto">The Dungeon to create</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        Task<(Guid, bool)> CreateNewDungeon(DungeonDto dungeonDto);

        /// <summary>
        /// Updates the given Dungeon in the Database
        /// </summary>
        /// <param name="dungeonDto">The dungeon with updated information</param>
        /// <returns>The old dungeon in case the Database transaction failed, otherwise null</returns>
        Task<DungeonDto> UpdateDungeon(DungeonDto dungeonDto);

        /// <summary>
        /// Deletes the Dungeon of the given ID from the database
        /// </summary>
        /// <param name="dungeonId">The Dungeon that contains the class</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<bool> DeleteDungeon(Guid dungeonId);

        /// <summary>
        /// Gets all Dungeons
        /// </summary>
        /// <returns>A Collection of DungeonDtos, otherwise null</returns>
        Task<ICollection<DungeonDto>> GetAllDungeons();

        /// <summary>
        /// Gets all Dungeons for a certain user
        /// </summary>
        /// <returns>A collection of the users dungeons if successfull, otherwise null</returns>
        Task<ICollection<DungeonDto>> GetAllDungeonsForUser();

        /// <summary>
        /// Gets the Dungeon of the ID
        /// </summary>
        /// <param name="dungeonId">The ID of the requested dungeon</param>
        /// <returns>The requested dungeon, otherwise null</returns>
        Task<DungeonDto> GetDungeon(Guid dungeonId);

        /// <summary>
        /// Opens a request of a user to enter a private Dungeon
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns>True if submitting the request was successfull, else false</returns>
        Task<bool> OpenEnterRequest(Guid dungeonId);

        /// <summary>
        /// Answers a users request to enter a private dungeon
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns>True if sending it to the Backend was successfull, else false</returns>
        Task<bool> SubmitEnterRequest(Guid dungeonId);
    }
}

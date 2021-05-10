using Apollon.Mud.Shared.Dungeon.Race;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// Interface to provide CRUD Functions for Dungeon Races
    /// </summary>
    public interface IRaceService
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
        /// <param name="raceDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the race</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        Task<Guid> CreateNewRace(RaceDto raceDto, Guid dungeonId);

        /// <summary>
        /// Updates the given race in the Database
        /// </summary>
        /// <param name="raceDto">The race with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the race</param>
        /// <returns>The old race in case the Database transaction failed, otherwise null</returns>
        Task<RaceDto> UpdateRace(RaceDto raceDto, Guid dungeonId);

        /// <summary>
        /// Deletes the given race in the Database
        /// </summary>
        /// <param name="raceId">The id of the race to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the race</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<bool> DeleteRace(Guid dungeonId, Guid raceId);

        /// <summary>
        /// Gets all races of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested races</param>
        /// <returns>A Collection of the requested races, otherwise null</returns>
        Task<ICollection<RaceDto>> GetAllRaces(Guid dungeonId);

        /// <summary>
        /// Gets one race of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested race</param>
        /// <param name="raceId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        Task<RaceDto> GetRace(Guid dungeonId, Guid raceId);

    }
}

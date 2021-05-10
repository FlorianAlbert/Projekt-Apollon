using Apollon.Mud.Shared.Dungeon.Requestable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// Interface to provide CRUD Functions for Dungeon Special Actions
    /// </summary>
    public interface ISpecialActionService
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
        /// <param name="requestableDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the special action</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        Task<Guid> CreateNewRequestable(RequestableDto requestableDto, Guid dungeonId);

        /// <summary>
        /// Updates the given special action in the Database
        /// </summary>
        /// <param name="requestableDto">The special action with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the special action</param>
        /// <returns>The old special action in case the Database transaction failed, otherwise null</returns>
        Task<RequestableDto> UpdateRequestable(RequestableDto requestableDto, Guid dungeonId);

        /// <summary>
        /// Deletes the given special action in the Database
        /// </summary>
        /// <param name="requestableId">The id of the special action to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the special action</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<bool> DeleteRequestable(Guid dungeonId, Guid actionId);

        /// <summary>
        /// Gets all special actions of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested special actions</param>
        /// <returns>A Collection of the requested special actions, otherwise null</returns>
        Task<ICollection<RequestableDto>> GetAllRequestables(Guid dungeonId);

        /// <summary>
        /// Gets one special action of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested special action</param>
        /// <param name="special actionId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        Task<RequestableDto> GetRequestable(Guid dungeonId, Guid actionId);
    }
}

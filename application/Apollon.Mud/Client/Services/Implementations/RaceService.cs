using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Race;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementations
{
    public class RaceService : IRaceService
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
        /// This service contains all logic for sending races to the backend and persist them
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public RaceService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="raceDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the race</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewRace(RaceDto raceDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/races/" + dungeonId, raceDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Updates the given race in the Database
        /// </summary>
        /// <param name="raceDto">The race with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the race</param>
        /// <returns>The old race in case the Database transaction failed, otherwise null</returns>
        public async Task<RaceDto> UpdateRace(RaceDto raceDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/races/" + dungeonId, raceDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<RaceDto>();

            return null;
        }

        /// <summary>
        /// Deletes the given race in the Database
        /// </summary>
        /// <param name="raceId">The id of the race to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the race</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        public async Task<bool> DeleteRace(Guid dungeonId, Guid raceId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/races/" + dungeonId + "/" + raceId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all races of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested races</param>
        /// <returns>A Collection of the requested races, otherwise null</returns>
        public async Task<ICollection<RaceDto>> GetAllRaces(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/races/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<RaceDto>>();

            return null;
        }

        /// <summary>
        /// Gets one race of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested race</param>
        /// <param name="raceId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        public async Task<RaceDto> GetRace(Guid dungeonId, Guid raceId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/races/" + dungeonId + "/" + raceId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<RaceDto>();

            return null;
        }

    }
}
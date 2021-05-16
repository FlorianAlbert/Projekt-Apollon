using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementations
{
    public class WearableService : IWearableService
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
        /// This service contains all logic for sending wearables to the backend and persist them
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public WearableService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="wearableDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the wearable</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewWearable(WearableDto wearableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/wearables/" + dungeonId, wearableDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Updates the given wearable in the Database
        /// </summary>
        /// <param name="wearableDto">The wearable with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the wearable</param>
        /// <returns>The old wearable in case the Database transaction failed, otherwise null</returns>
        public async Task<WearableDto> UpdateWearable(WearableDto wearableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/wearables/" + dungeonId, wearableDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<WearableDto>();

            return null;
        }

        /// <summary>
        /// Deletes the given wearable in the Database
        /// </summary>
        /// <param name="wearableId">The id of the wearable to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the wearable</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        public async Task<bool> DeleteWearable(Guid dungeonId, Guid wearableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/wearables/" + dungeonId + "/" + wearableId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all wearables of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested wearables</param>
        /// <returns>A Collection of the requested wearables, otherwise null</returns>
        public async Task<ICollection<WearableDto>> GetAllWearables(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/wearables/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<WearableDto>>();

            return null;
        }

        /// <summary>
        /// Gets one wearable of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested wearable</param>
        /// <param name="wearableId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        public async Task<WearableDto> GetWearable(Guid dungeonId, Guid wearableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/wearables/" + dungeonId + "/" + wearableId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<WearableDto>();

            return null;
        }

    }
}

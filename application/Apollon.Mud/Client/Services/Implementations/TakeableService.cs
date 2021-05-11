using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class TakeableService : ITakeableService
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
        /// This service contains all logic for sending takeables to the backend and persist them
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public TakeableService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="takeableDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the takeable</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewTakeable(TakeableDto takeableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/takeables/" + dungeonId, takeableDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Updates the given takeable in the Database
        /// </summary>
        /// <param name="takeableDto">The takeable with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the takeable</param>
        /// <returns>The old takeable in case the Database transaction failed, otherwise null</returns>
        public async Task<TakeableDto> UpdateTakeable(TakeableDto takeableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/takeables/" + dungeonId, takeableDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<TakeableDto>();

            return null;
        }

        /// <summary>
        /// Deletes the given takeable in the Database
        /// </summary>
        /// <param name="takeableId">The id of the takeable to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the takeable</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        public async Task<bool> DeleteTakeable(Guid dungeonId, Guid takeableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/takeables/" + dungeonId + "/" + takeableId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all takeables of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested takeables</param>
        /// <returns>A Collection of the requested takeables, otherwise null</returns>
        public async Task<ICollection<TakeableDto>> GetAllTakeables(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/takeables/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<TakeableDto>>();

            return null;
        }

        /// <summary>
        /// Gets one takeable of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested takeable</param>
        /// <param name="takeableId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        public async Task<TakeableDto> GetTakeable(Guid dungeonId, Guid takeableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/takeables/" + dungeonId + "/" + takeableId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<TakeableDto>();

            return null;
        }

    }
}

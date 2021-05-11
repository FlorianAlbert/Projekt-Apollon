using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class UsableService : IUsableService
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
        /// This service contains all logic for sending usables to the backend and persist them
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public UsableService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="usableDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the usable</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewUsable(UsableDto usableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/usables/" + dungeonId, usableDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Updates the given usable in the Database
        /// </summary>
        /// <param name="usableDto">The usable with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the usable</param>
        /// <returns>The old usable in case the Database transaction failed, otherwise null</returns>
        public async Task<UsableDto> UpdateUsable(UsableDto usableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/usables/" + dungeonId, usableDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<UsableDto>();

            return null;
        }

        /// <summary>
        /// Deletes the given usable in the Database
        /// </summary>
        /// <param name="usableId">The id of the usable to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the usable</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        public async Task<bool> DeleteUsable(Guid dungeonId, Guid usableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/usables/" + dungeonId + "/" + usableId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all usables of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested usables</param>
        /// <returns>A Collection of the requested usables, otherwise null</returns>
        public async Task<ICollection<UsableDto>> GetAllUsables(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/usables/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<UsableDto>>();

            return null;
        }

        /// <summary>
        /// Gets one usable of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested usable</param>
        /// <param name="usableId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        public async Task<UsableDto> GetUsable(Guid dungeonId, Guid usableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/usables/" + dungeonId + "/" + usableId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<UsableDto>();

            return null;
        }

    }
}

using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Requestable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class SpecialActionService : ISpecialActionService
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
        /// This service contains all logic for sending special actions to the backend and persist them
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public SpecialActionService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="requestableDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the special action</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewRequestable(RequestableDto requestableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/specialActions/" + dungeonId, requestableDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Updates the given special action in the Database
        /// </summary>
        /// <param name="requestableDto">The special action with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the special action</param>
        /// <returns>The old special action in case the Database transaction failed, otherwise null</returns>
        public async Task<RequestableDto> UpdateRequestable(RequestableDto requestableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/specialActions/" + dungeonId, requestableDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<RequestableDto>(); ;

            return null;
        }

        /// <summary>
        /// Deletes the given special action in the Database
        /// </summary>
        /// <param name="requestableId">The id of the special action to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the special action</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        public async Task<bool> DeleteRequestable(Guid dungeonId, Guid requestableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/specialActions/" + dungeonId + "/" + requestableId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all special actions of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested special actions</param>
        /// <returns>A Collection of the requested special actions, otherwise null</returns>
        public async Task<ICollection<RequestableDto>> GetAllRequestables(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/specialActions/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<RequestableDto>>();

            return null;
        }

        /// <summary>
        /// Gets one special action of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested special action</param>
        /// <param name="special actionId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        public async Task<RequestableDto> GetRequestable(Guid dungeonId, Guid requestableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/specialActions/" + dungeonId + "/" + requestableId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<RequestableDto>();

            return null;
        }
    }
}

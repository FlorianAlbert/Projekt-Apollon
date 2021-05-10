using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Inspectable;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class InspectableService : IInspectableService
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
        /// This service contains all logic for sending inspectableS to the backend and persist them
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public InspectableService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="inspectableDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the inspectable</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewInspectable(InspectableDto inspectableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/inspectables/" + dungeonId, inspectableDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Updates the given inspectable in the Database
        /// </summary>
        /// <param name="inspectableDto">The inspectable with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the inspectable</param>
        /// <returns>The old inspectable in case the Database transaction failed, otherwise null</returns>
        public async Task<InspectableDto> UpdateInspectable(InspectableDto inspectableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/inspectables/" + dungeonId, inspectableDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<InspectableDto>();

            return null;
        }

        /// <summary>
        /// Deletes the given inspectable in the Database
        /// </summary>
        /// <param name="inspectableId">The id of the inspectable to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the inspectable</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        public async Task<bool> DeleteInspectable(Guid dungeonId, Guid inspectableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/inspectables/" + dungeonId + "/" + inspectableId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all inspectables of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested inspectables</param>
        /// <returns>A Collection of the requested inspectables, otherwise null</returns>
        public async Task<ICollection<InspectableDto>> GetAllInspectables(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/inspectables/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<InspectableDto>>();

            return null;
        }

        /// <summary>
        /// Gets one inspectable of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested inspectable</param>
        /// <param name="inspectableId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        public async Task<InspectableDto> GetInspectable(Guid dungeonId, Guid inspectableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/inspectables/" + dungeonId + "/" + inspectableId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<InspectableDto>();

            return null;
        }

    }
}

using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementations
{
    public class ConsumableService : IConsumableService
    {
        /// <summary>
        /// The HttpClient injected into the Service
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// The Cancellation Token Source used by the Service
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// This class contains all logic for handling Consumables between Front- and Backend
        /// </summary>
        /// <param name="httpClientFactory">The HttpClientFactory injected into the service</param>
        /// <param name="userContext">The scoped UserContext of the current connection</param>
        public ConsumableService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given consumableDto to the consumableController with the associated Dungeon and persists it in the Database
        /// </summary>
        /// <param name="consumableDto">The consumable to persist</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the consumable</param>
        /// <returns>The Guid of the created consumable if successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewConsumable(ConsumableDto consumableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/consumables/" + dungeonId, consumableDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Updates a consumable in the database
        /// </summary>
        /// <param name="consumableDto">The consumable to be updated</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the Consumable</param>
        /// <returns>null if successfull, otherwise the old Consumable to write it back</returns>
        public async Task<ConsumableDto> UpdateConsumable(ConsumableDto consumableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/consumables/" + dungeonId, consumableDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) await response.Content.ReadFromJsonAsync<ConsumableDto>();

            return null;
        }

        /// <summary>
        /// Calls the consumableController to delete the consumable of the Dungeon from the Database
        /// </summary>
        /// <param name="consumableId">The ID of the consumable to delete</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the consumable</param>
        /// <returns>True if successfull, otherwise false</returns>
        public async Task<bool> DeleteConsumable(Guid dungeonId, Guid consumableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/consumables/" + dungeonId + "/" + consumableId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Receives all consumables associated with the Dungeon from the Database as consumableDtos
        /// </summary>
        /// <param name="dungeonId">The Dungeon thats consumables are wanted</param>
        /// <returns>A collection of Dungeon-consumables if successfull, otherwise null</returns>
        public async Task<ICollection<ConsumableDto>> GetAllConsumables(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/consumables/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<ConsumableDto>>();

            return null;
        }

        /// <summary>
        /// Gets a single consumable from the Database
        /// </summary>
        /// <param name="dungeonId">The ID of the Dungeon associated with the Dungeon</param>
        /// <param name="consumableId">The ID of the wanted Dungeon</param>
        /// <returns>An consumableDto if succesfull, otherwise null</returns>
        public async Task<ConsumableDto> GetConsumable(Guid dungeonId, Guid consumableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/consumables/" + dungeonId + "/" + consumableId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ConsumableDto>();

            return null;
        }

    }
}

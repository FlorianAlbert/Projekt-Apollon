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

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class ConsumableService : IConsumableService
    {
        /// <summary>
        /// TODO
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userContext"></param>
        public ConsumableService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="consumableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
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
        /// TODO
        /// </summary>
        /// <param name="consumableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<ConsumableDto> UpdateConsumable(ConsumableDto consumableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/consumables/" + dungeonId, consumableDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ConsumableDto>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="consumableId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteConsumable(Guid dungeonId, Guid consumableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/consumables/" + dungeonId + "/" + consumableId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="consumableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<ICollection<ConsumableDto>> GetAllConsumables(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/consumables/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<ConsumableDto>>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="consumableId"></param>
        /// <returns></returns>
        public async Task<ConsumableDto> GetConsumable(Guid dungeonId, Guid consumableId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/consumables/" + dungeonId + "/" + consumableId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ConsumableDto>();

            return null;
        }

    }
}

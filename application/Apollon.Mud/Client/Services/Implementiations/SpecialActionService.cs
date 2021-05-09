using Apollon.Mud.Client.Data;
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
        /// <param name="httpClient"></param>
        public SpecialActionService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="requestableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
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
        /// TODO
        /// </summary>
        /// <param name="requestableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<RequestableDto> UpdateRequestable(RequestableDto requestableDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/specialActions/" + dungeonId, requestableDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return null;

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRequestable(Guid actionId, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/specialActions/" + dungeonId + "/" + actionId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<ICollection<RequestableDto>> GetAllRequestables(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/specialActions/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<RequestableDto>>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public async Task<RequestableDto> GetRequestable(Guid dungeonId, Guid actionId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/specialActions/" + dungeonId + "/" + actionId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<RequestableDto>();

            return null;
        }
    }
}

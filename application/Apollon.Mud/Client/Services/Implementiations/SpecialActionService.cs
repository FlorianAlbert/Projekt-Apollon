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
        public SpecialActionService(HttpClient httpClient)
        {
            HttpClient = httpClient;
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

            var response = await HttpClient.PostAsJsonAsync("api/specialActions/{dungeonId}", requestableDto, cancellationToken);
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

            var response = await HttpClient.PutAsJsonAsync("api/specialActions/{dungeonId}", requestableDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return null;

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public Task<bool> DeleteRequestable(Guid dungeonId, Guid actionId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="requestableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public Task<ICollection<RequestableDto>> GetAllRequestables(Guid dungeonId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public Task<RequestableDto> GetRequestable(Guid dungeonId, Guid actionId)
        {
            throw new NotImplementedException();
        }
    }
}

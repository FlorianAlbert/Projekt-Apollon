using Apollon.Mud.Client.Data;
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

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class RaceService : IRaceService
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
        public RaceService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="raceDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
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
        /// TODO
        /// </summary>
        /// <param name="raceDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<RaceDto> UpdateRace(RaceDto raceDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/races", raceDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<RaceDto>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="raceId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRace(Guid dungeonId, Guid raceId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/races/" + dungeonId + "/" + raceId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<ICollection<RaceDto>> GetAllRaces(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/races" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<RaceDto>>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="raceId"></param>
        /// <returns></returns>
        public async Task<RaceDto> GetRace(Guid dungeonId, Guid raceId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/races/" + dungeonId + "/" + raceId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<RaceDto>();

            return null;
        }

    }
}
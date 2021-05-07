using Apollon.Mud.Client.Data;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementiations
{
    /// <summary>
    /// TODO
    /// </summary>
    public class DungeonService : IDungeonService
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
        /// <param name=""></param>
        public DungeonService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonDto"></param>
        /// <returns></returns>
        public async Task<Guid> CreateNewDungeon(DungeonDto dungeonDto)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/dungeons", dungeonDto, cancellationToken);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = response.Content.ReadFromJsonAsync<Guid>();
                responseGuid.Wait();
                return responseGuid.Result;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteDungeon(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/dungeons/" + dungeonId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<DungeonDto>> GetAllDungeons()
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/dungeons", cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<DungeonDto>>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<DungeonDto> GetDungeon(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/dungeons/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<DungeonDto>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonDto"></param>
        /// <returns></returns>
        public async Task<DungeonDto> UpdateDungeon(DungeonDto dungeonDto)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/dungeons", dungeonDto, cancellationToken);

            if(response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<DungeonDto>();

            return null;
        }
    }
}

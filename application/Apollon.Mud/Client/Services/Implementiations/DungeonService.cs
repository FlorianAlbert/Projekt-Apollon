using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon;
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
        public DungeonService(HttpClient httpClient)
        {
            HttpClient = httpClient;
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
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public Task<bool> DeleteDungeon(Guid dungeonId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<DungeonDto>> GetAllDungeons()
        {
            throw new NotImplementedException();
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

using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Npc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class NpcService : INpcService
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
        public NpcService(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="npcDto, dungeonId"></param>
        /// <returns></returns>
        public async Task<Guid> CreateNewNpc(NpcDto npcDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/npcs", npcDto, cancellationToken);
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
        /// <param name="npcDto, dungeonId"></param>
        /// <returns></returns>
        public async Task<NpcDto> UpdateNpc(NpcDto npcDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/npcs", npcDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return null;

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="npcDto, dungeonId"></param>
        /// <returns></returns>
        public Task<bool> DeleteNpc(Guid npcId, Guid dungeonId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="npcDto, dungeonId"></param>
        /// <returns></returns>
        public Task<ICollection<NpcDto>> GetAllNpcs(Guid dungeonId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId, actionId"></param>
        /// <returns></returns>
        public Task<NpcDto> GetNpc(Guid dungeonId, Guid actionId)
        {
            throw new NotImplementedException();
        }

    }
}

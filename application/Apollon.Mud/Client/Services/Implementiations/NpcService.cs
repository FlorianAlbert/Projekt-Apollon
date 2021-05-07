﻿using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Npc;
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
        public NpcService(HttpClient httpClient)
        {
            HttpClient = httpClient;
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="npcDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<Guid> CreateNewNpc(NpcDto npcDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/npcs/{dungeonId}", npcDto, cancellationToken);
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
        /// <param name="npcDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<NpcDto> UpdateNpc(NpcDto npcDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/npcs/{dungeonId}", npcDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<NpcDto>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="npcDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public Task<bool> DeleteNpc(NpcDto npcdto, Guid dungeonId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="npcDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public Task<ICollection<NpcDto>> GetAllNpcs(Guid dungeonId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public Task<NpcDto> GetNpc(Guid dungeonId, Guid actionId)
        {
            throw new NotImplementedException();
        }

    }
}

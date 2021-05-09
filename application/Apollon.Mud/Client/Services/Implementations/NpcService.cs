using Apollon.Mud.Client.Data;
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
        /// The Rest Http Client injected into the class
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Creates Cancellation Tokens for each Http Request
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// This service contains all logic for sending NPCS to the backend and persist them
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public NpcService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="npcDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the npc</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewNpc(NpcDto npcDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/npcs/" + dungeonId, npcDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Updates the given npc in the Database
        /// </summary>
        /// <param name="npcDto">The Npc with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the Npc</param>
        /// <returns>The old Npc in case the Database transaction failed, otherwise null</returns>
        public async Task<NpcDto> UpdateNpc(NpcDto npcDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/npcs/" + dungeonId, npcDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<NpcDto>();

            return null;
        }

        /// <summary>
        /// Deletes the given Npc in the Database
        /// </summary>
        /// <param name="npcId">The id of the Npc to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the npc</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        public async Task<bool> DeleteNpc(Guid npcId, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/npcs/" + dungeonId + "/" + npcId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all Npcs of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested npcs</param>
        /// <returns>A Collection of the requested Npcs, otherwise null</returns>
        public async Task<ICollection<NpcDto>> GetAllNpcs(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/npcs/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<NpcDto>>();

            return null;
        }

        /// <summary>
        /// Gets one Npc of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested Npc</param>
        /// <param name="npcId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        public async Task<NpcDto> GetNpc(Guid dungeonId, Guid npcId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/npcs/" + dungeonId + "/" + npcId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<NpcDto>();

            return null;
        }

    }
}

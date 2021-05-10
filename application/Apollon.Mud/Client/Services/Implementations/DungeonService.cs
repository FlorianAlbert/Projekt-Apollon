using Apollon.Mud.Client.Data.Account;
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
    
    public class DungeonService : IDungeonService
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
        /// This service contains all logic for sending Dungeons to the backend and persist them
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public DungeonService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="dungeonDto">The Dungeon to create</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
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
        /// Deletes the Dungeon of the given ID from the database
        /// </summary>
        /// <param name="dungeonId">The Dungeon that contains the class</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        public async Task<bool> DeleteDungeon(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/dungeons/" + dungeonId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all Dungeons
        /// </summary>
        /// <returns>A Collection of DungeonDtos, otherwise null</returns>
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
        /// <returns></returns>
        public async Task<ICollection<DungeonDto>> GetAllDungeonsForUser()
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/dungeons/userdungeons", cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<DungeonDto>>();

            return null;
        }

        /// <summary>
        /// Gets the Dungeon of the ID
        /// </summary>
        /// <param name="dungeonId">The ID of the requested dungeon</param>
        /// <returns>The requested dungeon, otherwise null</returns>
        public async Task<DungeonDto> GetDungeon(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/dungeons/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<DungeonDto>();

            return null;
        }

        /// <summary>
        /// Updates the given Dungeon in the Database
        /// </summary>
        /// <param name="dungeonDto">The dungeon with updated information</param>
        /// <returns>The old dungeon in case the Database transaction failed, otherwise null</returns>
        public async Task<DungeonDto> UpdateDungeon(DungeonDto dungeonDto)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/dungeons", dungeonDto, cancellationToken);

            if(response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<DungeonDto>();

            return null;
        }
    }
}

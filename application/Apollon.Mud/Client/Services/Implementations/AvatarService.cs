using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Avatar;
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
    public class AvatarService : IAvatarService
    {
        /// <summary>
        /// The HttpClient injected into the Service
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// The Cancellation Token Source used by the Service
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// This class contains all logic for handling Avatars between Front- and Backend
        /// </summary>
        /// <param name="httpClientFactory">The HttpClientFactory injected into the service</param>
        /// <param name="userContext">The scoped UserContext of the current connection</param>
        public AvatarService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given AvatarDto to the AvatarController with the associated Dungeon and persists it in the Database
        /// </summary>
        /// <param name="avatarDto">The Avatar to persist</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the Avatar</param>
        /// <returns>The Guid of the created Avatar if successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewAvatar(AvatarDto avatarDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/avatars/" + dungeonId, avatarDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="avatarDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        //public async Task<AvatarDto> UpdateAvatar(AvatarDto avatarDto, Guid dungeonId)
        //{
        //    CancellationToken cancellationToken = CancellationTokenSource.Token;

        //    var response = await HttpClient.PutAsJsonAsync("api/avatars/" + dungeonId, avatarDto, cancellationToken);

        //    if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<AvatarDto>();

        //    return null;
        //}

        /// <summary>
        /// Calls the AvatarController to delete the Avatar of the Dungeon from the Database
        /// </summary>
        /// <param name="avatarId">The ID of the Avatar to delete</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the avatar</param>
        /// <returns>True if successfull, otherwise false</returns>
        public async Task<bool> DeleteAvatar(Guid dungeonId, Guid avatarId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/avatars/" + dungeonId + "/" + avatarId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Receives all Avatars associated with the Dungeon from the Database as AvatarDtos
        /// </summary>
        /// <param name="dungeonId">The Dungeon thats avatars are wanted</param>
        /// <returns>A collection of Dungeon-Avatars if successfull, otherwise null</returns>
        public async Task<ICollection<AvatarDto>> GetAllAvatars(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/avatars/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<AvatarDto>>();

            return null;
        }

        /// <summary>
        /// Gets all Avatars for a single User of the associated Dungeon
        /// </summary>
        /// <param name="dungeonId">THe Dungeon thats avatars are wanted</param>
        /// <returns>A collection fot Dungeon-Avatars if successfull, otherwise null</returns>
        public async Task<ICollection<AvatarDto>> GetAllAvatarsForUser(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/avatars/" + dungeonId + "/user", cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<AvatarDto>>();

            return null;
        }

        /// <summary>
        /// Gets a single Avatar from the Database
        /// </summary>
        /// <param name="dungeonId">The ID of the Dungeon associated with the Dungeon</param>
        /// <param name="avatarId">The ID of the wanted Dungeon</param>
        /// <returns>An AvatarDto if succesfull, otherwise null</returns>
        public async Task<AvatarDto> GetAvatar(Guid dungeonId, Guid avatarId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/avatars/" + dungeonId + "/" + avatarId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<AvatarDto>();

            return null;
        }

    }
}

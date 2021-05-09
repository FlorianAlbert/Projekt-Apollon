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
        public AvatarService(HttpClient httpClient)
        {
            HttpClient = httpClient;
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="avatarDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
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
        /// TODO
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
        /// TODO
        /// </summary>
        /// <param name="avatarId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAvatar(Guid dungeonId, Guid avatarId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/avatars/" + dungeonId + "/" + avatarId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<ICollection<AvatarDto>> GetAllAvatars(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/avatars/" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<AvatarDto>>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<ICollection<AvatarDto>> GetAllAvatarsForUser(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/avatars/" + dungeonId + "/user", cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<AvatarDto>>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        public async Task<AvatarDto> GetAvatar(Guid dungeonId, Guid avatarId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/avatars/" + dungeonId + "/" + avatarId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<AvatarDto>();

            return null;
        }

    }
}

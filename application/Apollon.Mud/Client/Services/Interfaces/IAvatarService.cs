using Apollon.Mud.Shared.Dungeon.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    public interface IAvatarService
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
        /// <param name="avatarDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewAvatar(AvatarDto avatarDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="avatarDto"></param>
        /// <returns></returns>
        //Task<AvatarDto> UpdateAvatar(AvatarDto avatarDto);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        Task<bool> DeleteAvatar(Guid avatarId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<AvatarDto>> GetAllAvatars(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<AvatarDto>> GetAllAvatarsForUser(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        Task<AvatarDto> GetAvatar(Guid dungeonId, Guid avatarId);
    }
}

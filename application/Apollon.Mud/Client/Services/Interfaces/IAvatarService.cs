using Apollon.Mud.Shared.Dungeon.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// Interface to provide CRUD Functions for Dungeon Consumables
    /// </summary>
    public interface IAvatarService
    {
        /// <summary>
        /// The HttpClient used by the service
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// The CancellationTokenSource used by the service
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Sends the given AvatarDto to the AvatarController with the associated Dungeon and persists it in the Database
        /// </summary>
        /// <param name="avatarDto">The Avatar to persist</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the Avatar</param>
        /// <returns>The Guid of the created Avatar if successfull, otherwise an empty Guid</returns>
        Task<(Guid, bool)> CreateNewAvatar(AvatarDto avatarDto, Guid dungeonId);

        /// <summary>
        /// Calls a backend Controller to delete the Avatar of the Dungeon from the Database
        /// </summary>
        /// <param name="avatarId">The ID of the Avatar to delete</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the avatar</param>
        /// <returns>True if successfull, otherwise false</returns>
        Task<bool> DeleteAvatar(Guid dungeonId, Guid avatarId);

        /// <summary>
        /// Receives all Avatars associated with the Dungeon from the Database as AvatarDtos
        /// </summary>
        /// <param name="dungeonId">The Dungeon thats avatars are wanted</param>
        /// <returns>A collection of Dungeon-Avatars if successfull, otherwise null</returns>
        Task<ICollection<AvatarDto>> GetAllAvatars(Guid dungeonId);

        /// <summary>
        /// Gets all Avatars for a single User of the associated Dungeon
        /// </summary>
        /// <param name="dungeonId">THe Dungeon thats avatars are wanted</param>
        /// <returns>A collection fot Dungeon-Avatars if successfull, otherwise null</returns>
        Task<ICollection<AvatarDto>> GetAllAvatarsForUser(Guid dungeonId);

        /// <summary>
        /// Gets a single Avatar from the Database
        /// </summary>
        /// <param name="dungeonId">The ID of the Dungeon associated with the Dungeon</param>
        /// <param name="avatarId">The ID of the wanted Dungeon</param>
        /// <returns>An AvatarDto if succesfull, otherwise null</returns>
        Task<AvatarDto> GetAvatar(Guid dungeonId, Guid avatarId);
    }
}

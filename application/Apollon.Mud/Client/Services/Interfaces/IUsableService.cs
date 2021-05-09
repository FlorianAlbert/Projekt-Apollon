using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IUsableService
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
        /// <param name="usableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewUsable(UsableDto usableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="usableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<UsableDto> UpdateUsable(UsableDto usableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="usableId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<bool> DeleteUsable(Guid dungeonId, Guid usableId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<UsableDto>> GetAllUsables(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="usableId"></param>
        /// <returns></returns>
        Task<UsableDto> GetUsable(Guid dungeonId, Guid usableId);
    }
}

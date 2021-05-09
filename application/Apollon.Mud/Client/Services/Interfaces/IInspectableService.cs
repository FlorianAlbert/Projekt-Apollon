using Apollon.Mud.Shared.Dungeon.Inspectable;
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
    public interface IInspectableService
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
        /// <param name="inspectableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewInspectable(InspectableDto inspectableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="inspectableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<InspectableDto> UpdateInspectable(InspectableDto inspectableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="inspectableId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        /// 
        Task<bool> DeleteInspectable(Guid dungeonId, Guid inspectableId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<InspectableDto>> GetAllInspectables(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="inspectableId"></param>
        /// <returns></returns>
        Task<InspectableDto> GetInspectable(Guid dungeonId, Guid inspectableId);
    }
}

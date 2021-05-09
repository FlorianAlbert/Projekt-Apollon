using Apollon.Mud.Shared.Dungeon.Requestable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace Apollon.Mud.Client.Services.Interfaces
{
    public interface ISpecialActionService
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
        /// <param name="requestableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewRequestable(RequestableDto requestableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="requestableDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<RequestableDto> UpdateRequestable(RequestableDto requestableDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<bool> DeleteRequestable(Guid actionId, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<RequestableDto>> GetAllRequestables(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        Task<RequestableDto> GetRequestable(Guid dungeonId, Guid actionId);
    }
}

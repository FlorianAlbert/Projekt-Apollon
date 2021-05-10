using Apollon.Mud.Shared.Dungeon.Room;
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
    public interface IRoomService
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
        /// <param name="roomDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewRoom(RoomDto roomDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="roomDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<bool> UpdateRoom(RoomDto roomDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<bool> DeleteRoom(Guid roomId, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<RoomDto>> GetAllRooms(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<RoomDto> GetRoom(Guid roomId, Guid dungeonId);


    }
}
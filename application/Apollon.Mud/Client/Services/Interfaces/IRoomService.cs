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
    /// Interface to provide CRUD Functions for Dungeon Rooms
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// The Rest Http Client injected into the room
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Creates Cancellation Tokens for each Http Request
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Sends the given room to the backend and saves its connection to the given Dungeon in the Database
        /// </summary>
        /// <param name="roomDto">The Room to create</param>
        /// <param name="dungeonId">The dungeon that contains the room</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        Task<Guid> CreateNewRoom(RoomDto roomDto, Guid dungeonId);

        /// <summary>
        /// Updates the given room in the Database
        /// </summary>
        /// <param name="roomDto">The room with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the room</param>
        /// <returns>The old Room in case the Database transaction failed, otherwise null</returns>
        Task<bool> UpdateRoom(RoomDto roomDto, Guid dungeonId);

        /// <summary>
        /// Deletes the given room in the Database
        /// </summary>
        /// <param name="roomId">The room to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the room</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<bool> DeleteRoom(Guid roomId, Guid dungeonId);

        /// <summary>
        /// Gets all rooms of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested rooms</param>
        /// <returns>A Collection of the requested rooms, otherwise null</returns>
        Task<ICollection<RoomDto>> GetAllRooms(Guid dungeonId);

        /// <summary>
        /// Gets one room from a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested room</param>
        /// <param name="roomId">The ID of the requested room</param>
        /// <returns>The requested room, otherwise null</returns>
        Task<RoomDto> GetRoom(Guid dungeonId, Guid roomId);


    }
}
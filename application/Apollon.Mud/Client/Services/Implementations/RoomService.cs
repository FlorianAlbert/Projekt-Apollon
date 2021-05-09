using Apollon.Mud.Client.Data;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Room;
using Apollon.Mud.Shared.Dungeon.Npc;
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
    public class RoomService : IRoomService
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
        /// This service contains all logic for sending Dungeon Roomes to the back
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public RoomService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends the given class to the backend and saves its connection to the given Dungeon in the Database
        /// </summary>
        /// <param name="roomDto">The Room to create</param>
        /// <param name="dungeonId">The dungeon that contains the class</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        public async Task<Guid> CreateNewRoom(RoomDto roomDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/rooms/" + dungeonId, roomDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Updates the given class in the Database
        /// </summary>
        /// <param name="roomDto">The class with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the class</param>
        /// <returns>The old Room in case the Database transaction failed, otherwise null</returns>
        public async Task<RoomDto> UpdateRoom(RoomDto roomDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/rooms", roomDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<RoomDto>();

            return null;
        }

        /// <summary>
        /// Deletes the given class in the Database
        /// </summary>
        /// <param name="classId">The class to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the class</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        public async Task<bool> DeleteRoom(Guid classId, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/rooms/" + dungeonId + "/" + classId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets all classes of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested classes</param>
        /// <returns>A Collection of the requested Roomes, otherwise null</returns>
        public async Task<ICollection<RoomDto>> GetAllRooms(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/rooms" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<RoomDto>>();

            return null;
        }

        /// <summary>
        /// Gets one class from a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested class</param>
        /// <param name="classId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        public async Task<RoomDto> GetRoom(Guid dungeonId, Guid roomId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/rooms/" + dungeonId + "/" + roomId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<RoomDto>();

            return null;
        }
    }
}

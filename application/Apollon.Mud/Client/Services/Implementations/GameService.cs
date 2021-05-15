using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Avatar;
using Apollon.Mud.Shared.Game;
using Apollon.Mud.Shared.Game.DungeonMaster;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementations
{
    public class GameService : IGameService
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
        /// This service contains all logic for handling game logic
        /// </summary>
        /// <param name="httpClientFactory">The HttpClient injected into this class</param>
        /// <param name="userContext">The usercontext needed for authorization</param>
        public GameService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Sends a user command to the backend
        /// </summary>
        /// <param name="message">The command of the user</param>
        /// <returns>The HTTP Status Code the server returned</returns>
        public async Task<HttpStatusCode> SendCommand(string message)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/game/command", message, cancellationToken);

            return response.StatusCode;
        }

        /// <summary>
        /// Sends a request to enter the Dungeon to the Backend
        /// </summary>
        /// <param name="AsMaster">Wether the user joins as a DM</param>
        /// <param name="dungeonId">The Dungeon to join</param>
        /// <param name="avatarId">The avatar to join with, null if Dungeon Master</param>
        /// <param name="chatId">The ID of the ChatHub connection</param>
        /// <param name="gameId">The ID of the GameHub connection</param>
        /// <returns>The Statuscode returned by the server</returns>
        public async Task<HttpStatusCode> EnterDungeon(bool AsMaster, Guid dungeonId, Guid? avatarId, string chatId, string gameId)
        {
            EnterDungeonDto enterDto = new()
            {
                AsDungeonMaster = AsMaster,
                DungeonId = dungeonId,
                AvatarId = avatarId,
                ChatConnectionId = chatId,
                GameConnectionId = gameId
            };

            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/game/enter", enterDto, cancellationToken);

            return response.StatusCode;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> KickAvatar(Guid avatarId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsync("api/game/kick/" + avatarId, null, cancellationToken);

            return response.StatusCode;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> KickAll(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsync("api/game/kickall/" + dungeonId, null, cancellationToken);

            return response.StatusCode;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="message"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> AnswerDungeonMasterRequest(string message, AvatarDto avatar)
        {
            DungeonMasterRequestResponseDto answerDto = new DungeonMasterRequestResponseDto()
            {
                Avatar = avatar,
                Message = message
            };

            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/game/executeDungeonMasterRequestResponse", answerDto, cancellationToken);

            return response.StatusCode;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public async Task LeaveGame()
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            HttpClient.PostAsJsonAsync<object>("api/game/leave", null, cancellationToken);
        }
    }
}
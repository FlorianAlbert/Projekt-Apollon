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
        /// TODO
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> SendCommand(string message)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/game/command", message, cancellationToken);

            return response.StatusCode;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="AsMaster"></param>
        /// <param name="dungeonId"></param>
        /// <param name="avatarId"></param>
        /// <param name="chatId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> EnterDungeon(bool AsMaster, Guid dungeonId, Guid? avatarId, string chatId, string gameId)
        {
            EnterDungeonDto enterDto = new EnterDungeonDto()
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
    }
}
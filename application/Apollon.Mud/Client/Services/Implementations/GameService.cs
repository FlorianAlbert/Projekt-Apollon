using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Avatar;
using Apollon.Mud.Shared.Game;
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

        public Task<HttpStatusCode> SendCommand(string message)
        {
            throw new NotImplementedException();
        }

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

        public Task<HttpStatusCode> KickAvatar(Guid avatarId)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> KickAll(Guid dungeonId)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> AnswerDungeonMasterRequest(string message, AvatarDto avatar)
        {
            throw new NotImplementedException();
        }
    }
}
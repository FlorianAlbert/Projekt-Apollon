using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Game.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementations
{
    public class ChatService : IChatService
    {
        /// <summary>
        /// The HttpClient injected into the Service
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// The Cancellation Token Source used by the Service
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }
        
        public ChatService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Posts a message for all avatars inside a room
        /// </summary>
        /// <param name="chatMessageDto"></param>
        /// <returns></returns>
        public async Task<bool> PostRoomMessage(ChatMessageDto chatMessageDto)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/chat/roomMessage", chatMessageDto, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Posts a message to a specific avatar
        /// </summary>
        /// <param name="chatMessageDto"></param>
        /// <returns></returns>
        public async Task<bool> PostWhisperMessage(ChatMessageDto chatMessageDto)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/chat/whisper", chatMessageDto, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Posts a message to all avatars in a dungeon
        /// </summary>
        /// <param name="chatMessageDto"></param>
        /// <returns></returns>
        public async Task<bool> PostGlobalMessage(ChatMessageDto chatMessageDto)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/chat/global", chatMessageDto, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}

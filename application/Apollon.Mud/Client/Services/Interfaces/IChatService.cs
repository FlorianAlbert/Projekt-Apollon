using Apollon.Mud.Shared.Game.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    public interface IChatService
    {
        /// <summary>
        /// The HttpClient used by the service
        /// </summary>
        HttpClient HttpClient { get; }

        /// <summary>
        /// The CancellationTokenSource used by the service
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }
        /// <summary>
        /// Posts a message in the room the sender is in
        /// </summary>
        /// <param name="chatMessageDto"></param>
        /// <returns></returns>
        Task<bool> PostRoomMessage(ChatMessageDto chatMessageDto);

        /// <summary>
        /// Posts a message to a specific avatar
        /// </summary>
        /// <param name="chatMessageDto"></param>
        /// <returns></returns>
        Task<bool> PostWhisperMessage(ChatMessageDto chatMessageDto);

        /// <summary>
        /// Posts a message for all avatars in the dungeon
        /// </summary>
        /// <param name="chatMessageDto"></param>
        /// <returns></returns>
        Task<bool> PostGlobalMessage(ChatMessageDto chatMessageDto);
    }
}

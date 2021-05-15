using Apollon.Mud.Shared.Dungeon.Avatar;
using Apollon.Mud.Shared.Dungeon.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameService
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
        /// Sends a players input to the GameController
        /// </summary>
        /// <param name="message">The Players input</param>
        /// <returns>The Statuscode of the Controller</returns>
        Task<HttpStatusCode> SendCommand(string message);

        /// <summary>
        /// Opens a connection to both Hubs when joining a dungeon
        /// </summary>
        /// <param name="AsMaster">Wether the joining Player is a DM or not</param>
        /// <param name="dungeonId">The ID of the Dungeon that is being joined</param>
        /// <param name="avatarId">The ID of the Avatar the user is joining with</param>
        /// <param name="chatId">The Chat Hub Connection Id</param>
        /// <param name="gameId">The Game Hub Connection Id</param>
        /// <returns>The Statuscode from the controller</returns>
        Task<HttpStatusCode> EnterDungeon(bool AsMaster, Guid dungeonId, Guid? avatarId, string chatId, string gameId);

        /// <summary>
        /// Removes the Avatar from the Dungeon
        /// </summary>
        /// <param name="avatarId"></param>
        /// <returns></returns>
        Task<HttpStatusCode> KickAvatar(Guid avatarId);

        /// <summary>
        /// Kicks a avaters from the Dungeon
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<HttpStatusCode> KickAll(Guid dungeonId);

        /// <summary>
        /// Sends the Dungeon Masters answer to the Player
        /// </summary>
        /// <param name="message"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        Task<HttpStatusCode> AnswerDungeonMasterRequest(string message, AvatarDto avatar);

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        void LeaveGame();


    }
}
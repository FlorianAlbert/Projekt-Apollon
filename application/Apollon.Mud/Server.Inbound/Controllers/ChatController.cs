using System;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Shared.Game.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    /// <summary>
    /// REST-Controller to post new ChatMessages in the game chat
    /// </summary>
    [ApiController]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private IChatService _ChatService;
        private IConnectionService _ConnectionService;

        /// <summary>
        /// Creates a new instance of ChatController
        /// </summary>
        /// <param name="chatService">The <see cref="IChatService"/> that handles the incoming messages</param>
        /// <param name="connectionService">The <see cref="IConnectionService"/> that keeps the existing connections</param>
        public ChatController(IChatService chatService, IConnectionService connectionService)
        {
            _ChatService = chatService;
            _ConnectionService = connectionService;
        }

        /// <summary>
        /// Endpoint to post a message for the room the sender is in
        /// </summary>
        /// <param name="chatMessageDto"><see cref="ChatMessageDto"/> with the containing message informations</param>
        /// <returns>The HttpStatusCode Result</returns>
        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("roomMessage")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostRoomMessage([FromBody] ChatMessageDto chatMessageDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            if (sessionIdClaim is null || !Guid.TryParse(sessionIdClaim.Value, out var sessionId)) return BadRequest();

            var connection = _ConnectionService.GetConnection(userId, sessionId);

            if (connection is null) return BadRequest();

            if (connection.IsDungeonMaster) return Forbid();

            if (chatMessageDto is null) return BadRequest();

            await _ChatService.PostRoomMessage(connection.DungeonId, connection.AvatarId.Value, chatMessageDto.Message);

            return Ok();
        }

        /// <summary>
        /// Endpoint to post a message for the a specific avatar in the room the sender is in or the dungeon the Dungeon Master is part of
        /// </summary>
        /// <param name="chatMessageDto"><see cref="ChatMessageDto"/> with the containing message informations</param>
        /// <returns>The HttpStatusCode Result</returns>
        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("whisper")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostWhisperMessage([FromBody] ChatMessageDto chatMessageDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            if (sessionIdClaim is null || !Guid.TryParse(sessionIdClaim.Value, out var sessionId)) return BadRequest();

            var connection = _ConnectionService.GetConnection(userId, sessionId);

            if (connection is null) return BadRequest();

            if (chatMessageDto is null) return BadRequest();

            await _ChatService.PostWhisperMessage(connection.DungeonId, connection.AvatarId, chatMessageDto.RecipientName, chatMessageDto.Message);

            return Ok();
        }

        /// <summary>
        /// Endpoint to post a message for all the avatars in the dungeon the Dungeon Master is in
        /// </summary>
        /// <param name="chatMessageDto"><see cref="ChatMessageDto"/> with the containing message informations</param>
        /// <returns>The HttpStatusCode Result</returns>
        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("global")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PostGlobalMessage([FromBody] ChatMessageDto chatMessageDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            if (sessionIdClaim is null || !Guid.TryParse(sessionIdClaim.Value, out var sessionId)) return BadRequest();

            var connection = _ConnectionService.GetConnection(userId, sessionId);

            if (connection is null) return BadRequest();

            if (!connection.IsDungeonMaster) return Forbid();

            if (chatMessageDto is null) return BadRequest();

            await _ChatService.PostGlobalMessage(connection.DungeonId, chatMessageDto.Message);

            return Ok();
        }
    }
}
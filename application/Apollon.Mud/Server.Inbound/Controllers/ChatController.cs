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
    [ApiController]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        private IChatService _ChatService;
        private IConnectionService _ConnectionService;

        public ChatController(IChatService chatService, IConnectionService connectionService)
        {
            _ChatService = chatService;
            _ConnectionService = connectionService;
        }

        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("roomMessage")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostRoomMessage(ChatMessageDto chatMessageDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            if (sessionIdClaim is null || !Guid.TryParse(sessionIdClaim.Value, out var sessionId)) return BadRequest();

            var connection = _ConnectionService.GetConnection(userId, sessionId);

            if (connection.AvatarId is null) return Forbid();

            _ChatService.PostRoomMessage(connection.DungeonId, connection.AvatarId.Value, chatMessageDto.Message);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("whisper")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostWhisperMessage(ChatMessageDto chatMessageDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            if (sessionIdClaim is null || !Guid.TryParse(sessionIdClaim.Value, out var sessionId)) return BadRequest();

            var connection = _ConnectionService.GetConnection(userId, sessionId);

            _ChatService.PostWhisperMessage(connection.DungeonId, connection.AvatarId, chatMessageDto.RecipientName, chatMessageDto.Message);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("global")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostGlobalMessage(ChatMessageDto chatMessageDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            if (sessionIdClaim is null || !Guid.TryParse(sessionIdClaim.Value, out var sessionId)) return BadRequest();

            var connection = _ConnectionService.GetConnection(userId, sessionId);

            if (connection.AvatarId is not null) return Forbid();

            _ChatService.PostGlobalMessage(connection.DungeonId, chatMessageDto.Message);

            return Ok();
        }
    }
}
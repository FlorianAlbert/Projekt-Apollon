using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Game;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Shared.Game.Chat;
using Apollon.Mud.Shared.Implementations.Dungeons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    /// <summary>
    /// Controller to play in a dungeon.
    /// </summary>
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        #region member
        /// <summary>
        /// Service to dungeon master actions.
        /// </summary>
        private readonly IMasterService _masterService;

        /// <summary>
        /// Service to execute requests or commands of an avatar.
        /// </summary>
        private readonly IPlayerService _playerService;

        /// <summary>
        /// Service to get the connection of a player and his/her avatar.
        /// </summary>
        private readonly IConnectionService _connectionService;
        #endregion

        public GameController(IMasterService masterService, IPlayerService playerService,
            IConnectionService connectionService, IGameDbService gameDbService)
        {
            _masterService = masterService;
            _playerService = playerService;
            _connectionService = connectionService;
        }

        #region methods
        [HttpGet]
        [Route("command")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Player, Admin")]
        public async Task<IActionResult> SendCommand([FromBody]string message)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");
            if (sessionIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var sessionId)) return BadRequest();

            var userConnection = _connectionService.GetConnection(userId, sessionId);
            if (userConnection is null) return BadRequest();

            var dungeonId = userConnection.DungeonId;

            if (userConnection.IsDungeonMaster) return Forbid();
            
            _playerService.ValidateCommand(userConnection.AvatarId.GetValueOrDefault(), dungeonId, message);
            return Ok();
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Game;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Shared.Game;
using Apollon.Mud.Shared.Game.Chat;
using Apollon.Mud.Shared.Game.DungeonMaster;
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

        /// <summary>
        /// Service to get access to the game db.
        /// </summary>
        private readonly IGameDbService _gameDbService;

        /// <summary>
        /// Service to get access to the user db.
        /// </summary>
        private readonly IUserDbService _userDbService;
        #endregion

        public GameController(IMasterService masterService, IPlayerService playerService,
            IConnectionService connectionService, IGameDbService gameDbService,
            IUserDbService userDbService)
        {
            _masterService = masterService;
            _playerService = playerService;
            _connectionService = connectionService;
            _gameDbService = gameDbService;
            _userDbService = userDbService;
        }

        #region methods
        [HttpPost]
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
            
            await _playerService.ValidateCommand(userConnection.AvatarId.GetValueOrDefault(), dungeonId, message);
            return Ok();
        }

        [HttpPost]
        [Route("enter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Player, Admin")]
        public async Task<IActionResult> EnterDungeon([FromBody] EnterDungeonDto enterDungeonDto)
        {
            if (enterDungeonDto is null) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");
            if (sessionIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var sessionId)) return BadRequest();

            var user = await _userDbService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await _gameDbService.Get<Dungeon>(enterDungeonDto.DungeonId);
            if (dungeon is null) return BadRequest();

            var canEnter = dungeon.WhiteList.Contains(user) && !dungeon.BlackList.Contains(user);
            if (!canEnter) return Forbid();

            if (enterDungeonDto.AsDungeonMaster)
            {
                var isEnteringAsMasterPossible = dungeon.CurrentDungeonMaster is null
                                                 && dungeon.DungeonMasters.Contains(user);
                if (!isEnteringAsMasterPossible) return Forbid();

                if (await _masterService.EnterDungeon(
                    user,
                    sessionId,
                    enterDungeonDto.ChatConnectionId,
                    enterDungeonDto.GameConnectionId,
                    dungeon.Id)) 
                    return Ok();

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (enterDungeonDto.AvatarId is null) return BadRequest();
            var avatar = dungeon.RegisteredAvatars.FirstOrDefault(a => a.Id == enterDungeonDto.AvatarId.GetValueOrDefault());
            if (avatar is null || 
                avatar.Status is Status.Approved || 
                avatar.ChosenClass.Status is Status.Pending || 
                avatar.ChosenRace.Status is Status.Pending) 
                return BadRequest();

            if (avatar.Owner != user) return Forbid();

            if (await _playerService.EnterDungeon(
                userId,
                sessionId,
                enterDungeonDto.ChatConnectionId,
                enterDungeonDto.GameConnectionId,
                dungeon.Id,
                avatar.Id))
                return Ok();

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);

        }

        [HttpPost]
        [Route("kick/{avatarId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Player, Admin")]
        public async Task<IActionResult> KickAvatar([FromRoute] Guid avatarId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await _userDbService.GetUser(userId);
            if (user is null) return BadRequest();

            var avatar = await _gameDbService.Get<Avatar>(avatarId);

            var dungeon = avatar?.Dungeon;
            if (dungeon is null) return BadRequest();

            if (!dungeon.DungeonMasters.Contains(user)) return Forbid();

            await _masterService.KickAvatar(avatarId, dungeon.Id);
            
            return Ok();
        }

        [HttpPost]
        [Route("kickall/{dungeonId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Player, Admin")]
        public async Task<IActionResult> KickAll([FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await _userDbService.GetUser(userId);
            if (user is null) return BadRequest();
            
            var dungeon = await _gameDbService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            if (!dungeon.DungeonMasters.Contains(user) && !await _userDbService.IsUserInRole(user.Id, Roles.Admin.ToString())) return Forbid();
            
            await _masterService.KickAllAvatars(dungeonId);
            return Ok();
        }

        [HttpPost]
        [Route("executeDungeonMasterRequestResponse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ExecuteDungeonMasterRequestResponse([FromBody] DungeonMasterRequestResponseDto dungeonMasterRequestResponseDto)
        {
            if (dungeonMasterRequestResponseDto is null) return BadRequest();
            
            var avatarDto = dungeonMasterRequestResponseDto.Avatar;
            if (avatarDto is null) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");
            if (sessionIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var sessionId)) return BadRequest();

            var user = await _userDbService.GetUser(userId);
            if (user is null) return BadRequest();

            var userConnection = _connectionService.GetConnection(userId, sessionId);
            if (userConnection is null) return BadRequest();
            if (!userConnection.IsDungeonMaster) return Forbid();

            var dungeon = await _gameDbService.Get<Dungeon>(userConnection.DungeonId);
            if (dungeon is null) return BadRequest();

            await _masterService.ExecuteDungeonMasterRequestResponse(
                dungeonMasterRequestResponseDto.Message,
                avatarDto.Id,
                avatarDto.CurrentHealth,
                dungeon.Id);

            return Ok();
        }

        [HttpPost]
        [Route("leave")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LeaveDungeon()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var sessionIdClaim = User.Claims.FirstOrDefault(x => x.Type == "SessionId");
            if (sessionIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var sessionId)) return BadRequest();

            var user = await _userDbService.GetUser(userId);
            if (user is null) return BadRequest();

            var userConnection = _connectionService.GetConnection(userId, sessionId);
            if (userConnection is null) return BadRequest();

            var dungeon = await _gameDbService.Get<Dungeon>(userConnection.DungeonId);
            if (dungeon is null) return BadRequest();

            if (userConnection.IsDungeonMaster)
            {
                if(await _masterService.LeaveDungeon(dungeon.Id, userId, sessionId)) return Ok();
            }
            else
            {
                if (await _playerService.LeaveDungeon(userConnection.AvatarId.GetValueOrDefault(), dungeon.Id))
                    return Ok();
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        #endregion
    }
}

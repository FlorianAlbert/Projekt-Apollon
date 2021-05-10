using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables;
using Apollon.Mud.Shared.Dungeon.Requestable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/specialActions")]
    [ApiController]
    public class SpecialActionController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public SpecialActionController(IGameDbService gameDbService, IUserService userService)
        {
            GameConfigService = gameDbService;
            UserService = userService;
        }

        [HttpPost]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player, Admin")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNew([FromRoute] Guid dungeonId, [FromBody] RequestableDto specialActionDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!(await GameConfigService.Get<Dungeon>(dungeonId)).DungeonMasters.Contains(user)) return Unauthorized();

            var newSpecialAction = new Requestable(specialActionDto.MessageRegex, specialActionDto.PatternForPlayer)
            {
                Status = (Status)specialActionDto.Status
            };

            var actionDungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            actionDungeon.ConfiguredRequestables.Add(newSpecialAction);

            if (await GameConfigService.NewOrUpdate(newSpecialAction))
            {
                if (await GameConfigService.NewOrUpdate(actionDungeon))
                {
                    return Ok(newSpecialAction.Id);
                }
                GameConfigService.Delete<Requestable>(newSpecialAction.Id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Udpate([FromBody] RequestableDto specialActionDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!( await GameConfigService.Get<Dungeon>(dungeonId)).DungeonMasters.Contains(user)) return Unauthorized();

            var actionToUpdate = await GameConfigService.Get<Requestable>(specialActionDto.Id);

            if (actionToUpdate is null) return BadRequest();

            var actionDungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            actionDungeon.ConfiguredRequestables.Remove(actionToUpdate);

            actionToUpdate.Status = (Status)specialActionDto.Status;
            actionToUpdate.MessageRegex = specialActionDto.MessageRegex;
            actionToUpdate.PatternForPlayer = specialActionDto.PatternForPlayer;

            if (await GameConfigService.NewOrUpdate(actionToUpdate))
            {
                if (await GameConfigService .NewOrUpdate(actionDungeon))
                {
                    return Ok();
                }
                GameConfigService.Delete<Requestable>(actionToUpdate.Id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var oldAction = await GameConfigService .Get<Requestable>(specialActionDto.Id);

            var oldActionDto = new RequestableDto
            {
                Status = (int)oldAction.Status,
                Id = oldAction.Id,
                PatternForPlayer = oldAction.PatternForPlayer,
                MessageRegex = oldAction.MessageRegex
            };

            return BadRequest(oldActionDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{actionId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid actionId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!(await GameConfigService.Get<Dungeon>(dungeonId)).DungeonMasters.Contains(user)) return Unauthorized();

            var actionToDelete = GameConfigService.Get<Requestable>(actionId);

            if (actionToDelete is null) return BadRequest();

            if (await GameConfigService.Delete<Requestable>(actionId)) return Ok();

            return BadRequest();            // TODO: evtl ändern
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(ICollection<RequestableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var specialActions = (await GameConfigService.Get<Dungeon>(dungeonId)).ConfiguredRequestables;

            if (!(specialActions is null))
            {
                var specialActionDtos = specialActions.Select(s => new RequestableDto()
                {
                    Id = s.Id,
                    MessageRegex = s.MessageRegex,
                    PatternForPlayer = s.PatternForPlayer,
                    Status = (int)s.Status
                }).ToList();

                return Ok(specialActionDtos);
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{actionId}")]
        [ProducesResponseType(typeof(RequestableDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid actionId)
        {
            var action = (await GameConfigService.Get<Dungeon>(dungeonId)).ConfiguredRequestables.FirstOrDefault(r => r.Id == actionId);

            if (action is null) return BadRequest();

            var actionDto = new RequestableDto
            {
                Id = action.Id,
                PatternForPlayer = action.PatternForPlayer,
                MessageRegex = action.MessageRegex,
                Status = (int)action.Status
            };

            return Ok(actionDto);
        }
    }
}

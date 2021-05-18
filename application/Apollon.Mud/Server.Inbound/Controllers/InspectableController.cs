using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Shared.Dungeon.Inspectable;
using Microsoft.AspNetCore.Authorization;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Shared.Implementations.Dungeons;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/inspectables")]
    [ApiController]
    public class InspectableController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public InspectableController(IGameDbService gameConfigService, IUserService userService)
        {
            GameConfigService = gameConfigService;
            UserService = userService;
        }

        [HttpPost]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player, Admin")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateNew([FromBody] InspectableDto inspectableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (inspectableDto is null) return BadRequest();
            var newInspectable = new Inspectable(
                inspectableDto.Description,
                inspectableDto.Name)
            {
                Status = (Status)inspectableDto.Status,
                Dungeon = dungeon
            };
            
            if (await GameConfigService.NewOrUpdate(newInspectable)) return Ok(newInspectable.Id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] InspectableDto inspectableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (inspectableDto is null) return BadRequest();

            var inspectableToUpdate = dungeon.ConfiguredInspectables.FirstOrDefault(x => x.Id == inspectableDto.Id);
            if (inspectableToUpdate is null) return BadRequest();

            inspectableToUpdate.Name = inspectableDto.Name;
            inspectableToUpdate.Description = inspectableDto.Description;
            inspectableToUpdate.Status = (Status) inspectableDto.Status;
            
            if (await GameConfigService.NewOrUpdate(inspectableToUpdate)) return Ok();

            inspectableToUpdate = await GameConfigService.Get<Inspectable>(inspectableDto.Id);

            var oldInspectableDto = new InspectableDto()
            {
                Description = inspectableToUpdate.Description,
                Id = inspectableToUpdate.Id,
                Name = inspectableToUpdate.Name,
                Status = (int)inspectableToUpdate.Status
            };

            return BadRequest(oldInspectableDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{inspectableId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid inspectableId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();
            
            if (await GameConfigService.Delete<Inspectable>(inspectableId)) return Ok();
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(InspectableDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            //Gets all pure type inspectables and no subclass object
            var inspectables = dungeon.ConfiguredInspectables
                .Where(i => i is not Takeable and not Npc);

            var inspectableDtos = inspectables.Select(inspectable =>
                new InspectableDto
                {
                    Description = inspectable.Description,
                    Id = inspectable.Id,
                    Name = inspectable.Name,
                    Status = (int)inspectable.Status
                }).ToArray();
            return Ok(inspectableDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{inspectableId}")]
        [ProducesResponseType(typeof(InspectableDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid inspectableId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            var inspectable = dungeon.ConfiguredInspectables.FirstOrDefault(x => x.Id == inspectableId);
            if (inspectable is null) return BadRequest();

            var inspectableDto = new InspectableDto()
            {
                Description = inspectable.Description,
                Id = inspectable.Id,
                Name = inspectable.Name,
                Status = (int) inspectable.Status
            };

            return Ok(inspectableDto);
        }
    }
}

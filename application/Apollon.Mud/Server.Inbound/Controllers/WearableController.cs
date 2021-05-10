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
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using Microsoft.AspNetCore.Authorization;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/wearables")]
    [ApiController]
    public class WearableController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public WearableController(IGameDbService gameConfigService, IUserService userService)
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
        public async Task<IActionResult> CreateNew([FromBody] WearableDto wearableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (wearableDto is null) return BadRequest();
            var newWearable = new Wearable(
                wearableDto.Name,
                wearableDto.Description,
                wearableDto.Weight,
                wearableDto.ProtectionBoost)
            {
                Status = (Status)wearableDto.Status,
                Dungeon = dungeon
            };
            
            if (await GameConfigService.NewOrUpdate(newWearable)) return Ok(newWearable.Id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] WearableDto wearableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (wearableDto is null) return BadRequest();

            if (dungeon.ConfiguredInspectables.FirstOrDefault(x => x.Id == wearableDto.Id) is not Wearable wearableToUpdate) return BadRequest();

            wearableToUpdate.Name = wearableDto.Name;
            wearableToUpdate.Description = wearableDto.Description;
            wearableToUpdate.Status = (Status)wearableDto.Status;
            wearableToUpdate.ProtectionBoost = wearableDto.ProtectionBoost;
            wearableToUpdate.Weight = wearableDto.Weight;
            
            if (await GameConfigService.NewOrUpdate(wearableToUpdate)) return Ok(wearableToUpdate.Id);

            wearableToUpdate = await GameConfigService.Get<Wearable>(wearableDto.Id);

            var oldWearableDto = new WearableDto()
            {
                Description = wearableToUpdate.Description,
                Id = wearableToUpdate.Id,
                Name = wearableToUpdate.Name,
                Status = (int)wearableToUpdate.Status,
                Weight = wearableToUpdate.Weight,
                ProtectionBoost = wearableToUpdate.ProtectionBoost
            };

            return BadRequest(oldWearableDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{wearableId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid wearableId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();
            
            if (await GameConfigService.Delete<Wearable>(wearableId)) return Ok();
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(WearableDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            //Gets all pure type wearables and no subclass object
            var wearables = dungeon.ConfiguredInspectables
                .OfType<Wearable>();

            var wearableDtos = wearables.Select(wearable =>
                new WearableDto
                {
                    Description = wearable.Description,
                    Id = wearable.Id,
                    Name = wearable.Name,
                    Status = (int)wearable.Status,
                    Weight = wearable.Weight,
                    ProtectionBoost = wearable.ProtectionBoost
                }).ToArray();
            return Ok(wearableDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{wearableId}")]
        [ProducesResponseType(typeof(WearableDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid wearableId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            if (dungeon.ConfiguredInspectables.FirstOrDefault(x => x.Id == wearableId) is not Wearable wearable) return BadRequest();

            var wearableDto = new WearableDto
            {
                Description = wearable.Description,
                Id = wearable.Id,
                Name = wearable.Name,
                Status = (int)wearable.Status,
                Weight = wearable.Weight,
                ProtectionBoost = wearable.ProtectionBoost
            };

            return Ok(wearableDto);
        }
    }
}

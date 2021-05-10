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
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Microsoft.AspNetCore.Authorization;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/takeables")]
    [ApiController]
    public class TakeableController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public TakeableController(IGameDbService gameConfigService, IUserService userService)
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
        public async Task<IActionResult> CreateNew([FromBody] TakeableDto takeableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (takeableDto is null) return BadRequest();
            var newTakeable = new Takeable(
                takeableDto.Weight,
                takeableDto.Description,
                takeableDto.Name)
            {
                Status = (Status)takeableDto.Status,
                Dungeon = dungeon
            };
            
            if (await GameConfigService.NewOrUpdate(newTakeable)) return Ok(newTakeable.Id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] TakeableDto takeableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (takeableDto is null) return BadRequest();

            if (dungeon.ConfiguredInspectables.FirstOrDefault(x => x.Id == takeableDto.Id) is not Takeable takeableToUpdate) 
                return BadRequest();

            takeableToUpdate.Name = takeableDto.Name;
            takeableToUpdate.Description = takeableDto.Description;
            takeableToUpdate.Status = (Status)takeableDto.Status;
            takeableToUpdate.Weight = takeableToUpdate.Weight;
            
            if (await GameConfigService.NewOrUpdate(takeableToUpdate)) return Ok(takeableToUpdate.Id);

            takeableToUpdate = await GameConfigService.Get<Takeable>(takeableDto.Id);

            var oldTakeableDto = new TakeableDto()
            {
                Description = takeableToUpdate.Description,
                Id = takeableToUpdate.Id,
                Name = takeableToUpdate.Name,
                Status = (int)takeableToUpdate.Status,
                Weight = takeableToUpdate.Weight
            };

            return BadRequest(oldTakeableDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{takeableId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid takeableId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();
            
            if (await GameConfigService.Delete<Takeable>(takeableId)) return Ok();
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(TakeableDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            //Gets all pure type takeables and no subclass object
            var takeables = dungeon.ConfiguredInspectables
                .OfType<Takeable>()
                .Where(i => i is not Consumable and not Wearable and not Usable);

            var takeableDtos = takeables.Select(takeable =>
                new TakeableDto
                {
                    Description = takeable.Description,
                    Id = takeable.Id,
                    Name = takeable.Name,
                    Status = (int)takeable.Status,
                    Weight = takeable.Weight
                }).ToArray();
            return Ok(takeableDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{takeableId}")]
        [ProducesResponseType(typeof(TakeableDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid takeableId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            if (dungeon.ConfiguredInspectables.FirstOrDefault(x => x.Id == takeableId) is not Takeable takeable) return BadRequest();

            var takeableDto = new TakeableDto
            {
                Description = takeable.Description,
                Id = takeable.Id,
                Name = takeable.Name,
                Status = (int)takeable.Status,
                Weight = takeable.Weight
            };

            return Ok(takeableDto);
        }
    }
}

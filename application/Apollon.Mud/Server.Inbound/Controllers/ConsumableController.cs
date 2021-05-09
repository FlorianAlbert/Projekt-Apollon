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
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Microsoft.AspNetCore.Authorization;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/consumables")]
    [ApiController]
    public class ConsumableController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public ConsumableController(IGameDbService gameConfigService, IUserService userService)
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
        public async Task<IActionResult> CreateNew([FromBody] ConsumableDto consumableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (consumableDto is null) return BadRequest();
            var newConsumable = new Consumable(
                consumableDto.Description,
                consumableDto.Name,
                consumableDto.Weight,
                consumableDto.EffectDescription)
            {
                Status = (Status)consumableDto.Status,
                Dungeon = dungeon
            };

            var consumableSaved = await GameConfigService.NewOrUpdate(newConsumable);
            if (consumableSaved) return Ok(newConsumable.Id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] ConsumableDto consumableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (consumableDto is null) return BadRequest();

            var consumableToUpdate = await GameConfigService.Get<Consumable>(consumableDto.Id);
            if (consumableToUpdate is null) return BadRequest();

            consumableToUpdate.Name = consumableDto.Name;
            consumableToUpdate.Description = consumableDto.Description;
            consumableToUpdate.Status = (Status)consumableDto.Status;

            var consumableSaved = await GameConfigService.NewOrUpdate(consumableToUpdate);
            if (consumableSaved) return Ok(consumableToUpdate.Id);

            consumableToUpdate = await GameConfigService.Get<Consumable>(consumableDto.Id);

            var oldConsumableDto = new ConsumableDto()
            {
                Description = consumableToUpdate.Description,
                Id = consumableToUpdate.Id,
                Name = consumableToUpdate.Name,
                Status = (int)consumableToUpdate.Status,
                Weight = consumableToUpdate.Weight,
                EffectDescription = consumableToUpdate.EffectDescription
            };

            return BadRequest(oldConsumableDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{consumableId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid consumableId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();
            if (dungeon.Status is Status.Approved) return Forbid();

            var consumableDeleted = await GameConfigService.Delete<Consumable>(consumableId);
            if (consumableDeleted) return Ok();
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(ICollection<ConsumableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            if (dungeon.ConfiguredInspectables is null) return BadRequest();

            //Gets all pure type consumables and no subclass object
            var consumables = dungeon.ConfiguredInspectables
                .OfType<Consumable>()
                .Where(i => !i.GetType().IsSubclassOf(typeof(Consumable))).ToList();

            var consumableDtos = consumables.Select(consumable =>
                new ConsumableDto()
                {
                    Description = consumable.Description,
                    Id = consumable.Id,
                    Name = consumable.Name,
                    Status = (int)consumable.Status,
                    Weight = consumable.Weight,
                    EffectDescription = consumable.EffectDescription
                });
            return Ok(consumableDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{consumableId}")]
        [ProducesResponseType(typeof(ConsumableDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid consumableId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            var consumable = await GameConfigService.Get<Consumable>(consumableId);
            if (consumable is null) return BadRequest();

            var consumableDto = new ConsumableDto()
            {
                Description = consumable.Description,
                Id = consumable.Id,
                Name = consumable.Name,
                Status = (int)consumable.Status,
                Weight = consumable.Weight,
                EffectDescription = consumable.EffectDescription
            };

            return Ok(consumableDto);
        }
    }
}

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
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Microsoft.AspNetCore.Authorization;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/usables")]
    [ApiController]
    public class UsableController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public UsableController(IGameDbService gameConfigService, IUserService userService)
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
        public async Task<IActionResult> CreateNew([FromBody] UsableDto usableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (usableDto is null) return BadRequest();
            var newUsable = new Usable(
                usableDto.Description,
                usableDto.Name,
                usableDto.Weight,
                usableDto.DamageBoost)
            {
                Status = (Status)usableDto.Status,
                Dungeon = dungeon
            };

            var usableSaved = await GameConfigService.NewOrUpdate(newUsable);
            if (usableSaved) return Ok(newUsable.Id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] UsableDto usableDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (usableDto is null) return BadRequest();

            var usableToUpdate = await GameConfigService.Get<Usable>(usableDto.Id);
            if (usableToUpdate is null) return BadRequest();

            usableToUpdate.Name = usableDto.Name;
            usableToUpdate.Description = usableDto.Description;
            usableToUpdate.Status = (Status)usableDto.Status;

            var usableSaved = await GameConfigService.NewOrUpdate(usableToUpdate);
            if (usableSaved) return Ok(usableToUpdate.Id);

            usableToUpdate = await GameConfigService.Get<Usable>(usableDto.Id);

            var oldUsableDto = new UsableDto()
            {
                Description = usableToUpdate.Description,
                Id = usableToUpdate.Id,
                Name = usableToUpdate.Name,
                Status = (int)usableToUpdate.Status,
                Weight = usableToUpdate.Weight,
                DamageBoost = usableToUpdate.DamageBoost
            };

            return BadRequest(oldUsableDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{usableId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid usableId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();
            if (dungeon.Status is Status.Approved) return Forbid();

            var usableDeleted = await GameConfigService.Delete<Usable>(usableId);
            if (usableDeleted) return Ok();
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(ICollection<UsableDto>), StatusCodes.Status200OK)]
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

            //Gets all pure type usables and no subclass object
            var usables = dungeon.ConfiguredInspectables
                .OfType<Usable>().ToList();

            var usableDtos = usables.Select(usable =>
                new UsableDto()
                {
                    Description = usable.Description,
                    Id = usable.Id,
                    Name = usable.Name,
                    Status = (int)usable.Status,
                    Weight = usable.Weight,
                    DamageBoost = usable.DamageBoost
                });
            return Ok(usableDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{usableId}")]
        [ProducesResponseType(typeof(UsableDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid usableId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            var usable = await GameConfigService.Get<Usable>(usableId);
            if (usable is null) return BadRequest();

            var usableDto = new UsableDto()
            {
                Description = usable.Description,
                Id = usable.Id,
                Name = usable.Name,
                Status = (int)usable.Status,
                Weight = usable.Weight,
                DamageBoost = usable.DamageBoost
            };

            return Ok(usableDto);
        }
    }
}

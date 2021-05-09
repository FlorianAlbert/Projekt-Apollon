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
using Apollon.Mud.Server.Model.Implementations.Dungeons.Races;
using Apollon.Mud.Shared.Dungeon.Race;
using Microsoft.AspNetCore.Authorization;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/races")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public RaceController(IGameDbService gameConfigService, IUserService userService)
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
        public async Task<IActionResult> CreateNew([FromBody] RaceDto raceDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (raceDto is null) return BadRequest();
            var newRace = new Race(
                raceDto.Name,
                raceDto.Description,
                raceDto.DefaultHealth,
                raceDto.DefaultProtection,
                raceDto.DefaultDamage)
            {
                Status = (Status) raceDto.Status,
                Dungeon = dungeon
            };
            
            var raceSaved = await GameConfigService.NewOrUpdate(newRace);
            if (raceSaved) return Ok(newRace.Id);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] RaceDto raceDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (raceDto is null) return BadRequest();

            var raceToUpdate = await GameConfigService.Get<Race>(raceDto.Id);
            if (raceToUpdate is null) return BadRequest();

            raceToUpdate.Name = raceDto.Name;
            raceToUpdate.Description = raceDto.Description;
            raceToUpdate.DefaultHealth = raceDto.DefaultHealth;
            raceToUpdate.DefaultProtection = raceDto.DefaultProtection;
            raceToUpdate.DefaultDamage = raceDto.DefaultDamage;
            raceToUpdate.Status = (Status) raceDto.Status;

            var raceSaved = await GameConfigService.NewOrUpdate(raceToUpdate);
            if (raceSaved) return Ok(raceToUpdate.Id);

            raceToUpdate = await GameConfigService.Get<Race>(raceDto.Id);
            var oldRaceDto = new RaceDto()
            {
                DefaultDamage = raceToUpdate.DefaultDamage,
                DefaultHealth = raceToUpdate.DefaultHealth,
                DefaultProtection = raceToUpdate.DefaultProtection,
                Description = raceToUpdate.Description,
                Id = raceToUpdate.Id,
                Name = raceToUpdate.Name,
                Status = (int)raceToUpdate.Status
            };

            return BadRequest(oldRaceDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{raceId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid raceId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();
            if (dungeon.Status is Status.Approved) return Forbid();

            var raceDeleted = await GameConfigService.Delete<Race>(raceId);
            if (raceDeleted) return Ok();
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(ICollection<RaceDto>), StatusCodes.Status200OK)]
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

            var races = dungeon.ConfiguredRaces;
            if (races is null) return BadRequest();

            var raceDtos = races.Select(race =>
                new RaceDto() {
                    DefaultDamage = race.DefaultDamage,
                    DefaultHealth = race.DefaultHealth,
                    DefaultProtection = race.DefaultProtection,
                    Description = race.Description,
                    Id = race.Id,
                    Name = race.Name,
                    Status = (int)race.Status
                });
            return Ok(raceDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{raceId}")]
        [ProducesResponseType(typeof(RaceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid raceId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            var race = await GameConfigService.Get<Race>(raceId);
            if (race is null) return BadRequest();

            var raceDto = new RaceDto()
            {
                DefaultDamage = race.DefaultDamage,
                DefaultHealth = race.DefaultHealth,
                DefaultProtection = race.DefaultProtection,
                Description = race.Description,
                Id = race.Id,
                Name = race.Name,
                Status = (int) race.Status
            };

            return Ok(raceDto);
        }
    }
}

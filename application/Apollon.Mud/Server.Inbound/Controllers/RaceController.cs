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
            if (dungeon.DungeonMasters.All(x => x.Id != user.Id)) return Unauthorized();

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

            //dungeon.ConfiguredRaces.Add(newRace);

            var raceSaved = await GameConfigService.NewOrUpdate(newRace);
            if (!raceSaved) return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return Ok(newRace.Id);
            //var dungeonSaved = await GameConfigService.NewOrUpdate(dungeon);
            //if (dungeonSaved) return Ok(newRace.Id);

            // var raceDeleted = await GameConfigService.Delete<Race>(newRace.Id);
            // while (!raceDeleted)
            // {
            //     raceDeleted = await GameConfigService.Delete<Race>(newRace.Id);
            // }
            // return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Udpate([FromBody] RaceDto raceDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();
            if (dungeon.DungeonMasters.All(x => x.Id != user.Id)) return Unauthorized();

            if (raceDto is null) return BadRequest();
            var newRace = new Race(
                raceDto.Name,
                raceDto.Description,
                raceDto.DefaultHealth,
                raceDto.DefaultProtection,
                raceDto.DefaultDamage)
            {
                Status = (Status)raceDto.Status,
                Dungeon = dungeon
            };

            var raceSaved = await GameConfigService.NewOrUpdate(newRace);
            if (raceSaved) return Ok(newRace.Id);

            var oldRace = await GameConfigService.Get<Race>(raceDto.Id);
            if (oldRace is null) return BadRequest();

            var oldRaceDto = new RaceDto()
            {
                DefaultDamage = oldRace.DefaultDamage,
                DefaultHealth = oldRace.DefaultHealth,
                DefaultProtection = oldRace.DefaultProtection,
                Description = oldRace.Description,
                Id = oldRace.Id,
                Name = oldRace.Name,
                Status = (int) oldRace.Status
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
            if (dungeon.DungeonMasters.All(x => x.Id != user.Id)) return Unauthorized();
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
            if (races.Count == 0) return BadRequest();

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

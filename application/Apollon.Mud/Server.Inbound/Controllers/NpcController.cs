using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Shared.Dungeon.Npc;
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
    [Route("api/npcs")]
    [ApiController]
    public class NpcController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public NpcController(IGameDbService gameDbService, IUserService userService)
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
        public async Task<IActionResult> CreateNew([FromBody] NpcDto npcDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!GameConfigService.Get<Dungeon>(dungeonId).DungeonMasters.Contains(user)) return Unauthorized();

            var newNpc = new Npc(npcDto.Text, npcDto.Description, npcDto.Name) { Status = (Status)npcDto.Status };

            var npcDungeon = GameConfigService.Get<Dungeon>(dungeonId);

            npcDungeon.ConfiguredInspectables.Add(newNpc);

            if (GameConfigService.NewOrUpdate(newNpc))
            {
                if (GameConfigService.NewOrUpdate(npcDungeon))
                {
                    return Ok(newNpc.Id);
                }
                GameConfigService.Delete<Npc>(newNpc.Id);
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Udpate([FromBody] NpcDto npcDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!GameConfigService.Get<Dungeon>(dungeonId).DungeonMasters.Contains(user)) return Unauthorized();

            var npcToUpdate = GameConfigService.Get<Npc>(npcDto.Id);

            if (npcToUpdate is null) return BadRequest();

            var npcDungeon = GameConfigService.Get<Dungeon>(dungeonId);
            npcDungeon.ConfiguredInspectables.Remove(npcToUpdate);

            npcToUpdate.Status = (Status)npcDto.Status;
            npcToUpdate.Name = npcDto.Name;
            npcToUpdate.Text = npcDto.Text;
            npcToUpdate.Description = npcDto.Description;

            npcDungeon.ConfiguredInspectables.Add(npcToUpdate);

            if (GameConfigService.NewOrUpdate(npcToUpdate))
            {
                if (GameConfigService.NewOrUpdate(npcDungeon))
                {
                    return Ok();
                }
                GameConfigService.Delete<Npc>(npcToUpdate.Id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var oldNpc = GameConfigService.Get<Npc>(npcDto.Id);

            var oldNpcDto = new NpcDto
            {
                Status = (int)oldNpc.Status,
                Id = oldNpc.Id,
                Name = oldNpc.Name,
                Text = oldNpc.Text,
                Description = oldNpc.Description
            };

            return BadRequest(oldNpcDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{npcId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid npcId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!GameConfigService.Get<Dungeon>(dungeonId).DungeonMasters.Contains(user)) return Unauthorized();

            var npcToDelete = GameConfigService.Get<Npc>(npcId);

            if (npcToDelete is null) return BadRequest();

            if (GameConfigService.Delete<Npc>(npcId)) return Ok();

            return BadRequest();     // TODO: evtl ändern
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(ICollection<NpcDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var npcs = GameConfigService.Get<Dungeon>(dungeonId).ConfiguredInspectables.OfType<Npc>();

            if (!(npcs is null))
            {
                var specialActionDtos = npcs.Select(n => new NpcDto()
                {
                    Id = n.Id,
                    Description = n.Description,
                    Name = n.Name,
                    Text = n.Text,
                    Status = (int)n.Status
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
            var npc = GameConfigService.Get<Dungeon>(dungeonId).ConfiguredInspectables.OfType<Npc>().FirstOrDefault(n => n.Id == actionId);

            if (npc is null) return BadRequest();

            var npcDto = new NpcDto
            {
                Text = npc.Text,
                Name = npc.Name,
                Description = npc.Description,
                Status = (int)npc.Status,
                Id = npc.Id
            };

            return Ok(npcDto);
        }
    }
}

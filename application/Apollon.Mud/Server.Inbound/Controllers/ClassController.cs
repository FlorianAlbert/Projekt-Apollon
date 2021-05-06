﻿using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables;
using Apollon.Mud.Shared.Dungeon.Class;
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
    public class ClassController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public ClassController(IGameDbService gameDbService, IUserService userService)
        {
            GameConfigService = gameDbService;
            UserService = userService;
        }

        [HttpPost]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player, Admin")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNew([FromBody] ClassDto classDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!GameConfigService.Get<Dungeon>(dungeonId).DungeonMasters.Contains(user)) return Unauthorized();

            var newClass = new Class(classDto.Name, classDto.Description, classDto.DefaultHealth, classDto.DefaultProtection, classDto.DefaultDamage)
            {
                Status = (Status)classDto.Status,
                
            };

            var classDungeon = GameConfigService.Get<Dungeon>(dungeonId);

            classDungeon.ConfiguredClasses.Add(newClass);

            if (GameConfigService.NewOrUpdate(newClass))
            {
                if (GameConfigService.NewOrUpdate(classDungeon))
                {
                    return Ok(newClass.Id);
                }
                GameConfigService.Delete<Npc>(newClass.Id);
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
        public async Task<IActionResult> Udpate([FromBody] ClassDto classDto, [FromRoute] Guid dungeonId)
        {

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!GameConfigService.Get<Dungeon>(dungeonId).DungeonMasters.Contains(user)) return Unauthorized();

            var classToUpdate = GameConfigService.Get<Class>(classDto.Id);

            if (classToUpdate is null) return BadRequest();

            var classDungeon = GameConfigService.Get<Dungeon>(dungeonId);
            classDungeon.ConfiguredClasses.Remove(classToUpdate);

            classToUpdate.Status = (Status)classDto.Status;
            classToUpdate.Name = classDto.Name;
            classToUpdate.DefaultDamage = classDto.DefaultDamage;
            classToUpdate.DefaultHealth = classDto.DefaultHealth;
            classToUpdate.DefaultProtection = classDto.DefaultProtection;
            classToUpdate.Description = classDto.Description;
            classToUpdate.

            classDungeon.ConfiguredInspectables.Add(classToUpdate);

            if (GameConfigService.NewOrUpdate(classToUpdate))
            {
                if (GameConfigService.NewOrUpdate(classDungeon))
                {
                    return Ok();
                }
                GameConfigService.Delete<Npc>(classToUpdate.Id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var oldNpc = GameConfigService.Get<Npc>(classDto.Id);

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

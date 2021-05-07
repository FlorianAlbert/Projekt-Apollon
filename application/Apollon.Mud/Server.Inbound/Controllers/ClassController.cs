using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Shared.Dungeon.Class;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
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

            var newClass = new Class(classDto.Name, 
                classDto.Description, 
                classDto.DefaultHealth, 
                classDto.DefaultProtection, 
                classDto.DefaultDamage)
                { Status = (Status)classDto.Status };

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
            foreach(ConsumableDto consumable in classDto.InventoryConsumableDtos)
            {
                classToUpdate.StartInventory.Add(
                    new Consumable(consumable.Name, consumable.Description, consumable.Weight, consumable.EffectDescription)
                    { Status = (Status)consumable.Status });
            }
            foreach (WearableDto wearable in classDto.InventoryWearableDtos)
            {
                classToUpdate.StartInventory.Add(
                    new Wearable(wearable.Name, wearable.Description, wearable.Weight, wearable.ProtectionBoost)
                    { Status = (Status)wearable.Status });
            }
            foreach (UsableDto usable in classDto.InventoryUsableDtos)
            {
                classToUpdate.StartInventory.Add(
                    new Usable(usable.Name, usable.Description, usable.Weight, usable.DamageBoost)
                    { Status = (Status)usable.Status });
            }
            foreach (TakeableDto takeable in classDto.InventoryTakeableDtos)
            {
                classToUpdate.StartInventory.Add(
                    new Takeable(takeable.Weight, takeable.Description, takeable.Name)
                    { Status = (Status)takeable.Status });
            }

            classDungeon.ConfiguredClasses.Add(classToUpdate);

            if (GameConfigService.NewOrUpdate(classToUpdate))
            {
                if (GameConfigService.NewOrUpdate(classDungeon))
                {
                    return Ok();
                }
                GameConfigService.Delete<Npc>(classToUpdate.Id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var oldClass = GameConfigService.Get<Class>(classDto.Id);

            var oldClassDto = new ClassDto
            {
                Status = (int)oldClass.Status,
                Id = oldClass.Id,
                Name = oldClass.Name,
                Description = oldClass.Description,
                DefaultHealth = oldClass.DefaultHealth,
                DefaultDamage = oldClass.DefaultDamage,
                DefaultProtection = oldClass.DefaultProtection,
                InventoryTakeableDtos = oldClass.StartInventory.OfType<Takeable>().Where
                (x => !x.GetType().IsSubclassOf(typeof(Takeable))).Select(x => new TakeableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Description,
                    Weight = x.Weight
                }).ToList(),
                InventoryUsableDtos = oldClass.StartInventory.OfType<Usable>().Select(x => new UsableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Description,
                    Weight = x.Weight,
                    DamageBoost = x.DamageBoost
                }).ToList(),
                InventoryConsumableDtos = oldClass.StartInventory.OfType<Consumable>().Select(x => new ConsumableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Description,
                    Weight = x.Weight,
                    EffectDescription = x.EffectDescription
                }).ToList(),
                InventoryWearableDtos = oldClass.StartInventory.OfType<Wearable>().Select(x => new WearableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Description,
                    Weight = x.Weight,
                    ProtectionBoost = x.ProtectionBoost
                }).ToList(),
            };

            return BadRequest(oldClassDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{classId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid classId)
        {
            if (GameConfigService.Get<Dungeon>(dungeonId) is null) return BadRequest();

            if (GameConfigService.Get<Dungeon>(dungeonId).Status is Status.Approved) return Forbid();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!GameConfigService.Get<Dungeon>(dungeonId).DungeonMasters.Contains(user)) return Unauthorized();

            var classToDelete = GameConfigService.Get<Class>(classId);

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

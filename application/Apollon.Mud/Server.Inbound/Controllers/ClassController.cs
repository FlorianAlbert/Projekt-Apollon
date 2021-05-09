using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Shared.Dungeon.Class;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/classes")]
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

            if ((await GameConfigService.Get<Dungeon>(dungeonId)).DungeonMasters.All(x => x.Id != user.Id)) return Unauthorized();

            var newClass = new Class(classDto.Name, 
                classDto.Description, 
                classDto.DefaultHealth, 
                classDto.DefaultProtection, 
                classDto.DefaultDamage)
                { Status = (Status)classDto.Status };

            var classDungeon = await GameConfigService.Get<Dungeon>(dungeonId);//würde ich weiter vorne machen, dann kannst du bei der Authorization Abfrage die Variable benutzen

            classDungeon.ConfiguredClasses.Add(newClass);

            if (await GameConfigService.NewOrUpdate(newClass))
            {
                if (await GameConfigService.NewOrUpdate(classDungeon))
                {
                    return Ok(newClass.Id);
                }
                await GameConfigService.Delete<Class>(newClass.Id);
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
        public async Task<IActionResult> Update([FromBody] ClassDto classDto, [FromRoute] Guid dungeonId)
        {

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!(await GameConfigService.Get<Dungeon>(dungeonId)).DungeonMasters.Contains(user)) return Unauthorized();

            var classToUpdate = await GameConfigService.Get<Class>(classDto.Id);

            if (classToUpdate is null) return BadRequest();

            var classDungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            classDungeon.ConfiguredClasses.Remove(classToUpdate);//müsste nicht aufgerufen werden, da die Klasse nur geupdated wird oder?

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

            if (await GameConfigService.NewOrUpdate(classToUpdate))
            {
                if (await GameConfigService.NewOrUpdate(classDungeon))
                {
                    return Ok();
                }
                await GameConfigService.Delete<Class>(classToUpdate.Id);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var oldClass = await GameConfigService.Get<Class>(classDto.Id);

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
            if (await GameConfigService.Get<Dungeon>(dungeonId) is null) return BadRequest();

            if ((await GameConfigService.Get<Dungeon>(dungeonId)).Status is Status.Approved) return Forbid();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!(await GameConfigService.Get<Dungeon>(dungeonId)).DungeonMasters.Contains(user)) return Unauthorized();

            var classToDelete = await GameConfigService.Get<Class>(classId);

            if (classToDelete is null) return BadRequest();

            if (await GameConfigService.Delete<Class>(classId)) return Ok();

            return BadRequest();     // TODO: evtl ändern
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(ICollection<ClassDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var classesDungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (classesDungeon is null) return BadRequest();

            var classes = classesDungeon.ConfiguredClasses;

            if (!(classes is null))
            {
                var ClassDtos = classes.Select(c => new ClassDto()
                {
                    Id = c.Id,
                    Description = c.Description,
                    Name = c.Name,
                    Status = (int)c.Status,
                    DefaultDamage = c.DefaultDamage,
                    DefaultHealth = c.DefaultHealth,
                    DefaultProtection = c.DefaultProtection,
                    InventoryTakeableDtos = c.StartInventory.OfType<Takeable>().Where
                    (x => !x.GetType().IsSubclassOf(typeof(Takeable))).Select
                    (x => new TakeableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight
                    }).ToList(),
                    InventoryUsableDtos = c.StartInventory.OfType<Usable>().Select
                    (x => new UsableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        DamageBoost = x.DamageBoost
                    }).ToList(),
                    InventoryConsumableDtos = c.StartInventory.OfType<Consumable>().Select
                    (x => new ConsumableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        EffectDescription = x.EffectDescription
                    }).ToList(),
                    InventoryWearableDtos = c.StartInventory.OfType<Wearable>().Select
                    (x => new WearableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        ProtectionBoost = x.ProtectionBoost
                    }).ToList(),
                }).ToList();

                return Ok(ClassDtos);
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{classId}")]
        [ProducesResponseType(typeof(ClassDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid classId)
        {
            var classesDungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (classesDungeon is null) return BadRequest();

            var classToSend = classesDungeon.ConfiguredClasses.FirstOrDefault(c => c.Id == classId);

            if (!(classToSend is null))
            {
                var ClassDto = new ClassDto()
                {
                    Id = classToSend.Id,
                    Description = classToSend.Description,
                    Name = classToSend.Name,
                    Status = (int)classToSend.Status,
                    DefaultDamage = classToSend.DefaultDamage,
                    DefaultHealth = classToSend.DefaultHealth,
                    DefaultProtection = classToSend.DefaultProtection,
                    InventoryTakeableDtos = classToSend.StartInventory.OfType<Takeable>().Where
                    (x => !x.GetType().IsSubclassOf(typeof(Takeable))).Select
                    (x => new TakeableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight
                    }).ToList(),
                    InventoryUsableDtos = classToSend.StartInventory.OfType<Usable>().Select
                    (x => new UsableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        DamageBoost = x.DamageBoost
                    }).ToList(),
                    InventoryConsumableDtos = classToSend.StartInventory.OfType<Consumable>().Select
                    (x => new ConsumableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        EffectDescription = x.EffectDescription
                    }).ToList(),
                    InventoryWearableDtos = classToSend.StartInventory.OfType<Wearable>().Select
                    (x => new WearableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        ProtectionBoost = x.ProtectionBoost
                    }).ToList(),
                };

                return Ok(ClassDto);
            }
            return BadRequest();
        }
    }
}

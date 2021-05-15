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
using Apollon.Mud.Shared.Implementations.Dungeons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Game;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/classes")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        private IMasterService MasterService { get; }

        public ClassController(IGameDbService gameDbService, IUserService userService, IMasterService masterService)
        {
            GameConfigService = gameDbService;
            UserService = userService;
            MasterService = masterService;
        }

        [HttpPost]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player, Admin")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNew([FromBody] ClassDto classDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            var classDungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (user is null || classDungeon is null) return BadRequest();

            if (!classDungeon.DungeonMasters.Contains(user)) return Unauthorized();

            var newClass = new Class(classDto.Name,
                classDto.Description,
                classDto.DefaultHealth,
                classDto.DefaultProtection,
                classDto.DefaultDamage)
            {
                Status = (Status)classDto.Status,
                Dungeon = classDungeon
            };

            foreach (var consumableDto in classDto.InventoryConsumableDtos)
            {
                var consumable = await GameConfigService.Get<Consumable>(consumableDto.Id);

                if (consumable is not null) newClass.StartInventory.Add(consumable);
            }

            foreach (var takeableDto in classDto.InventoryTakeableDtos)
            {
                var takeable = await GameConfigService.Get<Takeable>(takeableDto.Id);

                if (takeable is not null) newClass.StartInventory.Add(takeable);
            }

            foreach (var usableDto in classDto.InventoryUsableDtos)
            {
                var usable = await GameConfigService.Get<Usable>(usableDto.Id);

                if (usable is not null) newClass.StartInventory.Add(usable);
            }

            foreach (var wearableDto in classDto.InventoryWearableDtos)
            {
                var wearable = await GameConfigService.Get<Wearable>(wearableDto.Id);

                if (wearable is not null) newClass.StartInventory.Add(wearable);
            }

            if (await GameConfigService.NewOrUpdate(newClass)) return Ok(newClass.Id);

            return BadRequest();
        }

        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] ClassDto classDto, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            var classToUpdate = dungeon.ConfiguredClasses.FirstOrDefault(x => x.Id == classDto.Id);

            if (classToUpdate is null) return BadRequest();

            classToUpdate.Status = (Status)classDto.Status;
            classToUpdate.Name = classDto.Name;
            classToUpdate.DefaultDamage = classDto.DefaultDamage;
            classToUpdate.DefaultHealth = classDto.DefaultHealth;
            classToUpdate.DefaultProtection = classDto.DefaultProtection;
            classToUpdate.Description = classDto.Description;

            classToUpdate.StartInventory.Clear();

            if (!await GameConfigService.NewOrUpdate(classToUpdate)) return BadRequest();

            foreach (var consumableDto in classDto.InventoryConsumableDtos)
            {
                var consumable = await GameConfigService.Get<Consumable>(consumableDto.Id);

                if (consumable is not null) classToUpdate.StartInventory.Add(consumable);
            }

            foreach (var takeableDto in classDto.InventoryTakeableDtos)
            {
                var takeable = await GameConfigService.Get<Takeable>(takeableDto.Id);

                if (takeable is not null) classToUpdate.StartInventory.Add(takeable);
            }

            foreach (var usableDto in classDto.InventoryUsableDtos)
            {
                var usable = await GameConfigService.Get<Usable>(usableDto.Id);

                if (usable is not null) classToUpdate.StartInventory.Add(usable);
            }

            foreach (var wearableDto in classDto.InventoryWearableDtos)
            {
                var wearable = await GameConfigService.Get<Wearable>(wearableDto.Id);

                if (wearable is not null) classToUpdate.StartInventory.Add(wearable);
            }

            if (await GameConfigService.NewOrUpdate(classToUpdate))
            {
                if (classToUpdate.Status is not Status.Pending) return Ok();
                foreach (var avatar in classToUpdate.Avatars)
                {
                    if (avatar.Status is Status.Pending) continue;
                    await MasterService.KickAvatar(avatar.Id, dungeonId);
                }
                return Ok();
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
                (x => x is not Consumable and not Wearable and not Usable).Select(x => new TakeableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight
                }).ToList(),
                InventoryUsableDtos = oldClass.StartInventory.OfType<Usable>().Select(x => new UsableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight,
                    DamageBoost = x.DamageBoost
                }).ToList(),
                InventoryConsumableDtos = oldClass.StartInventory.OfType<Consumable>().Select(x => new ConsumableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight,
                    EffectDescription = x.EffectDescription
                }).ToList(),
                InventoryWearableDtos = oldClass.StartInventory.OfType<Wearable>().Select(x => new WearableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
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
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            if (dungeon.Status is Status.Approved) return Forbid();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (await GameConfigService.Delete<Class>(classId)) return Ok();

            return BadRequest();     // TODO: evtl ändern
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(ClassDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var classesDungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (classesDungeon is null) return BadRequest();

            var classDtos = classesDungeon.ConfiguredClasses.Select(c => new ClassDto()
            {
                Id = c.Id,
                Description = c.Description,
                Name = c.Name,
                Status = (int)c.Status,
                DefaultDamage = c.DefaultDamage,
                DefaultHealth = c.DefaultHealth,
                DefaultProtection = c.DefaultProtection,
                InventoryTakeableDtos = c.StartInventory.OfType<Takeable>().Where
                (x => x is not Consumable and not Wearable and not Usable).Select
                (x => new TakeableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight
                }).ToList(),
                InventoryUsableDtos = c.StartInventory.OfType<Usable>().Select
                (x => new UsableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight,
                    DamageBoost = x.DamageBoost
                }).ToList(),
                InventoryConsumableDtos = c.StartInventory.OfType<Consumable>().Select
                (x => new ConsumableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight,
                    EffectDescription = x.EffectDescription
                }).ToList(),
                InventoryWearableDtos = c.StartInventory.OfType<Wearable>().Select
                (x => new WearableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight,
                    ProtectionBoost = x.ProtectionBoost
                }).ToList(),
            }).ToArray();

            return Ok(classDtos);
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

            if (classToSend is null) return BadRequest();

            var classDto = new ClassDto()
            {
                Id = classToSend.Id,
                Description = classToSend.Description,
                Name = classToSend.Name,
                Status = (int)classToSend.Status,
                DefaultDamage = classToSend.DefaultDamage,
                DefaultHealth = classToSend.DefaultHealth,
                DefaultProtection = classToSend.DefaultProtection,
                InventoryTakeableDtos = classToSend.StartInventory.OfType<Takeable>().Where
                    (x => x is not Consumable and not Wearable and not Usable).Select
                (x => new TakeableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight
                }).ToList(),
                InventoryUsableDtos = classToSend.StartInventory.OfType<Usable>().Select
                (x => new UsableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight,
                    DamageBoost = x.DamageBoost
                }).ToList(),
                InventoryConsumableDtos = classToSend.StartInventory.OfType<Consumable>().Select
                (x => new ConsumableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight,
                    EffectDescription = x.EffectDescription
                }).ToList(),
                InventoryWearableDtos = classToSend.StartInventory.OfType<Wearable>().Select
                (x => new WearableDto
                {
                    Id = x.Id,
                    Status = (int)x.Status,
                    Description = x.Description,
                    Name = x.Name,
                    Weight = x.Weight,
                    ProtectionBoost = x.ProtectionBoost
                }).ToList(),
            };

            return Ok(classDto);
        }
    }
}

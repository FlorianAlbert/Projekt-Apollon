using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Races;
using Apollon.Mud.Shared.Dungeon.Avatar;
using Apollon.Mud.Shared.Dungeon.Class;
using Apollon.Mud.Shared.Dungeon.Inspectable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Shared.Dungeon.Race;
using Apollon.Mud.Shared.Dungeon.Requestable;
using Apollon.Mud.Shared.Dungeon.Room;
using Apollon.Mud.Shared.Implementations.Dungeons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/avatars")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public AvatarController(IGameDbService gameDbService, IUserService userService)
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
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateNew([FromBody] AvatarDto avatar, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var avatarDungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (avatarDungeon is null) return BadRequest();

            if (!avatarDungeon.WhiteList.Contains(user) || 
                avatarDungeon.BlackList.Contains(user)) return Unauthorized();

            var avatarRace = avatarDungeon.ConfiguredRaces.FirstOrDefault(x => x.Id == avatar.Race.Id);

            var avatarClass = avatarDungeon.ConfiguredClasses.FirstOrDefault(x => x.Id == avatar.Class.Id);

            if (avatarRace is null || avatarClass is null) return BadRequest();

            if (avatarDungeon.RegisteredAvatars.Any(x => x.Name == avatar.Name) ) return Conflict();

            var newAvatar = new Avatar(avatar.Name,
                avatarRace,
                avatarClass,
                (Gender)avatar.Gender)
            {
                CurrentRoom = (await GameConfigService.Get<Dungeon>(dungeonId)).DefaultRoom,
                Status = (Status)avatar.Status,
                Owner = user,
                Dungeon = avatarDungeon
            };

            if (await GameConfigService.NewOrUpdate(newAvatar)) return Ok(newAvatar.Id);

            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{avatarId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId, [FromRoute] Guid avatarId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var avatarToDelete = await GameConfigService.Get<Avatar>(avatarId);

            if (avatarToDelete is null) return BadRequest();

            if (avatarToDelete.Dungeon.Id != dungeonId) return BadRequest();

            if (avatarToDelete.Owner != user) return Unauthorized();

            if (await GameConfigService.Delete<Avatar>(avatarId)) return Ok();

            return BadRequest();            // TODO: evtl ändern
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(AvatarDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var avatars = dungeon.RegisteredAvatars;

            var avatarDtos = avatars.Select(x => new AvatarDto
            {
                Id = x.Id,
                Name = x.Name,
                Status = (int) x.Status,
                Class = new ClassDto
                {
                    Id = x.ChosenClass.Id,
                    Name = x.ChosenClass.Name,
                    Description = x.ChosenClass.Description,
                    Status = (int) x.ChosenClass.Status
                },
                Race = new RaceDto
                {
                    Id = x.ChosenRace.Id,
                    Name = x.ChosenRace.Name,
                    Description = x.ChosenRace.Description,
                    Status = (int)x.ChosenRace.Status
                },
                CurrentRoom = new RoomDto
                {
                    Id = x.CurrentRoom.Id,
                    Name = x.CurrentRoom.Name,
                    Description = x.CurrentRoom.Description,
                    Status = (int) x.CurrentRoom.Status
                },
                HoldingItem = new TakeableDto
                {
                    Id = x.HoldingItem.Id,
                    Name = x.HoldingItem.Name,
                    Description = x.HoldingItem.Description,
                    Status = (int) x.HoldingItem.Status
                },
                Armor = new WearableDto
                {
                    Id = x.Armor.Id,
                    Name = x.Armor.Name,
                    Description = x.Armor.Description,
                    Status = (int) x.Armor.Status
                },
                CurrentHealth = x.CurrentHealth,
                Gender = (int) x.ChosenGender
            }).ToArray();

            return Ok(avatarDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/user")]
        [ProducesResponseType(typeof(AvatarDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllForUser([FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var userAvatars = (await GameConfigService.GetAll<Avatar>()).Where(x => x.Owner == user && x.Dungeon == dungeon);

            var userAvatarDtos = userAvatars.Select(x => new AvatarDto
            {
                Id = x.Id,
                Name = x.Name,
                Status = (int)x.Status,
                Class = new ClassDto
                {
                    Id = x.ChosenClass.Id,
                    Name = x.ChosenClass.Name,
                    Description = x.ChosenClass.Description,
                    Status = (int)x.ChosenClass.Status
                },
                Race = new RaceDto
                {
                    Id = x.ChosenRace.Id,
                    Name = x.ChosenRace.Name,
                    Description = x.ChosenRace.Description,
                    Status = (int)x.ChosenRace.Status
                },
                CurrentRoom = new RoomDto
                {
                    Id = x.CurrentRoom.Id,
                    Name = x.CurrentRoom.Name,
                    Description = x.CurrentRoom.Description,
                    Status = (int)x.CurrentRoom.Status
                },
                HoldingItem = x.HoldingItem is null 
                    ? null 
                    : new TakeableDto
                {
                    Id = x.HoldingItem.Id,
                    Name = x.HoldingItem.Name,
                    Description = x.HoldingItem.Description,
                    Status = (int)x.HoldingItem.Status
                },
                Armor = x.Armor is null 
                    ? null 
                    : new WearableDto
                {
                    Id = x.Armor.Id,
                    Name = x.Armor.Name,
                    Description = x.Armor.Description,
                    Status = (int)x.Armor.Status
                },
                CurrentHealth = x.CurrentHealth,
                Gender = (int)x.ChosenGender
            }).ToArray();

            return Ok(userAvatarDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{avatarId}")]
        [ProducesResponseType(typeof(AvatarDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid avatarId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var avatar = dungeon.RegisteredAvatars.FirstOrDefault(r => r.Id == avatarId);

            if (avatar is null) return BadRequest();

            var avatarDto = new AvatarDto
            {
                Id = avatar.Id,
                Status = (int)avatar.Status,
                Name = avatar.Name,
                Race = new RaceDto
                {
                    Id = avatar.ChosenRace.Id,
                    Status = (int)avatar.ChosenRace.Status,
                    Name = avatar.ChosenRace.Name,
                    Description = avatar.ChosenRace.Description,
                    DefaultHealth = avatar.ChosenRace.DefaultHealth,
                    DefaultProtection = avatar.ChosenRace.DefaultProtection,
                    DefaultDamage = avatar.ChosenRace.DefaultDamage
                },
                Class = new ClassDto
                {
                    Id = avatar.ChosenClass.Id,
                    Status = (int)avatar.ChosenClass.Status,
                    Name = avatar.ChosenClass.Name,
                    Description = avatar.ChosenClass.Description,
                    DefaultHealth = avatar.ChosenClass.DefaultHealth,
                    DefaultProtection = avatar.ChosenClass.DefaultProtection,
                    DefaultDamage = avatar.ChosenClass.DefaultDamage,
                    InventoryTakeableDtos = avatar.Inventory.OfType<Takeable>().Where
                    (x => x is not Consumable and not Wearable and not Usable).Select
                    (x => new TakeableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Name,
                        Weight = x.Weight
                    }).ToList(),
                    InventoryUsableDtos = avatar.Inventory.OfType<Usable>().Select
                    (x => new UsableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Name,
                        Weight = x.Weight,
                        DamageBoost = x.DamageBoost
                    }).ToList(),
                    InventoryConsumableDtos = avatar.Inventory.OfType<Consumable>().Select
                    (x => new ConsumableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Name,
                        Weight = x.Weight,
                        EffectDescription = x.EffectDescription
                    }).ToList(),
                    InventoryWearableDtos = avatar.Inventory.OfType<Wearable>().Select
                    (x => new WearableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Name,
                        Weight = x.Weight,
                        ProtectionBoost = x.ProtectionBoost
                    }).ToList(),
                },
                Gender = (int)avatar.ChosenGender,
                CurrentHealth = avatar.CurrentHealth,
                HoldingItem = new TakeableDto
                {
                    Id = avatar.HoldingItem.Id,
                    Status = (int)avatar.HoldingItem.Status,
                    Description = avatar.HoldingItem.Description,
                    Name = avatar.HoldingItem.Name,
                    Weight = avatar.HoldingItem.Weight
                },
                Armor = new WearableDto
                {
                    Id = avatar.Armor.Id,
                    Status = (int)avatar.Armor.Status,
                    Description = avatar.Armor.Description,
                    Name = avatar.Armor.Name,
                    Weight = avatar.Armor.Weight,
                    ProtectionBoost = avatar.Armor.ProtectionBoost
                },
                CurrentRoom = new RoomDto
                {
                    Id = avatar.CurrentRoom.Id,
                    Status = (int)avatar.CurrentRoom.Status,
                    Description = avatar.CurrentRoom.Description,
                    Name = avatar.CurrentRoom.Name,
                    Inspectables = avatar.CurrentRoom.Inspectables.OfType<Inspectable>().
                    Where(x => x is not Takeable and not Npc).
                    Select(x => new InspectableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Name,
                    }).ToList(),
                    Takeables = avatar.CurrentRoom.Inspectables.OfType<Takeable>().
                    Where(x => x is not Consumable and not Wearable and not Usable).
                    Select(x => new TakeableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Name,
                        Weight = x.Weight
                    }).ToList(),
                    Consumables = avatar.CurrentRoom.Inspectables.OfType<Consumable>().
                    Select(x => new ConsumableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Name,
                        Weight = x.Weight,
                        EffectDescription = x.EffectDescription
                    }).ToList(),
                    Wearables = avatar.CurrentRoom.Inspectables.OfType<Wearable>().
                    Select(x => new WearableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Name,
                        Weight = x.Weight,
                        ProtectionBoost = x.ProtectionBoost
                    }).ToList(),
                    Usables = avatar.CurrentRoom.Inspectables.OfType<Usable>().
                    Select(x => new UsableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Name,
                        Weight = x.Weight,
                        DamageBoost = x.DamageBoost
                    }).ToList(),
                }
            };
            return Ok(avatarDto);
        }
    }
}


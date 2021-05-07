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
        public async Task<IActionResult> CreateNew([FromBody] AvatarDto avatar, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var newAvatar = new Avatar(avatar.Name, await GameConfigService.Get<Race>(avatar.Race.Id), await GameConfigService.Get<Class>(avatar.Class.Id), (Gender)avatar.Gender, await GameConfigService.Get<Dungeon>(dungeonId), user)
            {
                CurrentRoom = (await GameConfigService.Get<Dungeon>(dungeonId)).DefaultRoom,
            };
            foreach(TakeableDto takeable in avatar.Class.InventoryTakeableDtos)
            {
                newAvatar.Inventory.Add(new Takeable(takeable.Weight, takeable.Description, takeable.Name)
                {
                    Status = (Status)takeable.Status
                });
            }
            foreach (UsableDto usable in avatar.Class.InventoryUsableDtos)
            {
                newAvatar.Inventory.Add(new Usable(usable.Name, usable.Description, usable.Weight, usable.DamageBoost)
                {
                    Status = (Status)usable.Status
                });
            }
            foreach (WearableDto wearable in avatar.Class.InventoryWearableDtos)
            {
                newAvatar.Inventory.Add(new Wearable(wearable.Name, wearable.Description, wearable.Weight, wearable.ProtectionBoost)
                {
                    Status = (Status)wearable.Status
                });
            }
            foreach (ConsumableDto consumable in avatar.Class.InventoryConsumableDtos)
            {
                newAvatar.Inventory.Add(new Consumable(consumable.Name, 
                                        consumable.Description, 
                                        consumable.Weight, 
                                        consumable.EffectDescription)
                                        {
                                            Status = (Status)consumable.Status
                                        });
            }

            if (await GameConfigService.NewOrUpdate(newAvatar)) return Ok(newAvatar.Id);

            return BadRequest();
        }

        /*//Brauchen wir überhaupt eine Update?
        [HttpPut]
        [Route("{dungeonId}")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] AvatarDto avatar, [FromRoute] Guid dungeonId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var avatarToUpdate = GameConfigService.Get<Avatar>(avatar.Id);

            if (avatarToUpdate is null) return BadRequest();

            if (avatarToUpdate.Owner.Id != user.Id) return Unauthorized(); 

            avatarToUpdate.ChosenRace = new Race(avatar.Race.Name,
                                        avatar.Race.Description,
                                        avatar.Race.DefaultHealth,
                                        avatar.Race.DefaultProtection,
                                        avatar.Race.DefaultDamage)
                                        {
                                            Status = (Status) avatar.Race.Status
                                        };

            avatarToUpdate.ChosenClass = new Class(avatar.Class.Name, 
                                        avatar.Class.Description, 
                                        avatar.Class.DefaultHealth, 
                                        avatar.Class.DefaultProtection, 
                                        avatar.Class.DefaultDamage) 
                                        {
                                            Status = (Status) avatar.Class.Status
                                        };

            avatarToUpdate.ChosenGender = (Gender)avatar.Gender;
            avatarToUpdate.Dungeon = GameConfigService.Get<Dungeon>(dungeonId);
            avatarToUpdate.CurrentHealth = avatar.CurrentHealth;
            avatarToUpdate.Inventory = avatar.


            avatarToUpdate.HoldingItem = new Takeable(avatar.HoldingItem.Weight, avatar.HoldingItem.Description, avatar.HoldingItem.Name)
            {
                Status = (Status)avatar.HoldingItem.Status
            };



            actionToUpdate.Status = (Status)specialActionDto.Status;
            actionToUpdate.MessageRegex = specialActionDto.MessageRegex;
            actionToUpdate.PatternForPlayer = specialActionDto.PatternForPlayer;

            if (GameConfigService.NewOrUpdate(actionToUpdate)) return Ok();

            var oldAction = GameConfigService.Get<Requestable>(specialActionDto.Id);

            var oldActionDto = new RequestableDto
            {
                Status = (int)oldAction.Status,
                Id = oldAction.Id,
                PatternForPlayer = oldAction.PatternForPlayer,
                MessageRegex = oldAction.MessageRegex
            };

            return BadRequest(oldActionDto);
        }*/

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{avatarId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete([FromRoute] Guid avatarId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var avatarToDelete = await GameConfigService.Get<Avatar>(avatarId);

            if (avatarToDelete.Owner != user) return Unauthorized();

            if (avatarToDelete is null) return BadRequest();

            if (await GameConfigService.Delete<Avatar>(avatarId)) return Ok();

            return BadRequest();            // TODO: evtl ändern
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(ICollection<RequestableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var avatars = (await GameConfigService.Get<Dungeon>(dungeonId)).RegisteredAvatars;

            if (!(avatars is null)) return Ok(avatars);

            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/user")]
        [ProducesResponseType(typeof(ICollection<RequestableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllForUser([FromRoute] Guid dungeonId)
        {
            var avatars = (await GameConfigService.Get<Dungeon>(dungeonId)).RegisteredAvatars;

            var userAvatars = avatars.Where(x => x.Owner.Id == "User.Id");

            if (!(userAvatars is null)) return Ok(avatars);

            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{avatarId}")]
        [ProducesResponseType(typeof(AvatarDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid avatarId)
        {
            var avatar = (await GameConfigService.Get<Dungeon>(dungeonId)).RegisteredAvatars.FirstOrDefault(r => r.Id == avatarId);

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
                    (x => !x.GetType().IsSubclassOf(typeof(Takeable))).Select
                    (x => new TakeableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight
                    }).ToList(),
                    InventoryUsableDtos = avatar.Inventory.OfType<Usable>().Select
                    (x => new UsableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        DamageBoost = x.DamageBoost
                    }).ToList(),
                    InventoryConsumableDtos = avatar.Inventory.OfType<Consumable>().Select
                    (x => new ConsumableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        EffectDescription = x.EffectDescription
                    }).ToList(),
                    InventoryWearableDtos = avatar.Inventory.OfType<Wearable>().Select
                    (x => new WearableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
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
                    Inspectables = avatar.CurrentRoom.Inspectables.OfType<Inspectable>().Where(x => !x.GetType().IsSubclassOf(typeof(Inspectable))).Select(x => new InspectableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                    }).ToList(),
                    Takeables = avatar.CurrentRoom.Inspectables.OfType<Takeable>().Where(x => !x.GetType().IsSubclassOf(typeof(Takeable))).Select(x => new TakeableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight
                    }).ToList(),
                    Consumables = avatar.CurrentRoom.Inspectables.OfType<Consumable>().Select(x => new ConsumableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        EffectDescription = x.EffectDescription
                    }).ToList(),
                    Wearables = avatar.CurrentRoom.Inspectables.OfType<Wearable>().Select(x => new WearableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        ProtectionBoost = x.ProtectionBoost
                    }).ToList(),
                    Usables = avatar.CurrentRoom.Inspectables.OfType<Usable>().Select(x => new UsableDto
                    {
                        Id = x.Id,
                        Status = (int)x.Status,
                        Description = x.Description,
                        Name = x.Description,
                        Weight = x.Weight,
                        DamageBoost = x.DamageBoost
                    }).ToList(),
                }
            };
            return Ok(avatarDto);
        }
    }
}


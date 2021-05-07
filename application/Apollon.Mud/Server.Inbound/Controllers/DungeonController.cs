using System;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Microsoft.AspNetCore.Mvc;
using Apollon.Mud.Shared.Dungeon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Shared.Dungeon.Room;
using Apollon.Mud.Shared.Dungeon.User;
using Apollon.Mud.Shared;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/dungeons")]
    [ApiController]
    public class DungeonController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        public DungeonController(IGameDbService gameDbService, IUserService userService)
        {
            GameConfigService = gameDbService;
            UserService = userService;
        }
         
        [HttpPost]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNew([FromBody]DungeonDto dungeonDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var newDungeon = new Dungeon(dungeonDto.DungeonEpoch, dungeonDto.DungeonDescription, dungeonDto.DungeonName)
            {
                Status = (Status) dungeonDto.Status,
                Visibility = (Visibility) dungeonDto.Visibility,
                DungeonOwner = user
            };
            newDungeon.WhiteList.Add(user);
            newDungeon.DungeonMasters.Add(user);

            if (GameConfigService.NewOrUpdate(newDungeon)) return Ok(newDungeon.Id);

            return BadRequest();        // TODO: evtl ändern
        }

        [HttpPut]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DungeonDto), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody]DungeonDto dungeonDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var dungeonToUpdate = GameConfigService.Get<Dungeon>(dungeonDto.Id);

            if (dungeonToUpdate is null) return BadRequest();

            if (dungeonToUpdate.DungeonMasters.All(x => x.Id != user.Id)) return Unauthorized();

            var newDungeonOwner = await UserService.GetUser(dungeonDto.DungeonOwner.Id);
            if (dungeonToUpdate.DungeonOwner.Id != newDungeonOwner.Id && dungeonToUpdate.DungeonOwner.Id != user.Id)
                return Unauthorized();

            dungeonToUpdate.Status = (Status) dungeonDto.Status;
            dungeonToUpdate.Visibility = (Visibility) dungeonDto.Visibility;
            dungeonToUpdate.DungeonName = dungeonDto.DungeonName;
            dungeonToUpdate.DungeonDescription = dungeonDto.DungeonDescription;
            dungeonToUpdate.DefaultRoom = GameConfigService.Get<Room>(dungeonDto.DefaultRoom.Id);
            dungeonToUpdate.DungeonEpoch = dungeonDto.DungeonEpoch;
            dungeonToUpdate.DungeonOwner = newDungeonOwner;

            var dungeonMasterTasks =
                dungeonDto.DungeonMasters.Select(async x => await UserService.GetUser(x.Id));
            var dungeonMasters = await Task.WhenAll(dungeonMasterTasks);
            dungeonToUpdate.DungeonMasters.Clear();
            foreach (var dungeonMaster in dungeonMasters)
            {
                dungeonToUpdate.DungeonMasters.Add(dungeonMaster);
            }

            var whiteListTasks = dungeonDto.WhiteList.Select(async x => await UserService.GetUser(x.Id));
            var dungeonWhiteList = await Task.WhenAll(whiteListTasks);
            dungeonToUpdate.WhiteList.Clear();
            foreach (var dungeonUser in dungeonWhiteList)
            {
                dungeonToUpdate.WhiteList.Add(dungeonUser);
            }

            var blackListTasks = dungeonDto.BlackList.Select(async x => await UserService.GetUser(x.Id));
            var dungeonBlackList = await Task.WhenAll(blackListTasks);
            dungeonToUpdate.BlackList.Clear();
            foreach (var dungeonUser in dungeonBlackList)
            {
                dungeonToUpdate.BlackList.Add(dungeonUser);
            }

            if (GameConfigService.NewOrUpdate(dungeonToUpdate)) return Ok();

            var oldDungeon = GameConfigService.Get<Dungeon>(dungeonDto.Id);

            var oldDungeonDto = new DungeonDto
            {
                Status = (int) oldDungeon.Status,
                Id = oldDungeon.Id,
                Visibility = (int) oldDungeon.Visibility,
                DungeonName = oldDungeon.DungeonName,
                DungeonDescription = oldDungeon.DungeonDescription,
                DungeonEpoch = oldDungeon.DungeonEpoch,
                DefaultRoom = new RoomDto
                {
                    Id = oldDungeon.DefaultRoom.Id,
                    Name = oldDungeon.DefaultRoom.Name
                },
                DungeonOwner = new DungeonUserDto
                {
                    Id = Guid.Parse(oldDungeon.DungeonOwner.Id),
                    Email = oldDungeon.DungeonOwner.Email
                },
                DungeonMasters = oldDungeon.DungeonMasters.Select(x => new DungeonUserDto
                {
                    Id = Guid.Parse(x.Id),
                    Email = x.Email
                }).ToList(),
                BlackList = oldDungeon.BlackList.Select(x => new DungeonUserDto
                {
                    Id = Guid.Parse(x.Id),
                    Email = x.Email
                }).ToList(),
                WhiteList = oldDungeon.WhiteList.Select(x => new DungeonUserDto
                {
                    Id = Guid.Parse(x.Id),
                    Email = x.Email
                }).ToList(),
                OpenRequests = oldDungeon.OpenRequests.Select(x => new DungeonUserDto
                {
                    Id = Guid.Parse(x.Id),
                    Email = x.Email
                }).ToList()
            };

            return BadRequest(oldDungeonDto);       // TODO: evtl ändern
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId)
        {
            var dungeonToDelete = GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeonToDelete is null) return BadRequest();

            if (dungeonToDelete.Status is Status.Approved) return Forbid();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (dungeonToDelete.DungeonOwner.Id != user.Id) return Unauthorized();

            if (GameConfigService.Delete<Dungeon>(dungeonId)) return Ok();

            return BadRequest();            // TODO: evtl ändern
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(typeof(DungeonDto[]), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var dungeons = GameConfigService.GetAll<Dungeon>();

            var dungeonDtos = dungeons.Select(x => new DungeonDto
            {
                Id = x.Id,
                DungeonName = x.DungeonName,
                DungeonDescription = x.DungeonDescription,
                DungeonEpoch = x.DungeonEpoch,
                Visibility = (int) x.Visibility,
                Status = (int) x.Status,
                DungeonOwner = new DungeonUserDto
                {
                    Email = x.DungeonOwner.Email,
                    Id = Guid.Parse(x.DungeonOwner.Id)
                }
            }).ToArray();

            return Ok(dungeonDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(DungeonDto), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId)
        {
            var dungeon = GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var dungeonDto = new DungeonDto
            {
                Id = dungeon.Id,
                DungeonName = dungeon.DungeonName,
                DungeonDescription = dungeon.DungeonDescription,
                DungeonEpoch = dungeon.DungeonEpoch,
                Visibility = (int)dungeon.Visibility,
                Status = (int)dungeon.Status,
                DungeonOwner = new DungeonUserDto
                {
                    Email = dungeon.DungeonOwner.Email,
                    Id = Guid.Parse(dungeon.DungeonOwner.Id)
                }
            };

            return Ok(dungeonDto);
        }

        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/request")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> OpenDungeonEnterRequest([FromRoute] Guid dungeonId)
        {
            var dungeon = GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (dungeon.BlackList.Any(x => x.Id == user.Id)) return Forbid();

            if (dungeon.WhiteList.Any(x => x.Id == user.Id)) return Conflict();

            if (dungeon.OpenRequests.Any(x => x.Id == user.Id)) return Conflict();

            if (dungeon.Visibility == Visibility.Private)
            {
                dungeon.OpenRequests.Add(user);
            }
            else
            {
                dungeon.WhiteList.Add(user);
            }

            GameConfigService.NewOrUpdate(dungeon);
            
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/submitRequest")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SubmitDungeonEnterRequest([FromRoute] Guid dungeonId, [FromBody] SubmitDungeonEnterRequestDto submitDungeonEnterRequestDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var dungeon = GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon.DungeonMasters.All(x => x.Id != user.Id)) return Unauthorized();

            var requestingUser = await UserService.GetUser(submitDungeonEnterRequestDto.RequestUserId);

            if (requestingUser is null) return BadRequest();

            if (submitDungeonEnterRequestDto.GrantAccess)
            {
                if (dungeon.WhiteList.All(x => x.Id != requestingUser.Id))
                {
                    dungeon.WhiteList.Add(requestingUser);
                }
            }
            else
            {
                if (dungeon.BlackList.All(x => x.Id != requestingUser.Id))
                {
                    dungeon.BlackList.Add(requestingUser);
                }
            }

            return Ok();
        }
    }
}

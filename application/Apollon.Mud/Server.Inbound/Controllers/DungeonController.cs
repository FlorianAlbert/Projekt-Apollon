using System;
using System.Collections.Generic;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Microsoft.AspNetCore.Mvc;
using Apollon.Mud.Shared.Dungeon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Game;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Shared.Dungeon.Room;
using Apollon.Mud.Shared.Dungeon.User;
using Apollon.Mud.Shared;
using Apollon.Mud.Shared.Implementations.Dungeons;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    /// <summary>
    /// Controller to configure a dungeon
    /// </summary>
    [Route("api/dungeons")]
    [ApiController]
    public class DungeonController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        private IMasterService MasterService { get; }

        /// <summary>
        /// Creates a new instance of DungeonController
        /// </summary>
        /// <param name="gameDbService">The GameDbService to communicate with the database</param>
        /// <param name="userService">The UserService to get user informations</param>
        public DungeonController(IGameDbService gameDbService, IUserService userService, IMasterService masterService)
        {
            GameConfigService = gameDbService;
            UserService = userService;
            MasterService = masterService;
        }

        /// <summary>
        /// Endpoint to create a new Dungeon
        /// </summary>
        /// <param name="dungeonDto">The DungeonDto containing the informations of the new dungeon</param>
        /// <returns>The id of the new dungeon</returns>
        [HttpPost]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateNew([FromBody] DungeonDto dungeonDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (dungeonDto is null) return BadRequest();

            if ((await GameConfigService.GetAll<Dungeon>()).Any(x => x.DungeonName == dungeonDto.DungeonName))
                return Conflict();

            var newDungeon = new Dungeon(dungeonDto.DungeonEpoch, dungeonDto.DungeonDescription, dungeonDto.DungeonName)
            {
                Status = (Status)dungeonDto.Status,
                Visibility = (Visibility)dungeonDto.Visibility,
                DungeonOwner = user,
                LastActive = DateTime.UtcNow
            };
            newDungeon.WhiteList.Add(user);
            newDungeon.DungeonMasters.Add(user);

            if (await GameConfigService.NewOrUpdate(newDungeon)) return Ok(newDungeon.Id);

            return BadRequest();        // TODO: evtl ändern
        }

        /// <summary>
        /// Endpoint to update a dungeon
        /// </summary>
        /// <param name="dungeonDto">The DungeonDto containing the informations of the updated dungeon</param>
        /// <returns>The HTTP Status Result</returns>
        [HttpPut]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DungeonDto), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] DungeonDto dungeonDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (dungeonDto is null) return BadRequest();

            var dungeonToUpdate = await GameConfigService.Get<Dungeon>(dungeonDto.Id);

            if (dungeonToUpdate is null) return BadRequest();

            if (!dungeonToUpdate.DungeonMasters.Contains(user) && !await UserService.IsUserInAdminRole(user.Id)) return Unauthorized();

            var newDungeonOwner = await UserService.GetUser(dungeonDto.DungeonOwner.Id);

            if (newDungeonOwner is null) return BadRequest();

            if (dungeonToUpdate.DungeonOwner.Id != newDungeonOwner.Id && dungeonToUpdate.DungeonOwner.Id != user.Id)
                return Unauthorized();

            Room newDefaultRoom;
            if (dungeonDto.DefaultRoom is not null)
            {
                newDefaultRoom = await GameConfigService.Get<Room>(dungeonDto.DefaultRoom.Id);//dungeonToUpdate.ConfiguredRooms.FirstOrDefault(x => x.Id == dungeonDto.DefaultRoom.Id);

                if (newDefaultRoom is null) return BadRequest();
            }
            else
            {
                newDefaultRoom = null;
            }

            dungeonToUpdate.Status = (Status)dungeonDto.Status;                        // TODO: Wenn auf Pending wechselt, alle Avatare kicken --- wird clientseitig gehandlet
            dungeonToUpdate.Visibility = (Visibility)dungeonDto.Visibility;
            dungeonToUpdate.DungeonName = dungeonDto.DungeonName;
            dungeonToUpdate.DungeonDescription = dungeonDto.DungeonDescription;
            dungeonToUpdate.DefaultRoom = newDefaultRoom;
            dungeonToUpdate.DungeonEpoch = dungeonDto.DungeonEpoch;
            dungeonToUpdate.DungeonOwner = newDungeonOwner;

            dungeonToUpdate.DungeonMasters.Clear();
            dungeonToUpdate.WhiteList.Clear();
            dungeonToUpdate.BlackList.Clear();

            if (!await GameConfigService.NewOrUpdate(dungeonToUpdate)) return BadRequest();

            dungeonDto.DungeonMasters ??= new List<DungeonUserDto>
            {
                new()
                {
                    Id = userId
                }
            };

            var dungeonMasterTasks =
                    dungeonDto.DungeonMasters.Select(async x => await UserService.GetUser(x.Id));
            var dungeonMasters = await Task.WhenAll(dungeonMasterTasks);
            foreach (var dungeonMaster in dungeonMasters)
            {
                if (dungeonMaster is not null) dungeonToUpdate.DungeonMasters.Add(dungeonMaster);
            }
            dungeonToUpdate.DungeonMasters.Add(dungeonToUpdate.DungeonOwner);


            dungeonDto.WhiteList ??= new List<DungeonUserDto>
            {
                new()
                {
                    Id = userId
                }
            };

            var whiteListTasks = dungeonDto.WhiteList.Select(async x => await UserService.GetUser(x.Id));
            var dungeonWhiteList = await Task.WhenAll(whiteListTasks);
            foreach (var dungeonUser in dungeonWhiteList)
            {
                if (dungeonUser is not null) dungeonToUpdate.WhiteList.Add(dungeonUser);
            }
            dungeonToUpdate.WhiteList.Add(dungeonToUpdate.DungeonOwner);


            if (dungeonDto.BlackList is not null)
            {
                var blackListTasks = dungeonDto.BlackList.Select(async x => await UserService.GetUser(x.Id));
                var dungeonBlackList = await Task.WhenAll(blackListTasks);
                foreach (var dungeonUser in dungeonBlackList)           //TODO: alle Avatare des users im dungeon löschen?
                {
                    if (dungeonUser is null || dungeonUser == dungeonToUpdate.DungeonOwner) continue;

                    dungeonToUpdate.BlackList.Add(dungeonUser);
                    var userAvatar = (await GameConfigService.GetAll<Avatar>()).
                        Where(a => a.Owner == dungeonUser && a.Status == Status.Approved);
                    foreach (var avatar in userAvatar)
                    {
                        await MasterService.KickAvatar(avatar.Id, avatar.Dungeon.Id);
                    }
                }
            }

            if (await GameConfigService.NewOrUpdate(dungeonToUpdate)) return Ok();

            var oldDungeon = await GameConfigService.Get<Dungeon>(dungeonDto.Id);

            var oldDungeonDto = new DungeonDto
            {
                Status = (int)oldDungeon.Status,
                Id = oldDungeon.Id,
                Visibility = (int)oldDungeon.Visibility,
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

        /// <summary>
        /// Endpoint to delete a dungeon
        /// </summary>
        /// <param name="dungeonId">The Id of the dungeon to delete</param>
        /// <returns>The HTTP Status Result</returns>
        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete([FromRoute] Guid dungeonId)
        {
            var dungeonToDelete = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeonToDelete is null) return BadRequest();

            if (dungeonToDelete.Status is Status.Approved) return Forbid();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (dungeonToDelete.DungeonOwner != user) return Unauthorized();

            if (await GameConfigService.Delete<Dungeon>(dungeonId)) return Ok();

            return BadRequest();            // TODO: evtl ändern
        }

        /// <summary>
        /// Endpoint to get all existing dungeons
        /// </summary>
        /// <returns>A list of DungeonDtos containing informations about all existing dungeons</returns>
        [HttpGet]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(typeof(DungeonDto[]), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var dungeons = await GameConfigService.GetAll<Dungeon>();

            var dungeonDtos = dungeons.Select(x =>
            {
                RoomDto defaultRoom;
                if (x.DefaultRoom is null) defaultRoom = null;
                else
                    defaultRoom = new RoomDto
                    {
                        Id = x.DefaultRoom.Id,
                        Name = x.DefaultRoom.Name,
                        Description = x.DefaultRoom.Description,
                        Status = (int)x.DefaultRoom.Status
                    };

                return new DungeonDto
                {
                    Id = x.Id,
                    DungeonName = x.DungeonName,
                    DungeonDescription = x.DungeonDescription,
                    DungeonEpoch = x.DungeonEpoch,
                    Visibility = (int)x.Visibility,
                    Status = (int)x.Status,
                    BlackList = x.BlackList.Select(u => new DungeonUserDto()
                    {
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        Id = Guid.Parse(u.Id),
                        LastActive = u.LastActive
                    }).ToList(),
                    WhiteList = x.WhiteList.Select(u => new DungeonUserDto()
                    {
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        Id = Guid.Parse(u.Id),
                        LastActive = u.LastActive
                    }).ToList(),
                    DungeonMasters = x.DungeonMasters.Select(u => new DungeonUserDto()
                    {
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        Id = Guid.Parse(u.Id),
                        LastActive = u.LastActive
                    }).ToList(),
                    OpenRequests = x.OpenRequests.Select(u => new DungeonUserDto()
                    {
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        Id = Guid.Parse(u.Id),
                        LastActive = u.LastActive
                    }).ToList(),
                    DungeonOwner = new DungeonUserDto
                    {
                        Email = x.DungeonOwner.Email,
                        Id = Guid.Parse(x.DungeonOwner.Id)
                    },
                    DefaultRoom = defaultRoom,
                    CurrentMaster = x.CurrentDungeonMaster is null ? null : 
                    new DungeonUserDto()
                    {
                        Email = x.CurrentDungeonMaster.Email,
                        EmailConfirmed = x.CurrentDungeonMaster.EmailConfirmed,
                        Id = Guid.Parse(x.CurrentDungeonMaster.Id),
                        LastActive = x.CurrentDungeonMaster.LastActive
                    },
                    LastActive = x.LastActive
                };
            }).ToArray();

            return Ok(dungeonDtos);
        }

        /// <summary>
        /// Endoint to get all dungeons for one user
        /// </summary>
        /// <returns>A list of DungeonDtos containing informations about all existing dungeons that belong to the requesting user</returns>
        [HttpGet]
        [Route("userdungeons")]
        [Authorize(Roles = "Player")]
        [ProducesResponseType(typeof(DungeonDto[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllForUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            var dungeons = (await GameConfigService.GetAll<Dungeon>()).Where(x => x.DungeonOwner.Id == user.Id);

            var dungeonDtos = dungeons.Select(x =>
            {
                RoomDto defaultRoom;
                if (x.DefaultRoom is null) defaultRoom = null;
                else
                    defaultRoom = new RoomDto
                    {
                        Id = x.DefaultRoom.Id,
                        Name = x.DefaultRoom.Name,
                        Description = x.DefaultRoom.Description,
                        Status = (int)x.DefaultRoom.Status
                    };

                return new DungeonDto
                {
                    Id = x.Id,
                    DungeonName = x.DungeonName,
                    DungeonDescription = x.DungeonDescription,
                    DungeonEpoch = x.DungeonEpoch,
                    Visibility = (int) x.Visibility,
                    Status = (int) x.Status,
                    BlackList = x.BlackList.Select(u => new DungeonUserDto()
                    {
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        Id = Guid.Parse(u.Id),
                        LastActive = u.LastActive
                    }).ToList(),
                    WhiteList = x.WhiteList.Select(u => new DungeonUserDto()
                    {
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        Id = Guid.Parse(u.Id),
                        LastActive = u.LastActive
                    }).ToList(),
                    DungeonMasters = x.DungeonMasters.Select(u => new DungeonUserDto()
                    {
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        Id = Guid.Parse(u.Id),
                        LastActive = u.LastActive
                    }).ToList(),
                    OpenRequests = x.OpenRequests.Select(u => new DungeonUserDto()
                    {
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        Id = Guid.Parse(u.Id),
                        LastActive = u.LastActive
                    }).ToList(),
                    DungeonOwner = new DungeonUserDto
                    {
                        Email = x.DungeonOwner.Email,
                        Id = Guid.Parse(x.DungeonOwner.Id)
                    },
                    DefaultRoom = defaultRoom,
                    LastActive = x.LastActive,
                    CurrentMaster = x.CurrentDungeonMaster is null ? null : new DungeonUserDto()
                    {
                        Email = x.CurrentDungeonMaster.Email,
                        EmailConfirmed = x.CurrentDungeonMaster.EmailConfirmed,
                        Id = Guid.Parse(x.CurrentDungeonMaster.Id),
                        LastActive = x.CurrentDungeonMaster.LastActive
                    }
                };
            }).ToArray();

            return Ok(dungeonDtos);
        }

        /// <summary>
        /// Endpoint to get a specific dungeon
        /// </summary>
        /// <param name="dungeonId">The Id of the requested dungeon</param>
        /// <returns>The DungeonDto containing informations about the requested dungeon</returns>
        [HttpGet]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(DungeonDto), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            RoomDto defaultRoom;
            if (dungeon.DefaultRoom is null) defaultRoom = null;
            else
                defaultRoom = new RoomDto
                {
                    Id = dungeon.DefaultRoom.Id,
                    Name = dungeon.DefaultRoom.Name,
                    Description = dungeon.DefaultRoom.Description,
                    Status = (int)dungeon.DefaultRoom.Status
                };

            var dungeonDto = new DungeonDto
            {
                Id = dungeon.Id,
                DungeonName = dungeon.DungeonName,
                DungeonDescription = dungeon.DungeonDescription,
                DungeonEpoch = dungeon.DungeonEpoch,
                Visibility = (int)dungeon.Visibility,
                Status = (int)dungeon.Status,
                BlackList = dungeon.BlackList.Select(u => new DungeonUserDto()
                {
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    Id = Guid.Parse(u.Id),
                    LastActive = u.LastActive
                }).ToList(),
                WhiteList = dungeon.WhiteList.Select(u => new DungeonUserDto()
                {
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    Id = Guid.Parse(u.Id),
                    LastActive = u.LastActive
                }).ToList(),
                DungeonMasters = dungeon.DungeonMasters.Select(u => new DungeonUserDto()
                {
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    Id = Guid.Parse(u.Id),
                    LastActive = u.LastActive
                }).ToList(),
                OpenRequests = dungeon.OpenRequests.Select(u => new DungeonUserDto()
                {
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    Id = Guid.Parse(u.Id),
                    LastActive = u.LastActive
                }).ToList(),
                DungeonOwner = new DungeonUserDto
                {
                    Email = dungeon.DungeonOwner.Email,
                    Id = Guid.Parse(dungeon.DungeonOwner.Id)
                },
                DefaultRoom = defaultRoom,
                LastActive = dungeon.LastActive,
                CurrentMaster = dungeon.CurrentDungeonMaster is null ? null : new DungeonUserDto()
                {
                    Email = dungeon.CurrentDungeonMaster.Email,
                    EmailConfirmed = dungeon.CurrentDungeonMaster.EmailConfirmed,
                    Id = Guid.Parse(dungeon.CurrentDungeonMaster.Id),
                    LastActive = dungeon.CurrentDungeonMaster.LastActive
                }
            };

            return Ok(dungeonDto);
        }

        /// <summary>
        /// Endpoint to request access to a dungeon
        /// </summary>
        /// <param name="dungeonId">Id of the dungeon the access gets requested for</param>
        /// <returns>The HTTP Status Result</returns>
        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/request")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> OpenDungeonEnterRequest([FromRoute] Guid dungeonId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (dungeon.BlackList.Contains(user)) return Forbid();

            if (dungeon.WhiteList.Contains(user)) return Conflict();

            if (dungeon.OpenRequests.Contains(user)) return Conflict();

            if (dungeon.Visibility == Visibility.Private)
            {
                dungeon.OpenRequests.Add(user);
            }
            else
            {
                dungeon.WhiteList.Add(user);
            }

            if (await GameConfigService.NewOrUpdate(dungeon)) return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Endpoint to submit an open request for accessing a dungeon
        /// </summary>
        /// <param name="dungeonId">Id of the dungeon the request gets submitted for</param>
        /// <param name="submitDungeonEnterRequestDto">The details for the submittal</param>
        /// <returns>The HTTP Status Result</returns>
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

            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            if (submitDungeonEnterRequestDto is null) return BadRequest();

            var requestingUser = await UserService.GetUser(submitDungeonEnterRequestDto.RequestUserId);

            if (requestingUser is null) return BadRequest();

            if (submitDungeonEnterRequestDto.GrantAccess)
            {
                if (!dungeon.WhiteList.Contains(requestingUser))
                {
                    dungeon.WhiteList.Add(requestingUser);
                }
            }
            else
            {
                if (!dungeon.BlackList.Contains(requestingUser))
                {
                    dungeon.BlackList.Add(requestingUser);
                }
            }

            return Ok();
        }
    }
}

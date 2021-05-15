using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Game;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Server.Model.ModelExtensions;
using Apollon.Mud.Shared.Dungeon.Inspectable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Shared.Dungeon.Npc;
using Apollon.Mud.Shared.Dungeon.Requestable;
using Apollon.Mud.Shared.Dungeon.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Shared.Implementations.Dungeons;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private IUserService UserService { get; }

        private ILogger<RoomController> _logger;

        private IMasterService MasterService { get; }

        public RoomController(ILogger<RoomController> logger, IGameDbService gameDbService, IUserService userService, IMasterService masterService)
        {
            _logger = logger;
            GameConfigService = gameDbService;
            UserService = userService;
            MasterService = masterService;
        }

        [HttpGet]
        [Authorize(Roles = "Player, Admin")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(RoomDto[]), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromRoute] Guid dungeonId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var roomDtos = dungeon.ConfiguredRooms.Select(x => new RoomDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                NeighborEastId = x.NeighborEast?.Id ?? Guid.Empty,
                NeighborSouthId = x.NeighborSouth?.Id ?? Guid.Empty,
                NeighborWestId = x.NeighborWest?.Id ?? Guid.Empty,
                NeighborNorthId = x.NeighborNorth?.Id ?? Guid.Empty,
                Status = (int)x.Status,
                Consumables = x.Inspectables.Where(c => c.Inspectable.GetType() == typeof(Consumable))
                    .Select(c => new ConsumableDto
                    {
                        Id = c.Inspectable.Id,
                        Name = c.Inspectable.Name,
                        Description = c.Inspectable.Description,
                        Weight = (c.Inspectable as Consumable).Weight,
                        EffectDescription = (c.Inspectable as Consumable).EffectDescription,
                        Status = (int)(c.Inspectable as Consumable).Status
                    }).ToList(),
                Usables = x.Inspectables.Where(u => u.Inspectable.GetType() == typeof(Usable))
                    .Select(u => new UsableDto
                    {
                        Id = u.Inspectable.Id,
                        Name = u.Inspectable.Name,
                        Description = u.Inspectable.Description,
                        Weight = (u.Inspectable as Usable).Weight,
                        Status = (int)(u.Inspectable as Usable).Status,
                        DamageBoost = (u.Inspectable as Usable).DamageBoost
                    }).ToList(),
                Takeables = x.Inspectables.Where(t => t.Inspectable.GetType() == typeof(Takeable))
                    .Where(t => t.Inspectable is not Consumable and not Wearable and not Usable)
                    .Select(t => new TakeableDto
                    {
                        Id = t.Inspectable.Id,
                        Name = t.Inspectable.Name,
                        Description = t.Inspectable.Description,
                        Weight = (t.Inspectable as Takeable).Weight,
                        Status = (int)(t.Inspectable as Takeable).Status
                    }).ToList(),
                Wearables = x.Inspectables.Where(w => w.Inspectable.GetType() == typeof(Wearable))
                    .Select(w => new WearableDto
                    {
                        Id = w.Inspectable.Id,
                        Name = w.Inspectable.Name,
                        Description = w.Inspectable.Description,
                        Weight = (w.Inspectable as Wearable).Weight,
                        Status = (int)(w.Inspectable as Wearable).Status,
                        ProtectionBoost = (w.Inspectable as Wearable).ProtectionBoost
                    }).ToList(),
                Npcs = x.Inspectables.Where(n => n.Inspectable.GetType() == typeof(Npc))
                    .Select(n => new NpcDto
                    {
                        Id = n.Inspectable.Id,
                        Name = n.Inspectable.Name,
                        Description = n.Inspectable.Description,
                        Status = (int)n.Inspectable.Status,
                        Text = (n.Inspectable as Npc).Text
                    }).ToList(),
                Inspectables = x.Inspectables
                    .Where(i => i.Inspectable is not Takeable and not Npc)
                    .Select(i => new InspectableDto
                    {
                        Id = i.Inspectable.Id,
                        Name = i.Inspectable.Name,
                        Description = i.Inspectable.Description,
                        Status = (int)i.Inspectable.Status
                    }).ToList(),
                SpecialActions = x.SpecialActions.Select(s => new RequestableDto
                {
                    Id = s.Id,
                    MessageRegex = s.MessageRegex,
                    PatternForPlayer = s.PatternForPlayer,
                    Status = (int)s.Status
                }).ToList()
            }).ToArray();

            return Ok(roomDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Player, Admin")]
        [Route("{dungeonId}/{roomId}")]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid roomId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var room = dungeon.ConfiguredRooms.FirstOrDefault(x => x.Id == roomId);

            if (room is null) return BadRequest();

            var roomDto = new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                NeighborEastId = room.NeighborEast?.Id ?? Guid.Empty,
                NeighborSouthId = room.NeighborSouth?.Id ?? Guid.Empty,
                NeighborWestId = room.NeighborWest?.Id ?? Guid.Empty,
                NeighborNorthId = room.NeighborNorth?.Id ?? Guid.Empty,
                Status = (int)room.Status,
                Consumables = room.Inspectables.Where(c => c.Inspectable.GetType() == typeof(Consumable))
                    .Select(c => new ConsumableDto
                    {
                        Id = c.Inspectable.Id,
                        Name = c.Inspectable.Name,
                        Description = c.Inspectable.Description,
                        Weight = (c.Inspectable as Consumable).Weight,
                        EffectDescription = (c.Inspectable as Consumable).EffectDescription,
                        Status = (int)(c.Inspectable as Consumable).Status
                    }).ToList(),
                Usables = room.Inspectables.Where(u => u.Inspectable.GetType() == typeof(Usable))
                    .Select(u => new UsableDto
                    {
                        Id = u.Inspectable.Id,
                        Name = u.Inspectable.Name,
                        Description = u.Inspectable.Description,
                        Weight = (u.Inspectable as Usable).Weight,
                        Status = (int)(u.Inspectable as Usable).Status,
                        DamageBoost = (u.Inspectable as Usable).DamageBoost
                    }).ToList(),
                Takeables = room.Inspectables.Where(t => t.Inspectable.GetType() == typeof(Takeable))
                    .Where(t => t.Inspectable is not Consumable and not Wearable and not Usable)
                    .Select(t => new TakeableDto
                    {
                        Id = t.Inspectable.Id,
                        Name = t.Inspectable.Name,
                        Description = t.Inspectable.Description,
                        Weight = (t.Inspectable as Takeable).Weight,
                        Status = (int)(t.Inspectable as Takeable).Status
                    }).ToList(),
                Wearables = room.Inspectables.Where(w => w.Inspectable.GetType() == typeof(Wearable))
                    .Select(w => new WearableDto
                    {
                        Id = w.Inspectable.Id,
                        Name = w.Inspectable.Name,
                        Description = w.Inspectable.Description,
                        Weight = (w.Inspectable as Wearable).Weight,
                        Status = (int)(w.Inspectable as Wearable).Status,
                        ProtectionBoost = (w.Inspectable as Wearable).ProtectionBoost
                    }).ToList(),
                Npcs = room.Inspectables.Where(n => n.Inspectable.GetType() == typeof(Npc))
                    .Select(n => new NpcDto
                    {
                        Id = n.Inspectable.Id,
                        Name = n.Inspectable.Name,
                        Description = n.Inspectable.Description,
                        Status = (int)n.Inspectable.Status,
                        Text = (n.Inspectable as Npc).Text
                    }).ToList(),
                Inspectables = room.Inspectables
                    .Where(i => i.Inspectable is not Takeable and not Npc)
                    .Select(i => new InspectableDto
                    {
                        Id = i.Inspectable.Id,
                        Name = i.Inspectable.Name,
                        Description = i.Inspectable.Description,
                        Status = (int)i.Inspectable.Status
                    }).ToList(),
                SpecialActions = room.SpecialActions.Select(s => new RequestableDto
                {
                    Id = s.Id,
                    MessageRegex = s.MessageRegex,
                    PatternForPlayer = s.PatternForPlayer,
                    Status = (int)s.Status
                }).ToList()
            };

            return Ok(roomDto);
        }

        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}/{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteRoom([FromRoute] Guid dungeonId, [FromRoute] Guid roomId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);
            if (dungeon is null) return BadRequest();

            var room = dungeon.ConfiguredRooms.FirstOrDefault(r => r.Id == roomId);
            if (dungeon.Status is Status.Approved && room.Status is Status.Approved) return Forbid();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);
            if (user is null) return BadRequest();

            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            await MasterService.MoveAvatarsToDefaultRoom(roomId, dungeonId);

            if (await GameConfigService.Delete<Room>(roomId)) return Ok();

            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateNew([FromRoute] Guid dungeonId, [FromBody] RoomDto roomDto)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            Room neighborEast;
            Room neighborSouth;
            Room neighborWest;
            Room neighborNorth;
            try
            {
                neighborEast = roomDto.NeighborEastId == Guid.Empty
                    ? null
                    : dungeon.ConfiguredRooms.SingleOrDefault(x => x.Id == roomDto.NeighborEastId);

                neighborSouth = roomDto.NeighborSouthId == Guid.Empty
                    ? null
                    : dungeon.ConfiguredRooms.SingleOrDefault(x => x.Id == roomDto.NeighborSouthId);

                neighborWest = roomDto.NeighborWestId == Guid.Empty
                    ? null
                    : dungeon.ConfiguredRooms.SingleOrDefault(x => x.Id == roomDto.NeighborWestId);

                neighborNorth = roomDto.NeighborNorthId == Guid.Empty
                    ? null
                    : dungeon.ConfiguredRooms.SingleOrDefault(x => x.Id == roomDto.NeighborNorthId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "There are more than one room with this Id in the dungeon.");
                return Conflict();
            }

            if (neighborNorth?.NeighborSouth is not null ||
                neighborEast?.NeighborWest is not null ||
                neighborSouth?.NeighborNorth is not null ||
                neighborWest?.NeighborEast is not null)
                return Conflict();

            var room = new Room(roomDto.Description, roomDto.Name)
            {
                Status = (Status)roomDto.Status,
                NeighborEast = neighborEast,
                NeighborSouth = neighborSouth,
                NeighborWest = neighborWest,
                NeighborNorth = neighborNorth,
                Dungeon = dungeon
            };

            foreach (var consumableDto in roomDto.Consumables)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == consumableDto.Id) is Consumable consumable) room.Inspectables.Add(consumable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Consumable with the Id {consumableDto.Id} in the dungeon.");
                }
            }

            foreach (var wearableDto in roomDto.Wearables)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == wearableDto.Id) is Wearable wearable) room.Inspectables.Add(wearable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Wearable with the Id {wearableDto.Id} in the dungeon.");
                }
            }

            foreach (var usableDto in roomDto.Usables)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == usableDto.Id) is Usable usable) room.Inspectables.Add(new usable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Usable with the Id {usableDto.Id} in the dungeon.");
                }
            }

            foreach (var takeableDto in roomDto.Takeables)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == takeableDto.Id) is Takeable takeable) room.Inspectables.Add(takeable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Takeable with the Id {takeableDto.Id} in the dungeon.");
                }
            }

            foreach (var npcDto in roomDto.Npcs)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == npcDto.Id) is Npc npc) room.Inspectables.Add(npc);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Npc with the Id {npcDto.Id} in the dungeon.");
                }
            }

            foreach (var inspectableDto in roomDto.Inspectables)
            {
                try
                {
                    var inspectable = dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == inspectableDto.Id);

                    if (inspectable is not null) room.Inspectables.Add(inspectable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Inspectable with the Id {inspectableDto.Id} in the dungeon.");
                }
            }

            foreach (var requestableDto in roomDto.SpecialActions)
            {
                try
                {
                    var requestable = dungeon.ConfiguredRequestables.SingleOrDefault(r => r.Id == requestableDto.Id);

                    if (requestable is not null) room.SpecialActions.Add(requestable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Requestable with the Id {requestableDto.Id} in the dungeon.");
                }
            }

            if (await GameConfigService.NewOrUpdate(room)) return Ok(room.Id);

            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromRoute] Guid dungeonId, [FromBody] RoomDto roomDto)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (!dungeon.DungeonMasters.Contains(user)) return Unauthorized();

            var room = dungeon.ConfiguredRooms.FirstOrDefault(x => x.Id == roomDto.Id);

            if (room is null) return BadRequest();
            if (dungeon.DefaultRoom == room && (Status) roomDto.Status is Status.Pending) return BadRequest(); //ToDo klären ob Conflict nicht besser wäre.

            Room neighborEast;
            Room neighborSouth;
            Room neighborWest;
            Room neighborNorth;
            try
            {
                neighborEast = roomDto.NeighborEastId == Guid.Empty
                    ? null
                    : dungeon.ConfiguredRooms.SingleOrDefault(x => x.Id == roomDto.NeighborEastId);

                neighborSouth = roomDto.NeighborSouthId == Guid.Empty
                    ? null
                    : dungeon.ConfiguredRooms.SingleOrDefault(x => x.Id == roomDto.NeighborSouthId);

                neighborWest = roomDto.NeighborWestId == Guid.Empty
                    ? null
                    : dungeon.ConfiguredRooms.SingleOrDefault(x => x.Id == roomDto.NeighborWestId);

                neighborNorth = roomDto.NeighborNorthId == Guid.Empty
                    ? null
                    : dungeon.ConfiguredRooms.SingleOrDefault(x => x.Id == roomDto.NeighborNorthId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "There are more than one room with this Id in the dungeon.");
                return Conflict();
            }

            if (neighborNorth?.NeighborSouth is not null && neighborNorth.NeighborSouth != room ||
                neighborEast?.NeighborWest is not null && neighborEast.NeighborWest != room ||
                neighborSouth?.NeighborNorth is not null && neighborSouth.NeighborNorth != room ||
                neighborWest?.NeighborEast is not null && neighborWest.NeighborEast != room)
                return Conflict();

            var moveAvatarsToDefault = room.Status == Status.Approved && (Status) roomDto.Status == Status.Pending;

            room.Description = roomDto.Description;
            room.Name = roomDto.Name;
            room.Status = (Status)roomDto.Status;
            room.NeighborEast = neighborEast;
            room.NeighborSouth = neighborSouth;
            room.NeighborWest = neighborWest;
            room.NeighborNorth = neighborNorth;

            room.Inspectables.Clear();

            if (!await GameConfigService.NewOrUpdate(room)) return BadRequest();

            foreach (var consumableDto in roomDto.Consumables)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == consumableDto.Id) is Consumable consumable) room.Inspectables.Add(consumable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Consumable with the Id {consumableDto.Id} in the dungeon.");
                }
            }

            foreach (var wearableDto in roomDto.Wearables)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == wearableDto.Id) is Wearable wearable) room.Inspectables.Add(wearable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Wearable with the Id {wearableDto.Id} in the dungeon.");
                }
            }

            foreach (var usableDto in roomDto.Usables)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == usableDto.Id) is Usable usable) room.Inspectables.Add(usable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Usable with the Id {usableDto.Id} in the dungeon.");
                }
            }

            foreach (var takeableDto in roomDto.Takeables)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == takeableDto.Id) is Takeable takeable) room.Inspectables.Add(takeable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Takeable with the Id {takeableDto.Id} in the dungeon.");
                }
            }

            foreach (var npcDto in roomDto.Npcs)
            {
                try
                {
                    if (dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == npcDto.Id) is Npc npc) room.Inspectables.Add(npc);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Npc with the Id {npcDto.Id} in the dungeon.");
                }
            }

            foreach (var inspectableDto in roomDto.Inspectables)
            {
                try
                {
                    var inspectable = dungeon.ConfiguredInspectables.SingleOrDefault(c => c.Id == inspectableDto.Id);

                    if (inspectable is not null) room.Inspectables.Add(inspectable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Inspectable with the Id {inspectableDto.Id} in the dungeon.");
                }
            }

            room.SpecialActions.RemoveAll();

            foreach (var requestableDto in roomDto.SpecialActions)
            {
                try
                {
                    var requestable = dungeon.ConfiguredRequestables.SingleOrDefault(r => r.Id == requestableDto.Id);

                    if (requestable is not null) room.SpecialActions.Add(requestable);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogError(ex, $"There are more than one Requestable with the Id {requestableDto.Id} in the dungeon.");
                }
            }

            if (moveAvatarsToDefault) await MasterService.MoveAvatarsToDefaultRoom(room.Id, dungeonId);
            if (await GameConfigService.NewOrUpdate(room))return Ok();


            return BadRequest();
        }

        
    }
}

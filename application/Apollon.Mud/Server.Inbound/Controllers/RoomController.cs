using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
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

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        private UserService UserService { get; }

        private ILogger<RoomController> _logger;

        public RoomController(ILogger<RoomController> logger, IGameDbService gameDbService, UserService userService)
        {
            _logger = logger;
            GameConfigService = gameDbService;
            UserService = userService;
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

            var rooms = dungeon.ConfiguredRooms;

            var roomDtos = rooms.Select(x => new RoomDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                NeighborEastId = x.NeighborEast.Id,
                NeighborSouthId = x.NeighborSouth.Id,
                NeighborWestId = x.NeighborWest.Id,
                NeighborNorthId = x.NeighborNorth.Id,
                Status = (int)x.Status,
                Consumables = x.Inspectables.OfType<Consumable>()
                    .Where(c => !c.GetType().IsSubclassOf(typeof(Consumable)))
                    .Select(c => new ConsumableDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Weight = c.Weight,
                        EffectDescription = c.EffectDescription,
                        Status = (int)c.Status
                    }).ToList(),
                Usables = x.Inspectables.OfType<Usable>()
                    .Where(u => !u.GetType().IsSubclassOf(typeof(Usable)))
                    .Select(u => new UsableDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Description = u.Description,
                        Weight = u.Weight,
                        Status = (int)u.Status,
                        DamageBoost = u.DamageBoost
                    }).ToList(),
                Takeables = x.Inspectables.OfType<Takeable>()
                    .Where(t => !t.GetType().IsSubclassOf(typeof(Takeable)))
                    .Select(t => new TakeableDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        Weight = t.Weight,
                        Status = (int)t.Status
                    }).ToList(),
                Wearables = x.Inspectables.OfType<Wearable>()
                    .Where(w => !w.GetType().IsSubclassOf(typeof(Wearable)))
                    .Select(w => new WearableDto
                    {
                        Id = w.Id,
                        Name = w.Name,
                        Description = w.Description,
                        Weight = w.Weight,
                        Status = (int)w.Status,
                        ProtectionBoost = w.ProtectionBoost
                    }).ToList(),
                Npcs = x.Inspectables.OfType<Npc>()
                    .Where(n => !n.GetType().IsSubclassOf(typeof(Npc)))
                    .Select(n => new NpcDto
                    {
                        Id = n.Id,
                        Name = n.Name,
                        Description = n.Description,
                        Status = (int)n.Status,
                        Text = n.Text
                    }).ToList(),
                Inspectables = x.Inspectables
                    .Where(i => !i.GetType().IsSubclassOf(typeof(Inspectable)))
                    .Select(i => new InspectableDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Description = i.Description,
                        Status = (int)i.Status
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
        [ProducesResponseType(typeof(RoomDto[]), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Get([FromRoute] Guid dungeonId, [FromRoute] Guid roomId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            Room room;
            try
            {
                room = dungeon.ConfiguredRooms.SingleOrDefault(x => x.Id == roomId);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "There are more than one room with this Id in the dungeon.");
                return Conflict();
            }

            if (room is null) return BadRequest();

            var roomDto = new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                NeighborEastId = room.NeighborEast.Id,
                NeighborSouthId = room.NeighborSouth.Id,
                NeighborWestId = room.NeighborWest.Id,
                NeighborNorthId = room.NeighborNorth.Id,
                Status = (int)room.Status,
                Consumables = room.Inspectables.OfType<Consumable>()
                    .Where(c => !c.GetType().IsSubclassOf(typeof(Consumable)))
                    .Select(c => new ConsumableDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Weight = c.Weight,
                        EffectDescription = c.EffectDescription,
                        Status = (int)c.Status
                    }).ToList(),
                Usables = room.Inspectables.OfType<Usable>()
                    .Where(u => !u.GetType().IsSubclassOf(typeof(Usable)))
                    .Select(u => new UsableDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Description = u.Description,
                        Weight = u.Weight,
                        Status = (int)u.Status,
                        DamageBoost = u.DamageBoost
                    }).ToList(),
                Takeables = room.Inspectables.OfType<Takeable>()
                    .Where(t => !t.GetType().IsSubclassOf(typeof(Takeable)))
                    .Select(t => new TakeableDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        Weight = t.Weight,
                        Status = (int)t.Status
                    }).ToList(),
                Wearables = room.Inspectables.OfType<Wearable>()
                    .Where(w => !w.GetType().IsSubclassOf(typeof(Wearable)))
                    .Select(w => new WearableDto
                    {
                        Id = w.Id,
                        Name = w.Name,
                        Description = w.Description,
                        Weight = w.Weight,
                        Status = (int)w.Status,
                        ProtectionBoost = w.ProtectionBoost
                    }).ToList(),
                Npcs = room.Inspectables.OfType<Npc>()
                    .Where(n => !n.GetType().IsSubclassOf(typeof(Npc)))
                    .Select(n => new NpcDto
                    {
                        Id = n.Id,
                        Name = n.Name,
                        Description = n.Description,
                        Status = (int)n.Status,
                        Text = n.Text
                    }).ToList(),
                Inspectables = room.Inspectables
                    .Where(i => !i.GetType().IsSubclassOf(typeof(Inspectable)))
                    .Select(i => new InspectableDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Description = i.Description,
                        Status = (int)i.Status
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
        public async Task<IActionResult> DeleteRoom([FromRoute] Guid dungeonId, [FromRoute] Guid roomId)
        {
            var dungeon = await GameConfigService.Get<Dungeon>(dungeonId);

            if (dungeon is null) return BadRequest();

            if (dungeon.Status is Status.Approved) return Forbid();

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await UserService.GetUser(userId);

            if (user is null) return BadRequest();

            if (dungeon.DungeonMasters.All(x => x.Id != user.Id)) return Unauthorized();

            GameConfigService.Delete<Room>(roomId);     // TODO: Nicht fertig!

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("{dungeonId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]           // TODO: Nachbarschaften checken ob korrekt?
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

            if (dungeon.DungeonMasters.All(x => x.Id != user.Id)) return Unauthorized();

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
                NeighborNorth = neighborNorth
            };

            foreach (var consumableDto in roomDto.Consumables)
            {
                try
                {
                    var consumable = dungeon.ConfiguredInspectables.OfType<Consumable>().SingleOrDefault(c => c.Id == consumableDto.Id);

                    if (consumable is not null) room.Inspectables.Add(consumable);
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
                    var wearable = dungeon.ConfiguredInspectables.OfType<Wearable>().SingleOrDefault(c => c.Id == wearableDto.Id);

                    if (wearable is not null) room.Inspectables.Add(wearable);
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
                    var usable = dungeon.ConfiguredInspectables.OfType<Usable>().SingleOrDefault(c => c.Id == usableDto.Id);

                    if (usable is not null) room.Inspectables.Add(usable);
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
                    var takeable = dungeon.ConfiguredInspectables.OfType<Takeable>().SingleOrDefault(c => c.Id == takeableDto.Id);

                    if (takeable is not null) room.Inspectables.Add(takeable);
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
                    var npc = dungeon.ConfiguredInspectables.OfType<Npc>().SingleOrDefault(c => c.Id == npcDto.Id);

                    if (npc is not null) room.Inspectables.Add(npc);
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
    }
}

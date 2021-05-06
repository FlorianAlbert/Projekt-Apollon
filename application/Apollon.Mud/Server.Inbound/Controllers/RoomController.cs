using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
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

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IGameDbService GameConfigService { get; }

        public RoomController(IGameDbService gameDbService)
        {
            GameConfigService = gameDbService;
        }

        [HttpGet]
        [Authorize(Roles = "Player, Admin")]
        public IActionResult GetAll([FromRoute] Guid dungeonId)
        {
            var dungeon = GameConfigService.Get<Dungeon>(dungeonId);

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
    }
}

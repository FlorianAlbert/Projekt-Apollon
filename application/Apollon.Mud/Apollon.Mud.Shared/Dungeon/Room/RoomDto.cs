using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Mud.Shared.Dungeon.Inspectable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Shared.Dungeon.Npc;
using Apollon.Mud.Shared.Dungeon.Requestable;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Room
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class RoomDto
    {
        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Inspectables")]
        public ICollection<InspectableDto> Inspectables { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Takeables")]
        public ICollection<TakeableDto> Takeables { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Consumables")]
        public ICollection<ConsumableDto> Consumables { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Wearables")]
        public ICollection<WearableDto> Wearables { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Usables")]
        public ICollection<UsableDto> Usables { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Npcs")]
        public ICollection<NpcDto> Npcs { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("NeighborNorthId")]
        public Guid NeighborNorthId { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("NeighborEastId")]
        public Guid NeighborEastId { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("NeighborSouthId")]
        public Guid NeighborSouthId { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("NeighborWestId")]
        public Guid NeighborWestId { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("SpecialActions")]
        public ICollection<RequestableDto> SpecialActions { get; set; }
    }
}

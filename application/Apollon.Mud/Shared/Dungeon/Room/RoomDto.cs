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
    /// Class which represents the data representation of IRoom.
    /// </summary>
    public class RoomDto
    {
        /// <summary>
        /// The id of the room.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the room is currently available in the dungeon.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The description of the room.
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the room.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The inspectables which are placed in the room
        /// </summary>
        [JsonProperty("Inspectables")]
        public ICollection<InspectableDto> Inspectables { get; set; }

        /// <summary>
        /// The takeables which are placed in the room
        /// </summary>
        [JsonProperty("Takeables")]
        public ICollection<TakeableDto> Takeables { get; set; }

        /// <summary>
        /// The consumables which are placed in the room
        /// </summary>
        [JsonProperty("Consumables")]
        public ICollection<ConsumableDto> Consumables { get; set; }

        /// <summary>
        /// The wearables which are placed in the room
        /// </summary>
        [JsonProperty("Wearables")]
        public ICollection<WearableDto> Wearables { get; set; }

        /// <summary>
        /// The usables which are placed in the room
        /// </summary>
        [JsonProperty("Usables")]
        public ICollection<UsableDto> Usables { get; set; }

        /// <summary>
        /// The npcs which are placed in the room
        /// </summary>
        [JsonProperty("Npcs")]
        public ICollection<NpcDto> Npcs { get; set; }

        /// <summary>
        /// The north neighbors id of the room.
        /// </summary>
        [JsonProperty("NeighborNorthId")]
        public Guid NeighborNorthId { get; set; }

        /// <summary>
        /// The east neighbors id of the room.
        /// </summary>
        [JsonProperty("NeighborEastId")]
        public Guid NeighborEastId { get; set; }

        /// <summary>
        /// The south neighbors id of the room.
        /// </summary>
        [JsonProperty("NeighborSouthId")]
        public Guid NeighborSouthId { get; set; }

        /// <summary>
        /// The west neighbors id of the room.
        /// </summary>
        [JsonProperty("NeighborWestId")]
        public Guid NeighborWestId { get; set; }

        /// <summary>
        /// The special actions/requestables which are executable in the room.
        /// </summary>
        [JsonProperty("SpecialActions")]
        public ICollection<RequestableDto> SpecialActions { get; set; }
    }
}

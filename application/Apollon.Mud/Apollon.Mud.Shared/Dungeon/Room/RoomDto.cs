using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Mud.Shared.Dungeon.Inspectable;
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

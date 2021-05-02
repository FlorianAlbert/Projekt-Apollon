using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Inspectable.Takeable
{
    /// <summary>
    /// Class which represents the data representation of ITakeable.
    /// </summary>
    public class TakeableDto
    {
        /// <summary>
        /// The unique id of the takeable.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the takeable is currently available in the dungeon.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The description of the takeable.
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the takeable.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The weight of the takeable.
        /// </summary>
        [JsonProperty("Weight")]
        public int Weight { get; set; }
    }
}

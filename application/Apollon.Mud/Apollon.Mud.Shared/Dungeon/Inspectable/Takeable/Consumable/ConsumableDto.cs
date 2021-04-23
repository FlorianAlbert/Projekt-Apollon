using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class ConsumableDto
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
    }
}

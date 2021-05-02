using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable
{
    /// <summary>
    /// Class which represents the data representation of IConsumable.
    /// </summary>
    public class ConsumableDto
    {
        /// <summary>
        /// The unique id of the consumable.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the consumable is currently available in the dungeon.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The description of the consumable.
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the consumable.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The weight of the consumable.
        /// </summary>
        [JsonProperty("Weight")]
        public int Weight { get; set; }

        /// <summary>
        /// The effect description of the consumable.
        /// </summary>
        [JsonProperty("EffectDescription")]
        public string EffectDescription { get; set; }
    }
}

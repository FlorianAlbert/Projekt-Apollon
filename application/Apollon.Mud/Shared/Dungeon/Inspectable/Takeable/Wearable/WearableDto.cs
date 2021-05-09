using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable
{
    /// <summary>
    /// Class which represents the data representation of IWearable.
    /// </summary>
    public class WearableDto
    {
        /// <summary>
        /// The unique id of the wearable.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the wearable is currently available in the dungeon.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The description of the wearable.
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the wearable.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The weight of the wearable.
        /// </summary>
        [JsonProperty("Weight")]
        public int Weight { get; set; }

        /// <summary>
        /// The protection boost of the wearable.
        /// </summary>
        [JsonProperty("ProtectionBoost")]
        public int ProtectionBoost { get; set; }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable
{
    /// <summary>
    /// Class which represents the data representation of IUsable.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UsableDto
    {
        /// <summary>
        /// The unique id of the usable.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the usable is currently available in the dungeon.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The description of the usable.
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the usable.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The weight of the usable.
        /// </summary>
        [JsonProperty("Weight")]
        public int Weight { get; set; }

        /// <summary>
        /// The damage boost of the usable.
        /// </summary>
        [JsonProperty("DamageBoost")]
        public int DamageBoost { get; set; }
    }
}

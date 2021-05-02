using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Race
{
    /// <summary>
    /// Class which represents the data representation of IRace.
    /// </summary>
    public class RaceDto
    {
        /// <summary>
        /// The unique id of the race.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the race is currently playable.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The name of the race.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The description of the race.
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// The default health of the race.
        /// </summary>
        [JsonProperty("DefaultHealth")]
        public int DefaultHealth { get; set; }

        /// <summary>
        /// The default protection of the race.
        /// </summary>
        [JsonProperty("DefaultProtection")]
        public int DefaultProtection { get; set; }

        /// <summary>
        /// The default damage of the race.
        /// </summary>
        [JsonProperty("DefaultDamage")]
        public int DefaultDamage { get; set; }
    }
}

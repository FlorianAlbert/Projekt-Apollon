using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Inspectable
{
    /// <summary>
    /// Class which represents the data representation of IInspectable.
    /// </summary>
    public class InspectableDto
    {
        /// <summary>
        /// The id of the InspectableDto.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status of the InspectableDto.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The description of the InspectableDto.
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the InspectableDto.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}

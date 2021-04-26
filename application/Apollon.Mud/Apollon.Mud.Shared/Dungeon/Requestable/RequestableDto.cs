using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Requestable
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class RequestableDto
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
        [JsonProperty("MessageRegex")]
        public string MessageRegex { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("PatternForPlayer")]
        public string PatternForPlayer { get; set; }
    }
}

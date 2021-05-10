using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Requestable
{
    /// <summary>
    /// Class which represents the data representation of IRequestable.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RequestableDto
    {
        /// <summary>
        /// The id of the requestable.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the requestable is currently executable in the dungeon.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The message regex which represents the "structure" of the requestable. 
        /// </summary>
        [JsonProperty("MessageRegex")]
        public string MessageRegex { get; set; }

        /// <summary>
        /// The pattern for the player which indicates how the execute this requestable.
        /// </summary>
        [JsonProperty("PatternForPlayer")]
        public string PatternForPlayer { get; set; }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Npc
{
    /// <summary>
    /// Class which represents the data representation of INpc.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class NpcDto
    {
        /// <summary>
        /// The unique id of the npc.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the npc is currently available in the dungeon.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The description of the npc.
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// The name of the npc.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The text of the npc which it can say.
        /// </summary>
        [JsonProperty("Text")]
        public string Text { get; set; }
    }
}

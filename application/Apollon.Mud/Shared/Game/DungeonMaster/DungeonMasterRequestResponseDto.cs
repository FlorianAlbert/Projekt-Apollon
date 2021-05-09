using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Shared.Dungeon.Avatar;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Game.DungeonMaster
{
    /// <summary>
    /// Class which represents the answer of the requestable from the dungeon master.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DungeonMasterRequestResponseDto
    {
        /// <summary>
        /// The answer of the requestable from the dungeon master.
        /// </summary>
        [JsonProperty("Message")]
        public string Message { get; set; }

        /// <summary>
        /// The avatar which executed the requestable.
        /// </summary>
        [JsonProperty("Avatar")]
        public AvatarDto Avatar { get; set; }
    }
}
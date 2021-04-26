using Apollon.Mud.Shared.Dungeon.Avatar;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Game.DungeonMaster
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class DungeonMasterRequestResponseDto
    {
        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Message")]
        public string Message { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Avatar")]
        public AvatarDto Avatar { get; set; }
    }
}
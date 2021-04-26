using Apollon.Mud.Shared.Dungeon.Avatar;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Game.DungeonMaster
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class DungeonMasterRequestDto
    {
        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Request")]
        public string Request { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Avatar")]
        public AvatarDto Avatar { get; set; }
    }
}
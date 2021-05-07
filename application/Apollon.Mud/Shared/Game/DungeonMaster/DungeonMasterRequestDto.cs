using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Shared.Dungeon.Avatar;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Game.DungeonMaster
{
    /// <summary>
    /// Class which represents the requestables which the dungeon master has to answer/work on.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DungeonMasterRequestDto
    {
        /// <summary>
        /// The text of the request.
        /// </summary>
        [JsonProperty("Request")]
        public string Request { get; set; }

        /// <summary>
        /// The avatar which executed the requestable.
        /// </summary>
        [JsonProperty("Avatar")]
        public AvatarDto Avatar { get; set; }
    }
}
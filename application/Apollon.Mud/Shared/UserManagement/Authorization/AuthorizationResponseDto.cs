using Apollon.Mud.Shared.Dungeon.User;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Authorization
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class AuthorizationResponseDto
    {
        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Token")]
        public string Token { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DungeonUserDto")]
        public DungeonUserDto DungeonUserDto { get; set; }
    }
}
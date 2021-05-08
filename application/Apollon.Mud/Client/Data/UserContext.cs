using Apollon.Mud.Shared.Dungeon.User;

namespace Apollon.Mud.Client.Data
{
    public class UserContext
    {
        /// <summary>
        /// Manages the data related to the user in the dungeon
        /// </summary>
        public DungeonUserDto DungeonUserContext { get; set; }

        /// <summary>
        /// Manages the token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Checks if the user is authorized
        /// </summary>
        public bool IsAuthorized { get; set; } = false;

    }
}

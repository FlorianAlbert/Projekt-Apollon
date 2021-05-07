using Apollon.Mud.Shared.Dungeon.User;

namespace Apollon.Mud.Client.Data
{
    /// <summary>
    /// The Context to save user-Information and keep him logged in
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// Saves ID, E-Mail, LastActive Time and Confirmation state
        /// </summary>
        public DungeonUserDto DungeonUserContext { get; set; }

        /// <summary>
        /// The users authorization Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The Bool that represents authorization
        /// </summary>
        public bool IsAuthorized { get; set; } = false;

    }
}

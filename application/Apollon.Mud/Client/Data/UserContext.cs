using Apollon.Mud.Shared.Dungeon.User;

namespace Apollon.Mud.Client.Data
{
    public class UserContext
    {
        /// <summary>
        /// TODO
        /// </summary>
        public DungeonUserDto DungeonUserContext { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsAuthorized { get; set; } = false;

    }
}

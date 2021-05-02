using System;

namespace Apollon.Mud.Server.Model.Implementations
{
    /// <summary>
    /// Describes a connection that exists during playing in a dungeon
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// DungeonId of the dungeon the client plays in
        /// </summary>
        public Guid DungeonId { get; set; }

        /// <summary>
        /// SignalR ConnectionId for the chat of the client
        /// </summary>
        public string ChatConnectionId { get; set; }

        /// <summary>
        /// SignalR ConnectionId for the chat of the game
        /// </summary>
        public string GameConnectionId { get; set; }

        /// <summary>
        /// AvatarId of the avatar the client is playing
        /// </summary>
        public Guid? AvatarId { get; set; }
    }
}
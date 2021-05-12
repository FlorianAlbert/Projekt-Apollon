using System;

namespace Apollon.Mud.Shared.Game
{
    /// <summary>
    /// Class which represents the entering of a dungeon by an avatar.
    /// </summary>
    public class EnterDungeonDto
    {
        /// <summary>
        /// Flag which indicates if the user wants to play as dungeon master.
        /// </summary>
        public bool AsDungeonMaster { get; set; }

        /// <summary>
        /// The id of the dungeon which should be entered.
        /// </summary>
        public Guid DungeonId { get; set; }

        /// <summary>
        /// The id of the avatar with which the user wants to enter the dungeon.
        /// </summary>
        public Guid? AvatarId { get; set; }

        /// <summary>
        /// The SignalR connection id for the chat.
        /// </summary>
        public string ChatConnectionId { get; set; }

        /// <summary>
        /// The SignalR connection id for the game.
        /// </summary>
        public string GameConnectionId { get; set; }
    }
}
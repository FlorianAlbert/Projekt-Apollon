using System;

namespace Apollon.Mud.Server.Domain.Interfaces.Chat
{
    /// <summary>
    /// This Service contains all the Logic
    /// to distribute the posted chat messages to
    /// the other clients correctly
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Endpoint for Players to send a chat message to all other avatars in the same room
        /// </summary>
        /// <param name="dungeonId">DungeonId of the dungeon the sender avatar is part of</param>
        /// <param name="avatarId">AvatarId of the sending avatar</param>
        /// <param name="message">Sent message</param>
        void PostRoomMessage(Guid dungeonId, Guid avatarId, string message);

        /// <summary>
        /// Endpoint for Players to send a chat message to a specific avatar in the same room
        /// and for Dungeon Master to send  a chat message to a specific avatar in the dungeon
        /// </summary>
        /// <param name="dungeonId">DungeonId of the dungeon the sender avatar or Dungeon Master is part of</param>
        /// <param name="senderAvatarId">AvatarId of the sending avatar or null if sender is Dungeon Master</param>
        /// <param name="recipientName">Name of the avatar that should get the message</param>
        /// <param name="message">Sent message</param>
        void PostWhisperMessage(Guid dungeonId, Guid? senderAvatarId, string recipientName, string message);

        /// <summary>
        /// Endpoint for Dungeon Master to send a chat message to all avatars in the dungeon
        /// </summary>
        /// <param name="dungeonId">DungeonId of the dungeon the Dungeon Master is part of</param>
        /// <param name="message">Sent message</param>
        void PostGlobalMessage(Guid dungeonId, string message);
    }
}
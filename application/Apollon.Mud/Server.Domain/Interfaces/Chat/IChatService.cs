using System;

namespace Apollon.Mud.Server.Domain.Interfaces.Chat
{
    public interface IChatService
    {
        void PostRoomMessage(Guid dungeonId, Guid avatarId, string message);

        void PostWhisperMessage(Guid dungeonId, Guid? senderAvatarId, string recipientName, string message);

        void PostGlobalMessage(Guid dungeonId, string message);
    }
}
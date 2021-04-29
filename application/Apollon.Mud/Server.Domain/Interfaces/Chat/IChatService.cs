using System;

namespace Apollon.Mud.Server.Domain.Interfaces.Chat
{
    public interface IChatService
    {
        void PostRoomMessage(Guid dungeonId, string senderName, string message);

        void PostWhisperMessage(Guid dungeonId, string senderName, string recipientName, string message);

        void PostGlobalMessage(Guid dungeonId, string message);
    }
}
using System.Diagnostics.CodeAnalysis;

namespace Apollon.Mud.Shared.Game.Chat
{
    /// <summary>
    /// Class which represents a chat-message.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ChatMessageDto
    {
        /// <summary>
        /// The recipient of this message.
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// The Message to be send.
        /// </summary>
        public string Message { get; set; }
    }
}
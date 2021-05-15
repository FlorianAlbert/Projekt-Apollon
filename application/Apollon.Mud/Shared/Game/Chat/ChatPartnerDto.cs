using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Game.Chat
{
    /// <summary>
    /// Class which represents the available avatars to chat with.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ChatPartnerDto
    {
        /// <summary>
        /// The avatars id.
        /// </summary>
        [JsonProperty("AvatarId")]
        public Guid? AvatarId { get; set; }

        /// <summary>
        /// The avatars name.
        /// </summary>
        [JsonProperty("AvatarName")]
        public string AvatarName { get; set; }
    }
}
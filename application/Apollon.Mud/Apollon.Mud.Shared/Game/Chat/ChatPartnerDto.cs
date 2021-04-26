using System;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Game.Chat
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class ChatPartnerDto
    {
        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("AvatarId")]
        public Guid AvatarId { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("AvatarName")]
        public string AvatarName { get; set; }
    }
}
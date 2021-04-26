using System;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Password
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class PasswortResetConfirmationDto
    {
        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("PasswordResetRequestId")]
        public Guid PasswordResetRequestId { get; set; }

        /// <summary>
        /// ToDo Hash statt Password versenden?! was braucht das Identity?
        /// </summary>
        [JsonProperty("NewPassword")]
        public string NewPassword { get; set; }
    }
}
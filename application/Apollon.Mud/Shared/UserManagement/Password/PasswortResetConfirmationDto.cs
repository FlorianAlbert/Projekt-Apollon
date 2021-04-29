using System;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Password
{
    /// <summary>
    /// Class which represents the confimation of the request to reset the password of an user.
    /// </summary>
    public class PasswortResetConfirmationDto
    {
        /// <summary>
        /// The id of the password reset request. 
        /// </summary>
        [JsonProperty("PasswordResetRequestId")]
        public Guid PasswordResetRequestId { get; set; }

        /// <summary>
        /// The password which was sat as new.
        /// </summary>
        [JsonProperty("NewPassword")]
        public string NewPassword { get; set; }
    }
}
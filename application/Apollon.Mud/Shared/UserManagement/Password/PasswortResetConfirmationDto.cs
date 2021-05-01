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
        /// The password which was sat as new.
        /// </summary>
        [JsonProperty("NewPassword")]
        public string NewPassword { get; set; }

        /// <summary>
        /// The token to reset the password.
        /// </summary>
        [JsonProperty("Token")]
        public string Token { get; set; } //ToDo change in UML
    }
}
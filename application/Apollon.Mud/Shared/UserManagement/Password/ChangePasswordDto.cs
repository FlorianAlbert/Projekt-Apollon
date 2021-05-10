using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Password
{
    /// <summary>
    /// Class which represents a request to change the password of an user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ChangePasswordDto
    {
        /// <summary>
        /// The old password of the user.
        /// </summary>
        [JsonProperty("OldPassword")]
        public string OldPassword { get; set; }

        /// <summary>
        /// The new password which should be used.
        /// </summary>
        [JsonProperty("NewPassword")]
        public string NewPassword { get; set; }
    }
}
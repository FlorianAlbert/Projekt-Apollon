using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Password
{
    /// <summary>
    /// Class which represents a request to reset the password of a user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RequestPasswordResetDto
    {
        /// <summary>
        /// The email-address of the user who's password should be reset.
        /// </summary>
        [JsonProperty("UserEmail")]
        public string UserEmail { get; set; }
    }
}
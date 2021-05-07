using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Authorization
{
    /// <summary>
    /// Class which represents an authorization request.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthorizationRequestDto
    {
        /// <summary>
        /// The email with which an authorization is requested.
        /// </summary>
        [JsonProperty("UserEmail")]
        public string UserEmail { get; set; }

        /// <summary>
        /// The password with which an authorization is requested. 
        /// </summary>
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Registration
{
    /// <summary>
    ///  Class which represents a registration request.
    /// </summary>
    public class RegistrationRequestDto
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
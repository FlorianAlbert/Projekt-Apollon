using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Authorization
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class AuthorizationRequestDto
    {
        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("UserEmail")]
        public string UserEmail { get; set; }

        /// <summary>
        /// ToDo 
        /// </summary>
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
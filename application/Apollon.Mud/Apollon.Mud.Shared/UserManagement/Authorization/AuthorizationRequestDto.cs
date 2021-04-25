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
        /// ToDo Hash statt Password versenden?! was braucht das Identity?
        /// </summary>
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
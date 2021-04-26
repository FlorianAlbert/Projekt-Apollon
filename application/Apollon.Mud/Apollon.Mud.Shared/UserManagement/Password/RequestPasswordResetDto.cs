using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Password
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class RequestPasswordResetDto
    {
        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("UserEmail")]
        public string UserEmail { get; set; }
    }
}
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Password
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("OldPassword")]
        public string OldPassword { get; set; }

        /// <summary>
        /// ToDo Hash statt Password versenden?! was braucht das Identity?
        /// </summary>
        [JsonProperty("NewPassword")]
        public string NewPassword { get; set; }
    }
}
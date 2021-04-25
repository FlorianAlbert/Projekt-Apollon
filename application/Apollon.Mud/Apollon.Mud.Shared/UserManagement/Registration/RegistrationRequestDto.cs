using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Registration
{
    /// <summary>
    /// ToDo gleiches Dto wie bei AuthorizationRequest. Könnte man nicht beides in ein Dto packen und für beide Anwensungsbereiche verwenden
    /// </summary>
    public class RegistrationRequestDto
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
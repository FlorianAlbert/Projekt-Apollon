using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.User
{
    /// <summary>
    /// ToDo ausreichend?!
    /// </summary>
    public class DungeonUserDto
    {
        /// <summary>
        /// ToDo serializes DateTime as ISO 8601 standard
        /// </summary>
        [JsonProperty("LastActive")]
        public DateTime LastActive { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Email")]
        public string Email { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("PasswordHash")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("EmailConfirmed")]
        public bool EmailConfirmed { get; set; }
    }
}

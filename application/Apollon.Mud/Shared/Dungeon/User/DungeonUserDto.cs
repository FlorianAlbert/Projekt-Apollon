using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.User
{
    /// <summary>
    /// Class which represents the data representation of IDungeonUser.
    /// </summary>
    public class DungeonUserDto
    {
        /// <summary>
        /// The time when the user was last seen.
        /// </summary>
        [JsonProperty("LastActive")]
        public DateTime LastActive { get; set; }

        /// <summary>
        /// The users email-address.
        /// </summary>
        [JsonProperty("Email")]
        public string Email { get; set; }

        /// <summary>
        /// If the user confirmed his/her email-address.
        /// </summary>
        [JsonProperty("EmailConfirmed")]
        public bool EmailConfirmed { get; set; }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.User
{
    /// <summary>
    /// Class which represents the data representation of IDungeonUser.
    /// </summary>
    [ExcludeFromCodeCoverage]
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

        public override bool Equals(object? obj)
        {
            if (obj is DungeonUserDto dUDto)
            {
                return LastActive == dUDto.LastActive
                       && Email == dUDto.Email
                       && EmailConfirmed == dUDto.EmailConfirmed;
            }
                return base.Equals(obj);
        }
    }
}

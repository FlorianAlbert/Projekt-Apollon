using Microsoft.AspNetCore.Identity;
using System;

namespace Apollon.Mud.Server.Model.Implementations.User
{
    /// <summary>
    /// Describes a user that can register on the platform
    /// </summary>
    /// <inheritdoc cref="IdentityUser"/>
    public class DungeonUser : IdentityUser
    {
        /// <summary>
        /// Last time the user was active
        /// </summary>
        public DateTime LastActive { get; set; }
    }
}

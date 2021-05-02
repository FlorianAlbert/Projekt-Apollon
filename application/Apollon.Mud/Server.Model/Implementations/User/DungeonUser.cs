using Microsoft.AspNetCore.Identity;
using System;

namespace Apollon.Mud.Server.Model.Implementations.User
{
    public class DungeonUser : IdentityUser
    {
        DateTime LastActive { get; set; }
    }
}

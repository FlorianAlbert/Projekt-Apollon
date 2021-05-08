using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.Dungeons;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;

namespace Apollon.Mud.Server.Model.Implementations.User
{
    /// <summary>
    /// Describes a user that can register on the platform
    /// </summary>
    /// <inheritdoc cref="IdentityUser"/>
    [ExcludeFromCodeCoverage]
    public class DungeonUser : IdentityUser
    {
        /// <summary>
        /// Last time the user was active
        /// </summary>
        public DateTime LastActive { get; set; }

        /// <summary>
        /// The dungeons this user is the DungeonOwner of
        /// </summary>
        public ICollection<Dungeon> DungeonOwnerDungeons { get; set; }

        /// <summary>
        /// The dungeons this user is blacklisted in
        /// </summary>
        public ICollection<Dungeon> BlackListDungeons { get; set; }

        /// <summary>
        /// The dungeons this user is whitelisted in
        /// </summary>
        public ICollection<Dungeon> WhiteListDungeons { get; set; }

        /// <summary>
        /// The dungeons this user is a DungeonMaster of
        /// </summary>
        public ICollection<Dungeon> DungeonMasterDungeons { get; set; }

        /// <summary>
        /// The dungeons this user is currently the DungeonMaster of
        /// </summary>
        public ICollection<Dungeon> CurrentDungeonMasterDungeons { get; set; }

        /// <summary>
        /// The dungeons this user has open EnterRequests on
        /// </summary>
        public ICollection<Dungeon> OpenRequestDungeons { get; set; }

        /// <summary>
        /// The avatars this user owns
        /// </summary>
        public ICollection<Avatar> Avatars { get; set; }
    }
}

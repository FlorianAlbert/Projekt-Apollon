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

        private ICollection<Dungeon> _DungeonOwnerDungeons;

        /// <summary>
        /// The dungeons this user is the DungeonOwner of
        /// </summary>
        public virtual ICollection<Dungeon> DungeonOwnerDungeons => _DungeonOwnerDungeons ??= new List<Dungeon>();

        private ICollection<Dungeon> _BlackListDungeons;

        /// <summary>
        /// The dungeons this user is blacklisted in
        /// </summary>
        public virtual ICollection<Dungeon> BlackListDungeons => _BlackListDungeons ??= new List<Dungeon>();

        private ICollection<Dungeon> _WhiteListDungeons;

        /// <summary>
        /// The dungeons this user is whitelisted in
        /// </summary>
        public virtual ICollection<Dungeon> WhiteListDungeons => _WhiteListDungeons ??= new List<Dungeon>();

        private ICollection<Dungeon> _DungeonMasterDungeons;

        /// <summary>
        /// The dungeons this user is a DungeonMaster of
        /// </summary>
        public virtual ICollection<Dungeon> DungeonMasterDungeons => _DungeonMasterDungeons ??= new List<Dungeon>();

        private ICollection<Dungeon> _CurrentDungeonMasterDungeons;

        /// <summary>
        /// The dungeons this user is currently the DungeonMaster of
        /// </summary>
        public virtual ICollection<Dungeon> CurrentDungeonMasterDungeons => _CurrentDungeonMasterDungeons ??= new List<Dungeon>();

        private ICollection<Dungeon> _OpenRequestDungeons;

        /// <summary>
        /// The dungeons this user has open EnterRequests on
        /// </summary>
        public virtual ICollection<Dungeon> OpenRequestDungeons => _OpenRequestDungeons ??= new List<Dungeon>();

        private ICollection<Avatar> _Avatars;

        /// <summary>
        /// The avatars this user owns
        /// </summary>
        public virtual ICollection<Avatar> Avatars => _Avatars ??= new List<Avatar>();
    }
}

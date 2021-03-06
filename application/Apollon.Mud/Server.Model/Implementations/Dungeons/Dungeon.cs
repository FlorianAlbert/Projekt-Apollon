using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Server.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Races;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Shared.Implementations.Dungeons;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons
{
    /// <summary>
    /// Describes a dungeon
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Dungeon : IApprovable
    {
        /// <summary>
        /// Creates a new instance of Dungeon
        /// </summary>
        /// <param name="dungeonEpoch">The epoch of the new dungeon</param>
        /// <param name="dungeonDescription">The description of the new dungeon</param>
        public Dungeon(string dungeonEpoch, string dungeonDescription, string dungeonName)
        {
            Id = Guid.NewGuid();

            DungeonEpoch = dungeonEpoch;
            DungeonDescription = dungeonDescription;
            DungeonName = dungeonName;

            Status = Status.Pending;
        }

        /// <summary>
        /// Epoch of the dungeon
        /// </summary>
        public string DungeonEpoch { get; set; }

        /// <summary>
        /// Description of the dungeon
        /// </summary>
        public string DungeonDescription { get; set; }

        public Guid? DefaultRoomId { get; set; }

        /// <summary>
        /// Start room of the dungeon
        /// </summary>
        public virtual Room DefaultRoom { get; set; }

        private ICollection<Race> _ConfiguredRaces;

        /// <summary>
        /// All races an avatar can choose between
        /// </summary>
        public virtual ICollection<Race> ConfiguredRaces => _ConfiguredRaces ??= new List<Race>();

        private ICollection<Class> _ConfiguredClasses;

        /// <summary>
        /// All classes an avatar can choose between
        /// </summary>
        public virtual ICollection<Class> ConfiguredClasses => _ConfiguredClasses ??= new List<Class>();

        private ICollection<Room> _ConfiguredRooms;

        /// <summary>
        /// All rooms of the dungeon
        /// </summary>
        public virtual ICollection<Room> ConfiguredRooms => _ConfiguredRooms ??= new List<Room>();

        private ICollection<Inspectable> _ConfiguredInspectables;

        /// <summary>
        /// All inspectables that can be placed in the rooms
        /// </summary>
        public virtual ICollection<Inspectable> ConfiguredInspectables => _ConfiguredInspectables ??= new List<Inspectable>();

        private ICollection<Requestable> _ConfiguredRequestables;

        /// <summary>
        /// All requestables that can be attached to a room
        /// </summary>
        public virtual ICollection<Requestable> ConfiguredRequestables => _ConfiguredRequestables ??= new List<Requestable>();

        private ICollection<Avatar> _RegisteredAvatars;

        /// <summary>
        /// All registered avatar
        /// </summary>
        public virtual ICollection<Avatar> RegisteredAvatars => _RegisteredAvatars ??= new List<Avatar>();

        private ICollection<DungeonUser> _DungeonMasters;

        /// <summary>
        /// All possible Dungeon Masters of the dungeon
        /// </summary>
        public virtual ICollection<DungeonUser> DungeonMasters => _DungeonMasters ??= new List<DungeonUser>();

        private ICollection<DungeonUser> _WhiteList;

        /// <summary>
        /// List of all users who have access to this dungeon
        /// </summary>
        public virtual ICollection<DungeonUser> WhiteList => _WhiteList ??= new List<DungeonUser>();

        private ICollection<DungeonUser> _BlackList;

        /// <summary>
        /// List of all users who are not allowed to have avatars in this dungeon
        /// </summary>
        public virtual ICollection<DungeonUser> BlackList => _BlackList ??= new List<DungeonUser>();

        private ICollection<DungeonUser> _OpenRequests;

        /// <summary>
        /// List of all users who want to have access to this dungeon, in case it is private
        /// </summary>
        public virtual ICollection<DungeonUser> OpenRequests => _OpenRequests ??= new List<DungeonUser>();

        /// <summary>
        /// User who is currently the Dungeon Master
        /// </summary>
        public virtual DungeonUser CurrentDungeonMaster { get; set; }

        /// <summary>
        /// User who owns this dungeon
        /// </summary>
        public virtual DungeonUser DungeonOwner { get; set; }

        /// <summary>
        /// Defines wether this dungeon is public or private accessible
        /// </summary>
        public Visibility Visibility { get; set; }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }

        /// <summary>
        /// Name of the dungeon
        /// </summary>
        public string DungeonName { get; set; }

        /// <summary>
        /// Time the dungeon got played by an avatar the last time
        /// </summary>
        public DateTime LastActive { get; set; }
    }
}

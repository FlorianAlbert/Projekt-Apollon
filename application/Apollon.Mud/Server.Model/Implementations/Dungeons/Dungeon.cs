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

        /// <summary>
        /// Start room of the dungeon
        /// </summary>
        public virtual Room DefaultRoom { get; set; }

        private ICollection<Race> _ConfiguredRaces;

        /// <summary>
        /// All races an avatar can choose between
        /// </summary>
        public virtual ICollection<Race> ConfiguredRaces
        {
            get
            {
                return _ConfiguredRaces ??= new List<Race>();
            }
        }

        private ICollection<Class> _ConfiguredClasses;

        /// <summary>
        /// All classes an avatar can choose between
        /// </summary>
        public virtual ICollection<Class> ConfiguredClasses
        {
            get
            {
                return _ConfiguredClasses ??= new List<Class>();
            }
        }

        private ICollection<Room> _ConfiguredRooms;

        /// <summary>
        /// All rooms of the dungeon
        /// </summary>
        public virtual ICollection<Room> ConfiguredRooms
        {
            get
            {
                return _ConfiguredRooms ??= new List<Room>();
            }
        }

        private ICollection<Inspectable> _ConfiguredInspectables;

        /// <summary>
        /// All inspectables that can be placed in the rooms
        /// </summary>
        public virtual ICollection<Inspectable> ConfiguredInspectables
        {
            get
            {
                return _ConfiguredInspectables ??= new List<Inspectable>();
            }
        }

        private ICollection<Requestable> _ConfiguredRequestables;

        /// <summary>
        /// All requestables that can be attached to a room
        /// </summary>
        public virtual ICollection<Requestable> ConfiguredRequestables
        {
            get
            {
                return _ConfiguredRequestables ??= new List<Requestable>();
            }
        }

        private ICollection<Avatar> _RegisteredAvatars;

        /// <summary>
        /// All registered avatar
        /// </summary>
        public virtual ICollection<Avatar> RegisteredAvatars
        {
            get
            {
                return _RegisteredAvatars ??= new List<Avatar>();
            }
        }

        private ICollection<DungeonUser> _DungeonMasters;

        /// <summary>
        /// All possible Dungeon Masters of the dungeon
        /// </summary>
        public virtual ICollection<DungeonUser> DungeonMasters
        {
            get
            {
                return _DungeonMasters ??= new List<DungeonUser>();
            }
        }

        private ICollection<DungeonUser> _WhiteList;

        /// <summary>
        /// List of all users who have access to this dungeon
        /// </summary>
        public virtual ICollection<DungeonUser> WhiteList
        {
            get
            {
                return _WhiteList ??= new List<DungeonUser>();
            }
        }

        private ICollection<DungeonUser> _BlackList;

        /// <summary>
        /// List of all users who are not allowed to have avatars in this dungeon
        /// </summary>
        public virtual ICollection<DungeonUser> BlackList
        {
            get
            {
                return _BlackList ??= new List<DungeonUser>();
            }
        }

        private ICollection<DungeonUser> _OpenRequests;

        /// <summary>
        /// List of all users who want to have access to this dungeon, in case it is private
        /// </summary>
        public virtual ICollection<DungeonUser> OpenRequests
        {
            get
            {
                return _OpenRequests ??= new List<DungeonUser>();
            }
        }

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

        /** TODO: In PlayerService zu verlagern
        public bool AddNeighborship(IRoom roomSource, Direction direction, IRoom roomSink)
        {
            throw new NotImplementedException();
        }

        public IRoom AddRoom(bool defaultRoom = false)
        {
            throw new NotImplementedException();
        }

        public void ChangeRoom(Avatar avatar, Direction direction)
        {
            throw new NotImplementedException();
        }

        public void EnterDungeon(Avatar avatar)
        {
            throw new NotImplementedException();
        }

        public IRoom GetRoom(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveNeigborship(IRoom room1, IRoom room2)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRoom(IRoom room)
        {
            throw new NotImplementedException();
        } **/
    }
}

using Apollon.Mud.Server.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables;
using Apollon.Mud.Shared.Implementations.Dungeons;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms
{
    /// <summary>
    /// Describes a room in a dungeon
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Room : IApprovable
    {
        /// <summary>
        /// Creates a new instance Room
        /// </summary>
        /// <param name="description">Description of the new room</param>
        /// <param name="name">Name of the new room</param>
        public Room(string description, string name)
        {
            Id = Guid.NewGuid();

            Description = description;
            Name = name;

            Status = Status.Pending;
        }

        /// <summary>
        /// The description of the whole room
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of the room
        /// </summary>
        public string Name { get; set; }

        private ICollection<RoomInspectable> _Inspectables;

        /// <summary>
        /// All inspectables that are currently in this room
        /// </summary>
        public virtual ICollection<RoomInspectable> Inspectables => _Inspectables ??= new List<RoomInspectable>();

        /// <summary>
        /// The room that is in the north
        /// </summary>
        public virtual Room NeighborNorth { get; set; }

        /// <summary>
        /// The room that is in the east
        /// </summary>
        public virtual Room NeighborEast { get; set; }

        /// <summary>
        /// The room that is in the south
        /// </summary>
        public virtual Room NeighborSouth { get; set; }

        /// <summary>
        /// The room that is in the west
        /// </summary>
        public virtual Room NeighborWest { get; set; }

        private ICollection<Requestable> _SpecialActions;

        /// <summary>
        /// All special actions an avatar can do in this room
        /// </summary>
        public virtual ICollection<Requestable> SpecialActions => _SpecialActions ??= new List<Requestable>();

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }

        /// <summary>
        /// The dungeon this room belongs to
        /// </summary>
        public virtual Dungeon Dungeon { get; set; }

        private ICollection<Avatar> _Avatars;

        /// <summary>
        /// The avatars that are currently in this room
        /// </summary>
        public virtual ICollection<Avatar> Avatars => _Avatars ??= new List<Avatar>();
    }
}

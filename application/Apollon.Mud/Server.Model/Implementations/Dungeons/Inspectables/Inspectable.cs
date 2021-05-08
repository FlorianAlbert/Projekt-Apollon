using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Server.Model.Interfaces;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables
{
    /// <summary>
    /// Describes an item that can be inspected by an avatar
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Inspectable : IApprovable
    {
        /// <summary>
        /// Creates a new instance of Insprectable
        /// </summary>
        /// <param name="description">Description of the new inspectable</param>
        /// <param name="name">Name of the new inspectable</param>
        public Inspectable(string description, string name)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;

            Status = Status.Pending;
        }

        /// <summary>
        /// The description of the item
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }

        /// <summary>
        /// The Rooms this inspectable is placed in
        /// </summary>
        public ICollection<Room> Rooms { get; set; }

        /// <summary>
        /// The dungeon the inspectable is configured in
        /// </summary>
        public Dungeon Dungeon { get; set; }
    }
}

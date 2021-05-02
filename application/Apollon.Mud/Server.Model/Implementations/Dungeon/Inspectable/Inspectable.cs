using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using System;
using Apollon.Mud.Server.Model.Interfaces;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable
{
    /// <inheritdoc cref="IInspectable"/>
    public class Inspectable : IInspectable
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

        /// <inheritdoc cref="IInspectable.Description"/>
        public string Description { get; set; }

        /// <inheritdoc cref="IInspectable.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }
    }
}

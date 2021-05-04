using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables
{
    /// <summary>
    /// Describes an item that can be taken by an avatar
    /// </summary>
    public class Takeable : Inspectable
    {
        /// <summary>
        /// Creates a new instance of Takeable
        /// </summary>
        /// <param name="weight">Weight of the new takeable</param>
        /// <param name="description">Description of the new takeable</param>
        /// <param name="name">Name of the new takeable</param>
        public Takeable(int weight, string description, string name)
            : base(description,name)
        {
            Id = Guid.NewGuid();
            
            Weight = weight;

            Status = Status.Pending;
        }

        /// <summary>
        /// The weight of the item
        /// </summary>
        public int Weight { get; set; }

        /// <inheritdoc cref="Inspectable.Description"/>
        public string Description { get; set; }

        /// <inheritdoc cref="Inspectable.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }
    }
}

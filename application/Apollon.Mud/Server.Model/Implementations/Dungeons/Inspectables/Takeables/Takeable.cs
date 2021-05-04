using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables
{
    /// <inheritdoc cref="ITakeable"/>
    public class Takeable : ITakeable
    {
        /// <summary>
        /// Creates a new instance of Takeable
        /// </summary>
        /// <param name="weight">Weight of the new takeable</param>
        /// <param name="description">Description of the new takeable</param>
        /// <param name="name">Name of the new takeable</param>
        public Takeable(int weight, string description, string name)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            Weight = weight;

            Status = Status.Pending;
        }

        /// <inheritdoc cref="ITakeable.Weight"/>
        public int Weight { get; set; }

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

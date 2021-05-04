using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Usable;
using System;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables
{
    /// <inheritdoc cref="IUsable"/>
    public class Usable : IUsable
    {
        /// <summary>
        /// Creates a new instance of Usable
        /// </summary>
        /// <param name="name">Name of the new usable</param>
        /// <param name="description">Description of the new usable</param>
        /// <param name="weight">Weight of the new usable</param>
        /// <param name="damageBoost">Damage Boost of the new usable</param>
        public Usable(string name, string description, int weight, int damageBoost)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            Weight = weight;
            DamageBoost = damageBoost;

            Status = Status.Pending;
        }

        /// <inheritdoc cref="IUsable.DamageBoost"/>
        public int DamageBoost { get; set; }

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

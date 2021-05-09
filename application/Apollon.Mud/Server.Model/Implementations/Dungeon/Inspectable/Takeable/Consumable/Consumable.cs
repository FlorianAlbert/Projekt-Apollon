using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Consumable;
using System;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable.Takeable.Consumable
{
    /// <inheritdoc cref="IConsumable"/>
    public class Consumable : IConsumable
    {
        /// <summary>
        /// Creates a new instance of Consumable
        /// </summary>
        /// <param name="name">Name of the new consumable</param>
        /// <param name="description">Description of the new consumable</param>
        /// <param name="weight">Weight of the new Consumable</param>
        /// <param name="effectDescription">Effect description of the new consumable</param>
        public Consumable(string name, string description, int weight, string effectDescription)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            Weight = weight;
            EffectDescription = effectDescription;

            Status = Status.Pending;
        }

        /// <inheritdoc cref="IConsumable.EffectDescription"/>
        public string EffectDescription { get; set; }

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

using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Wearable;
using System;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables
{
    /// <inheritdoc cref="IWearable"/>
    public class Wearable : IWearable
    {
        /// <summary>
        /// Creates a new instance of Wearable
        /// </summary>
        /// <param name="name">Name of the new wearable</param>
        /// <param name="description">Description of the new wearable</param>
        /// <param name="weight">Weight of the new wearable</param>
        /// <param name="protectionBoost">Protection boost of the new wearable</param>
        public Wearable(string name, string description, int weight, int protectionBoost)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            Weight = weight;
            ProtectionBoost = protectionBoost;

            Status = Status.Pending;
        }

        /// <inheritdoc cref="IWearable.ProtectionBoost"/>
        public int ProtectionBoost { get; set; }

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

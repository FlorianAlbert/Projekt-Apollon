using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Consumable;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable.Takeable.Consumable
{
    public class Consumable : IConsumable
    {
        public Consumable(string name, string description, int weight, string effectDescription)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            Weight = weight;
            EffectDescription = effectDescription;

            Status = Status.Pending;
        }

        public string EffectDescription { get; set; }
        public int Weight { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }
    }
}

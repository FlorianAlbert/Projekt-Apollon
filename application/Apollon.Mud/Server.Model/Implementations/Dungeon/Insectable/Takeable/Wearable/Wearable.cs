using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Wearable;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Insectable.Takeable.Wearable
{
    public class Wearable : IWearable
    {
        public Wearable(string name, string description, int weight, int protectionBoost)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            Weight = weight;
            ProtectionBoost = protectionBoost;

            Status = Status.Pending;
        }

        public int ProtectionBoost { get; set; }
        public int Weight { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }
    }
}

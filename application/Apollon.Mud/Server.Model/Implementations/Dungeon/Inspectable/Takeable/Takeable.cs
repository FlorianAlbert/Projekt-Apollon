using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable.Takeable
{
    public class Takeable : ITakeable
    {
        public Takeable(int weight, string description, string name)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            Weight = weight;

            Status = Status.Pending;
        }


        public int Weight { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }
    }
}

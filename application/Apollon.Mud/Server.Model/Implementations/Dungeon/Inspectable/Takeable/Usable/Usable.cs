using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Usable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable.Takeable.Usable
{
    public class Usable : IUsable
    {
        public Usable(string name, string description, int weight, int damageBoost)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            Weight = weight;
            DamageBoost = damageBoost;

            Status = Status.Pending;
        }

        public int DamageBoost { get; set; }
        public int Weight { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }
    }
}

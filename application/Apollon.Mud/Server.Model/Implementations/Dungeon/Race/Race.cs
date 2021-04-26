using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Race
{
    public class Race : IRace
    {
        public Race(string name, string description, int defaultHealth, int defaultProtection, int defaultDamage)
        {
            Id = Guid.NewGuid();

            name = Name;
            description = Description;
            defaultHealth = DefaultHealth;
            defaultProtection = DefaultProtection;
            defaultDamage = DefaultDamage;

            Status = Status.Pending;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DefaultHealth { get; set; }
        public int DefaultProtection { get; set; }
        public int DefaultDamage { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }
    }
}

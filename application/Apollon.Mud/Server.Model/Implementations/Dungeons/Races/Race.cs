using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Race;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Races
{
    /// <inheritdoc cref="IRace"/>
    public class Race : IRace
    {
        /// <summary>
        /// Creates new instance of Race
        /// </summary>
        /// <param name="name">Name of the new race</param>
        /// <param name="description">Description of the new race</param>
        /// <param name="defaultHealth">Health value of the new race</param>
        /// <param name="defaultProtection">Protection value of the new race</param>
        /// <param name="defaultDamage">Damage value of the new race</param>
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

        /// <inheritdoc cref="IChoosable.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="IChoosable.Description"/>
        public string Description { get; set; }

        /// <inheritdoc cref="IChoosable.DefaultHealth"/>
        public int DefaultHealth { get; set; }

        /// <inheritdoc cref="IChoosable.DefaultProtection"/>
        public int DefaultProtection { get; set; }

        /// <inheritdoc cref="IChoosable.DefaultDamage"/>
        public int DefaultDamage { get; set; }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }
    }
}

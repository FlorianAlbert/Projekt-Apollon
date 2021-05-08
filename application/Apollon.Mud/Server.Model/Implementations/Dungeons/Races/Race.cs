using Apollon.Mud.Server.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Races
{
    /// <summary>
    /// Describes a race an avatar can have
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Race : IChoosable
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

            Name = name;
            Description = description;
            DefaultHealth = defaultHealth;
            DefaultProtection = defaultProtection;
            DefaultDamage = defaultDamage;

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

        /// <summary>
        /// Dungeon the race belongs to
        /// </summary>
        public Dungeon Dungeon { get; set; }

        /// <summary>
        /// Avatars with this race
        /// </summary>
        public ICollection<Avatar> Avatars { get; set; }
    }
}

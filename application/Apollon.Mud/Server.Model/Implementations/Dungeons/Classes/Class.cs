using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Shared.Implementations.Dungeons;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Classes
{
    /// <summary>
    /// Describes a class an avatar can be part of
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Class : IChoosable
    {
        /// <summary>
        /// Creates a new instance of Class
        /// </summary>
        /// <param name="name">Name of the new class</param>
        /// <param name="description">Description of the new class</param>
        /// <param name="defaultHealth">Health value of the new class</param>
        /// <param name="defaultProtection">Protection value of the new class</param>
        /// <param name="defaultDamage">Damage value of the new class</param>
        public Class(string name, string description, int defaultHealth, int defaultProtection, int defaultDamage)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;
            DefaultHealth = defaultHealth;
            DefaultProtection = defaultProtection;
            DefaultDamage = defaultDamage;

            Status = Status.Pending;
        }

        private ClassInventory _StartInventory;

        /// <summary>
        /// The start inventory every avatar with this class has right after creation
        /// </summary>
        public virtual ClassInventory StartInventory => _StartInventory ??= new ClassInventory();

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
        /// Dungeon the class belongs to
        /// </summary>
        public virtual Dungeon Dungeon { get; set; }

        private ICollection<Avatar> _Avatars;

        /// <summary>
        /// Avatars with this class
        /// </summary>
        public virtual ICollection<Avatar> Avatars => _Avatars ??= new List<Avatar>();
    }
}

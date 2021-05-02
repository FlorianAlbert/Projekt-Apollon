using Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using System;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Class
{
    /// <inheritdoc cref="IClass"/>
    public class Class : IClass
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

        private IInventory _StartInventory;

        /// <inheritdoc cref="IClass.StartInventory"/>
        public IInventory StartInventory => _StartInventory ??= new Inventory();

        /// <inheritdoc cref="IInspectable.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="IInspectable.Description"/>
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

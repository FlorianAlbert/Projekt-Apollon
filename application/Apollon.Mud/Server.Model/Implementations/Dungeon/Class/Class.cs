using Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Class;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Class
{
    public class Class : IClass
    {
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
        public IInventory StartInventory 
        { 
            get => _StartInventory ??= new Inventory(); 
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

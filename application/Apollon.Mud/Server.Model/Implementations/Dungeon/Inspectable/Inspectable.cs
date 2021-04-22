using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Inspectable
{
    public class Inspectable : IInspectable
    {
        public Inspectable(string description, string name)
        {
            Id = Guid.NewGuid();

            Name = name;
            Description = description;

            Status = Status.Pending;
        }

        public string Description { get; set; }
        public string Name { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }
    }
}

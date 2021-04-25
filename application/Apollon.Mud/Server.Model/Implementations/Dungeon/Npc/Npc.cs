using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Npc;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Npc
{
    public class Npc : INpc
    {
        public Npc(string text, string description, string name)
        {
            Id = Guid.NewGuid();

            Text = text;
            Description = description;
            Name = name;

            Status = Status.Pending;

            
        }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }
    }
}

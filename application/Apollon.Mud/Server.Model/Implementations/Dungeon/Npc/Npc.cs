using Apollon.Mud.Server.Model.Interfaces;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Npc;
using System;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Npc
{
    /// <inheritdoc cref="INpc"/>
    public class Npc : INpc
    {
        /// <summary>
        /// Creates new instance of Npc
        /// </summary>
        /// <param name="text">Returning text of the new NPC</param>
        /// <param name="description">Description of the new NPC</param>
        /// <param name="name">Name of the new NPC</param>
        public Npc(string text, string description, string name)
        {
            Id = Guid.NewGuid();

            Text = text;
            Description = description;
            Name = name;

            Status = Status.Pending;
        }

        /// <inheritdoc cref="INpc.Text"/>
        public string Text { get; set; }

        /// <inheritdoc cref="IInspectable.Description"/>
        public string Description { get; set; }

        /// <inheritdoc cref="IInspectable.Name"/>
        public string Name { get; set; }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }
    }
}

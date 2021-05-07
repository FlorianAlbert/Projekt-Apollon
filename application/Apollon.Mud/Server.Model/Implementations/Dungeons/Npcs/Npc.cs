using System;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Npcs
{
    /// <summary>
    /// Describes a NPC an avatar can interact with
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Npc : Inspectable
    {
        /// <summary>
        /// Creates new instance of Npc
        /// </summary>
        /// <param name="text">Returning text of the new NPC</param>
        /// <param name="description">Description of the new NPC</param>
        /// <param name="name">Name of the new NPC</param>
        public Npc(string text, string description, string name)
            : base(description,name)
        {

            Text = text;

        }

        /// <summary>
        /// The intial text the NPC answers
        /// </summary>
        public string Text { get; set; }
    }
}

using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Npc
{
    /// <summary>
    /// Describes a NPC an avatar can interact with
    /// </summary>
    public interface INpc : IInspectable
    {
        /// <summary>
        /// The intial text the NPC answers
        /// </summary>
        string Text { get; set; }
    }
}

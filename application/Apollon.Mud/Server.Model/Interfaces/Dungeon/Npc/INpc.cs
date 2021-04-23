using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Npc
{
    public interface INpc : IInspectable
    {
        string Text { get; set; }

        // TODO: Nicht vergessen zu verschieben oder löschen
        //string Speak();
    }
}

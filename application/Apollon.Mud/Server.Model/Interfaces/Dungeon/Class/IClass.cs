using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Class
{
    public interface IClass : IChoosable
    {
        IInventory StartInventory { get; }

    }
}

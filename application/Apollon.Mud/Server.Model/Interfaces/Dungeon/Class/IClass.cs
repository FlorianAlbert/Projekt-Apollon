using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Class
{
    /// <summary>
    /// Describes a class an avatar can be part of
    /// </summary>
    public interface IClass : IChoosable
    {
        /// <summary>
        /// The start inventory every avatar with this class has right after creation
        /// </summary>
        IInventory StartInventory { get; }

    }
}

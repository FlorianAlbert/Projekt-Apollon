using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Class
{
    public interface IClass : IChoosable
    {
        ICollection<ITakeable> StartInventory { get; }

    }
}

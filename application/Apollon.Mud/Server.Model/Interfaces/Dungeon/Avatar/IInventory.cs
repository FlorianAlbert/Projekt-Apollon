using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar
{
    public interface IInventory : ICollection<ITakeable>
    {
        ITakeable FirstOrDefault(Func<ITakeable, bool> predicate);

        int WeightSum { get; }
    }
}

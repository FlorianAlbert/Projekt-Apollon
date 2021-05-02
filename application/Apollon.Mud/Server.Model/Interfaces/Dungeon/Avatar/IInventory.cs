using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar
{
    /// <summary>
    /// An inventory which contains takeables
    /// </summary>
    public interface IInventory : ICollection<ITakeable>
    {
        /// <summary>
        /// Gets the first or default item in the inventory, that matches the <paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">The condition the result item has to fulfill</param>
        /// <returns>The first matching item</returns>
        ITakeable FirstOrDefault(Func<ITakeable, bool> predicate);

        /// <summary>
        /// weight sum of all of the items in the inventory
        /// </summary>
        int WeightSum { get; }
    }
}

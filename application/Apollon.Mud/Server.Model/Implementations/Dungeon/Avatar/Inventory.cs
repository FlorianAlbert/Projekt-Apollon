using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar
{
    public class Inventory : IInventory
    {
        // TODO: diskutieren ob Wert realistisch
        private const int _MaxWeight = 100;

        private ICollection<ITakeable> _Items;

        private ICollection<ITakeable> Items => _Items ??= new List<ITakeable>();

        public int WeightSum => Items.Sum(takeable => takeable.Weight);

        public int Count => Items.Count;

        public bool IsReadOnly => false;

        public void Add(ITakeable item)
        {
            if (item != null && WeightSum + item.Weight <= _MaxWeight)
                Items.Add(item);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(ITakeable item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(ITakeable[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ITakeable> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public bool Remove(ITakeable item)
        {
            return Items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public ITakeable FirstOrDefault(Func<ITakeable, bool> predicate)
        {
            return Items.FirstOrDefault(predicate);
        }
    }
}

using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Avatar
{
    public class Inventory : ICollection<ITakeable>
    {
        // TODO: diskutieren ob Wert realistisch
        private const int _MaxWeight = 100;

        private List<ITakeable> _Items;

        private List<ITakeable> Items
        {
            get => _Items ??= new List<ITakeable>();
        }

        public int Count => Items.Count;

        public bool IsReadOnly => false;

        public void Add(ITakeable item)
        {
            if (item != null && Items.Sum(takeable => takeable.Weight) + item.Weight <= _MaxWeight)
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
    }
}

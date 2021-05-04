using Apollon.Mud.Server.Model.Interfaces.Dungeon.Avatar;
using Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars
{
    /// <inheritdoc cref="IInventory"/>
    public class Inventory : IInventory
    {
        // TODO: diskutieren ob Wert realistisch
        private const int _MaxWeight = 100;

        private ICollection<ITakeable> _Items;

        private ICollection<ITakeable> Items => _Items ??= new List<ITakeable>();

        /// <inheritdoc cref="IInventory.WeightSum"/>
        public int WeightSum => Items.Sum(takeable => takeable.Weight);

        /// <inheritdoc cref="ICollection{ITakeable}.Count"/>
        public int Count => Items.Count;

        /// <inheritdoc cref="ICollection{ITakeable}.IsReadOnly"/>
        public bool IsReadOnly => false;

        /// <inheritdoc cref="ICollection{ITakeable}.Add"/>
        public void Add(ITakeable item)
        {
            if (item != null && WeightSum + item.Weight <= _MaxWeight)
                Items.Add(item);
        }

        /// <inheritdoc cref="ICollection{ITakeable}.Clear"/>
        public void Clear()
        {
            Items.Clear();
        }

        /// <inheritdoc cref="ICollection{ITakeable}.Contains"/>
        public bool Contains(ITakeable item)
        {
            return Items.Contains(item);
        }

        /// <inheritdoc cref="ICollection{ITakeable}.CopyTo"/>
        public void CopyTo(ITakeable[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="IEnumerable{ITakeable}.GetEnumerator"/>
        public IEnumerator<ITakeable> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <inheritdoc cref="ICollection{ITakeable}.Remove"/>
        public bool Remove(ITakeable item)
        {
            return Items.Remove(item);
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <inheritdoc cref="IInventory.FirstOrDefault"/>
        public ITakeable FirstOrDefault(Func<ITakeable, bool> predicate)
        {
            return Items.FirstOrDefault(predicate);
        }
    }
}

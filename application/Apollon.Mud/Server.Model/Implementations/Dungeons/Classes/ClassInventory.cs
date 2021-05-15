using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Shared.Implementations.Dungeons;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Classes
{
    public class ClassInventory : ICollection<ClassTakeable>
    {
        private const int _MaxWeight = 100;

        private ICollection<ClassTakeable> _Items;

        private ICollection<ClassTakeable> Items => _Items ??= new List<ClassTakeable>();

        public ClassInventory() { }

        /// <summary>
        /// Adds all possible Takeables while the WeightSum of the Inventory is less then _MaxWeight.
        /// </summary>
        /// <param name="takeables"></param>
        public ClassInventory(IEnumerable<ClassTakeable> takeables)
        {
            foreach (var takeable in takeables)
            {
                Add(takeable);
            }
        }

        /// <summary>
        /// weight sum of all of the items in the inventory
        /// </summary>
        public int WeightSum => Items.Where(x => x.Takeable.Status is Status.Approved).Sum(takeable => takeable.Takeable.Weight);

        /// <inheritdoc cref="ICollection{ClassTakeable}.Count"/>
        [ExcludeFromCodeCoverage]
        public int Count => Items.Count;

        /// <inheritdoc cref="ICollection{ClassTakeable}.IsReadOnly"/>
        [ExcludeFromCodeCoverage]
        public bool IsReadOnly => false;

        /// <inheritdoc cref="ICollection{ClassTakeable}.Add"/>
        public void Add(ClassTakeable item)
        {
            if (item != null && (item.Takeable.Status is Status.Pending || item.Takeable.Status is Status.Approved && WeightSum + item.Takeable.Weight <= _MaxWeight))
                Items.Add(item);
        }

        /// <inheritdoc cref="ICollection{ClassTakeable}.Clear"/>
        [ExcludeFromCodeCoverage]
        public void Clear()
        {
            Items.Clear();
        }

        /// <inheritdoc cref="ICollection{ClassTakeable}.Contains"/>
        [ExcludeFromCodeCoverage]
        public bool Contains(ClassTakeable item)
        {
            return Items.Contains(item);
        }

        /// <inheritdoc cref="ICollection{ClassTakeable}.CopyTo"/>
        [ExcludeFromCodeCoverage]
        public void CopyTo(ClassTakeable[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="IEnumerable{ClassTakeable}.GetEnumerator"/>
        [ExcludeFromCodeCoverage]
        public IEnumerator<ClassTakeable> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <inheritdoc cref="ICollection{ClassTakeable}.Remove"/>
        [ExcludeFromCodeCoverage]
        public bool Remove(ClassTakeable item)
        {
            return Items.Remove(item);
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Gets the first or default item in the inventory, that matches the <paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">The condition the result item has to fulfill</param>
        /// <returns>The first matching item</returns>
        [ExcludeFromCodeCoverage]
        public ClassTakeable FirstOrDefault(Func<ClassTakeable, bool> predicate)
        {
            return Items.FirstOrDefault(predicate);
        }
    }
}
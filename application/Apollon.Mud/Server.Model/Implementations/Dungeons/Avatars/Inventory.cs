using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars
{
    /// <summary>
    /// An inventory which contains takeables
    /// </summary>
    public class Inventory : ICollection<Takeable>
    {
        // TODO: diskutieren ob Wert realistisch
        private const int _MaxWeight = 100;

        private ICollection<Takeable> _Items;

        private ICollection<Takeable> Items => _Items ??= new List<Takeable>();

        public Inventory(){}

        /// <summary>
        /// Adds all possible Takeables while the WeightSum of the Inventory is less then _MaxWeight.
        /// </summary>
        /// <param name="takeables"></param>
        public Inventory(IEnumerable<Takeable> takeables):base()
        {
            foreach (var takeable in takeables)
            {
                Add(takeable);
            }
        }

        /// <summary>
        /// weight sum of all of the items in the inventory
        /// </summary>
        public int WeightSum => Items.Sum(takeable => takeable.Weight);

        /// <inheritdoc cref="ICollection{Takeable}.Count"/>
        [ExcludeFromCodeCoverage]
        public int Count => Items.Count;

        /// <inheritdoc cref="ICollection{Takeable}.IsReadOnly"/>
        [ExcludeFromCodeCoverage]
        public bool IsReadOnly => false;

        /// <inheritdoc cref="ICollection{Takeable}.Add"/>
        public void Add(Takeable item)
        {
            if (item != null && WeightSum + item.Weight <= _MaxWeight)
                Items.Add(item);
        }

        /// <inheritdoc cref="ICollection{Takeable}.Clear"/>
        [ExcludeFromCodeCoverage]
        public void Clear()
        {
            Items.Clear();
        }

        /// <inheritdoc cref="ICollection{Takeable}.Contains"/>
        [ExcludeFromCodeCoverage]
        public bool Contains(Takeable item)
        {
            return Items.Contains(item);
        }

        /// <inheritdoc cref="ICollection{Takeable}.CopyTo"/>
        [ExcludeFromCodeCoverage]
        public void CopyTo(Takeable[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="IEnumerable{Takeable}.GetEnumerator"/>
        [ExcludeFromCodeCoverage]
        public IEnumerator<Takeable> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <inheritdoc cref="ICollection{Takeable}.Remove"/>
        [ExcludeFromCodeCoverage]
        public bool Remove(Takeable item)
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
        public Takeable FirstOrDefault(Func<Takeable, bool> predicate)
        {
            return Items.FirstOrDefault(predicate);
        }
    }
}

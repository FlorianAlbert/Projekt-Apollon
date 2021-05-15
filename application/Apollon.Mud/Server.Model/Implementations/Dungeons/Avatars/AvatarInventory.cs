using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables;
using Apollon.Mud.Shared.Implementations.Dungeons;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars
{
    /// <summary>
    /// An inventory which contains takeables
    /// </summary>
    public class AvatarInventory : ICollection<AvatarTakeable>
    {
        private const int _MaxWeight = 100;

        private ICollection<AvatarTakeable> _Items;

        private ICollection<AvatarTakeable> Items => _Items ??= new List<AvatarTakeable>();

        public AvatarInventory(){}

        /// <summary>
        /// Adds all possible Takeables while the WeightSum of the Inventory is less then _MaxWeight.
        /// </summary>
        /// <param name="takeables"></param>
        public AvatarInventory(IEnumerable<AvatarTakeable> takeables)
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

        /// <inheritdoc cref="ICollection{AvatarTakeable}.Count"/>
        [ExcludeFromCodeCoverage]
        public int Count => Items.Count;

        /// <inheritdoc cref="ICollection{AvatarTakeable}.IsReadOnly"/>
        [ExcludeFromCodeCoverage]
        public bool IsReadOnly => false;

        /// <inheritdoc cref="ICollection{AvatarTakeable}.Add"/>
        public void Add(AvatarTakeable item)
        {
            if (item != null && (item.Takeable.Status is Status.Pending || item.Takeable.Status is Status.Approved && WeightSum + item.Takeable.Weight <= _MaxWeight))
                Items.Add(item);
        }

        /// <inheritdoc cref="ICollection{AvatarTakeable}.Clear"/>
        [ExcludeFromCodeCoverage]
        public void Clear()
        {
            Items.Clear();
        }

        /// <inheritdoc cref="ICollection{AvatarTakeable}.Contains"/>
        [ExcludeFromCodeCoverage]
        public bool Contains(AvatarTakeable item)
        {
            return Items.Contains(item);
        }

        /// <inheritdoc cref="ICollection{AvatarTakeable}.CopyTo"/>
        [ExcludeFromCodeCoverage]
        public void CopyTo(AvatarTakeable[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc cref="IEnumerable{AvatarTakeable}.GetEnumerator"/>
        [ExcludeFromCodeCoverage]
        public IEnumerator<AvatarTakeable> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <inheritdoc cref="ICollection{AvatarTakeable}.Remove"/>
        [ExcludeFromCodeCoverage]
        public bool Remove(AvatarTakeable item)
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
        public AvatarTakeable FirstOrDefault(Func<AvatarTakeable, bool> predicate)
        {
            return Items.FirstOrDefault(predicate);
        }
    }
}

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables
{
    /// <summary>
    /// Describes an item that can be taken by an avatar
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Takeable : Inspectable
    {
        /// <summary>
        /// Creates a new instance of Takeable
        /// </summary>
        /// <param name="weight">Weight of the new takeable</param>
        /// <param name="description">Description of the new takeable</param>
        /// <param name="name">Name of the new takeable</param>
        public Takeable(int weight, string description, string name)
            : base(description,name)
        {

            Weight = weight;

        }

        /// <summary>
        /// The weight of the item
        /// </summary>
        public int Weight { get; set; }

        private ICollection<AvatarTakeable> _InventoryAvatars;

        /// <summary>
        /// The avatars whose inventory contain this takeable
        /// </summary>
        public virtual ICollection<AvatarTakeable> InventoryAvatars => _InventoryAvatars ??= new List<AvatarTakeable>();

        private ICollection<Avatar> _HoldingItemAvatars;

        /// <summary>
        /// The avatars who are holding this takeable
        /// </summary>
        public virtual ICollection<Avatar> HoldingItemAvatars => _HoldingItemAvatars ??= new List<Avatar>();

        private ICollection<ClassTakeable> _Classes;

        /// <summary>
        /// The classes whose StartInventory contain this takeable
        /// </summary>
        public virtual ICollection<ClassTakeable> Classes => _Classes ??= new List<ClassTakeable>();
    }
}

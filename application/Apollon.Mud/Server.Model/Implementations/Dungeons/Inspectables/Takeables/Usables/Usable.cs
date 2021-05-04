using System;
using Apollon.Mud.Server.Model.Interfaces;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Usables
{
    /// <summary>
    /// Describes an item that can be used as a weapon
    /// </summary>
    public class Usable : Takeable
    {
        /// <summary>
        /// Creates a new instance of Usable
        /// </summary>
        /// <param name="name">Name of the new usable</param>
        /// <param name="description">Description of the new usable</param>
        /// <param name="weight">Weight of the new usable</param>
        /// <param name="damageBoost">Damage Boost of the new usable</param>
        public Usable(string name, string description, int weight, int damageBoost)
            : base(weight,description,name)
        {

            DamageBoost = damageBoost;

        }

        /// <summary>
        /// The damage boost the item gives
        /// </summary>
        public int DamageBoost { get; set; }
    }
}

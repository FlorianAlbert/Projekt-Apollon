using System;
using System.Diagnostics.CodeAnalysis;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Consumables
{
    /// <summary>
    /// Describes an item that can be consumed
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Consumable : Takeable
    {
        /// <summary>
        /// Creates a new instance of Consumable
        /// </summary>
        /// <param name="name">Name of the new consumable</param>
        /// <param name="description">Description of the new consumable</param>
        /// <param name="weight">Weight of the new Consumable</param>
        /// <param name="effectDescription">Effect description of the new consumable</param>
        public Consumable(string name, string description, int weight, string effectDescription)
            : base(weight,description,name)
        {

            EffectDescription = effectDescription;

        }

        /// <summary>
        /// A description of the effect the consumation has
        /// </summary>
        public string EffectDescription { get; set; }
    }
}

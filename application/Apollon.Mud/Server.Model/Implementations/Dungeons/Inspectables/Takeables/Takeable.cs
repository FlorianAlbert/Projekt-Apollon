using System;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Interfaces;

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
    }
}

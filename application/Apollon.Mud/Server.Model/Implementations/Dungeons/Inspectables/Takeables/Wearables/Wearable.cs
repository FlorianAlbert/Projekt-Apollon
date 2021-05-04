using Apollon.Mud.Server.Model.Interfaces;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables.Wearables
{
    /// <summary>
    /// Describes an item that can be worn
    /// </summary>
    public class Wearable : Takeable
    {
        /// <summary>
        /// Creates a new instance of Wearable
        /// </summary>
        /// <param name="name">Name of the new wearable</param>
        /// <param name="description">Description of the new wearable</param>
        /// <param name="weight">Weight of the new wearable</param>
        /// <param name="protectionBoost">Protection boost of the new wearable</param>
        public Wearable(string name, string description, int weight, int protectionBoost)
            : base(weight,description,name)
        {

            ProtectionBoost = protectionBoost;

        }

        /// <summary>
        /// The protection boost the item gives if it is worn
        /// </summary>
        public int ProtectionBoost { get; set; }
    }
}

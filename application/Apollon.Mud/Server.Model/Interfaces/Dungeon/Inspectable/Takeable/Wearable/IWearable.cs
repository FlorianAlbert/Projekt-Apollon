namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Wearable
{
    /// <summary>
    /// Describes an item that can be worn
    /// </summary>
    public interface IWearable : ITakeable
    {
        /// <summary>
        /// The protection boost the item gives if it is worn
        /// </summary>
        int ProtectionBoost { get; set; }
    }
}

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Usable
{
    /// <summary>
    /// Describes an item that can be used as a weapon
    /// </summary>
    public interface IUsable : ITakeable
    {
        /// <summary>
        /// The damage boost the item gives
        /// </summary>
        int DamageBoost { get; set; }
    }
}

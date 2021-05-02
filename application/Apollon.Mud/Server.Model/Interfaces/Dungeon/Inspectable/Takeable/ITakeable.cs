namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable
{
    /// <summary>
    /// Describes an item that can be taken by an avatar
    /// </summary>
    public interface ITakeable : IInspectable
    {
        /// <summary>
        /// The weight of the item
        /// </summary>
        int Weight { get; set; }
    }
}

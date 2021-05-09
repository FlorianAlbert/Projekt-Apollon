namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable
{
    /// <summary>
    /// Describes an item that can be inspected by an avatar
    /// </summary>
    public interface IInspectable : IApprovable
    {
        /// <summary>
        /// The description of the item
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The name of the item
        /// </summary>
        string Name { get; set; }
    }
}

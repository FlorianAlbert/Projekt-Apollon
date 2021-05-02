namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Consumable
{
    /// <summary>
    /// Describes an item that can be consumed
    /// </summary>
    public interface IConsumable : ITakeable
    {
        /// <summary>
        /// A description of the effect the consumation has
        /// </summary>
        string EffectDescription { get; set; }
    }
}

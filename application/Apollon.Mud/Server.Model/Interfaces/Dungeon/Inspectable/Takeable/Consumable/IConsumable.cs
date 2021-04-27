namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Consumable
{
    public interface IConsumable : ITakeable
    {
        string EffectDescription { get; set; }
    }
}

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Usable
{
    public interface IUsable : ITakeable
    {
        int DamageBoost { get; set; }
    }
}

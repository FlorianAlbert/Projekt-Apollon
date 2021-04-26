namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable.Wearable
{
    public interface IWearable : ITakeable
    {
        int ProtectionBoost { get; set; }
    }
}

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable
{
    public interface IInspectable : IApprovable
    {
        string Description { get; set; }

        string Name { get; set; }
    }
}

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable
{
    public interface IRequestable : IApprovable
    {
        string Message { get; set; }
    }
}

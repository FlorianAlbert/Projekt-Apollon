namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable
{
    public interface IRequestable : IApprovable
    {
        string MessageRegex { get; set; }

        string PatternForPlayer { get; set; }
    }
}

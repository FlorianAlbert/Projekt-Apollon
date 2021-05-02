namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable
{
    /// <summary>
    /// Describes a special action that can be defined by the Dungeon Master and which will be forwarded to him
    /// </summary>
    public interface IRequestable : IApprovable
    {
        /// <summary>
        /// The regular expression to check the avatar input on
        /// </summary>
        string MessageRegex { get; set; }

        /// <summary>
        /// A readable command pattern for the player help
        /// </summary>
        string PatternForPlayer { get; set; }
    }
}

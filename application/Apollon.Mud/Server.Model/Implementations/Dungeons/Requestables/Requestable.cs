using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using System;
using Apollon.Mud.Server.Model.Interfaces;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables
{
    /// <inheritdoc cref="IRequestable"/>
    public class Requestable : IRequestable
    {
        /// <summary>
        /// Creates a new instance of Requestable
        /// </summary>
        /// <param name="messageRegex">Regular expression of the special action</param>
        /// <param name="patternForPlayer">Readable pattern for the player</param>
        public Requestable(string messageRegex, string patternForPlayer)
        {
            Id = Guid.NewGuid();

            MessageRegex = messageRegex;
            PatternForPlayer = patternForPlayer;

            Status = Status.Pending;
        }

        /// <inheritdoc cref="IApprovable.Id"/>
        public Guid Id { get; }

        /// <inheritdoc cref="IApprovable.Status"/>
        public Status Status { get; set; }

        /// <inheritdoc cref="IRequestable.MessageRegex"/>
        public string MessageRegex { get; set; }

        /// <inheritdoc cref="IRequestable.PatternForPlayer"/>
        public string PatternForPlayer { get; set; }
    }
}

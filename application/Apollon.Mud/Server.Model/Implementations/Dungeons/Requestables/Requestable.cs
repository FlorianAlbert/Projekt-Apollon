using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;
using Apollon.Mud.Server.Model.Interfaces;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Requestables
{
    /// <summary>
    /// Describes a special action that can be defined by the Dungeon Master and which will be forwarded to him
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Requestable : IApprovable
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

        /// <summary>
        /// The regular expression to check the avatar input on
        /// </summary>
        public string MessageRegex { get; set; }

        /// <summary>
        /// A readable command pattern for the player help
        /// </summary>
        public string PatternForPlayer { get; set; }

        /// <summary>
        /// Dungeon the requestable belongs to
        /// </summary>
        public virtual Dungeon Dungeon { get; set; }

        /// <summary>
        /// Rooms with this requestable
        /// </summary>
        public virtual ICollection<Room> Rooms { get; set; }
    }
}

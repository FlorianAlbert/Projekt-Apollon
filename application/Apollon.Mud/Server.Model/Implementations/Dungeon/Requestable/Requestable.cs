using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Requestable
{
    public class Requestable : IRequestable
    {
        public Requestable(string messageRegex, string patternForPlayer)
        {
            Id = Guid.NewGuid();

            MessageRegex = messageRegex;
            PatternForPlayer = patternForPlayer;

            Status = Status.Pending;
        }

        public Guid Id { get; }

        public Status Status { get; set; }
        public string MessageRegex { get; set; }
        public string PatternForPlayer { get; set; }
    }
}

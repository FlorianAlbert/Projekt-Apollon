using Apollon.Mud.Server.Model.Interfaces.Dungeon.Requestable;
using System;

namespace Apollon.Mud.Server.Model.Implementations.Dungeon.Requestable
{
    public class Requestable : IRequestable
    {
        public Requestable(string message)
        {
            Id = Guid.NewGuid();

            message = Message;

            Status = Status.Pending;
        }
        public string Message { get; set; }

        public Guid Id { get; }

        public Status Status { get; set; }
    }
}

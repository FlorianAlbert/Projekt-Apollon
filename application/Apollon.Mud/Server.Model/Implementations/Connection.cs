using System;

namespace Apollon.Mud.Server.Model.Implementations
{
    public class Connection
    {
        public Guid DungeonId { get; set; }

        public string ChatConnectionId { get; set; }

        public string GameConnectionId { get; set; }

        public Guid? AvatarId { get; set; }
    }
}
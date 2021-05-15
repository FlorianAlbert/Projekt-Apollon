using System;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Rooms;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables
{
    public class RoomInspectable
    {
        public virtual Room Room { get; set; }
        
        public virtual Inspectable Inspectable { get; set; }

        public Guid Id { get; set; }
    }
}
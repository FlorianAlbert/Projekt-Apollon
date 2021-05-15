using System;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Avatars;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables
{
    public class AvatarTakeable
    {
        public virtual Avatar Avatar { get; set; }

        public virtual Takeable Takeable { get; set; }

        public Guid Id { get; set; }
    }
}
using System;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Classes;

namespace Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables.Takeables
{
    public class ClassTakeable
    {
        public virtual Class Class { get; set; }

        public virtual Takeable Takeable { get; set; }

        public Guid Id { get; set; }
    }
}
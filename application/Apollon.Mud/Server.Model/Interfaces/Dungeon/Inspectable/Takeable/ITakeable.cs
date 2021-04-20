using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Model.Interfaces.Dungeon.Inspectable.Takeable
{
    public interface ITakeable : IInspectable
    {
        short Weight { get; set; }
    }
}

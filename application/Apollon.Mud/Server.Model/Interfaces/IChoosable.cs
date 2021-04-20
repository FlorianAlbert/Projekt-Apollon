using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Model.Interfaces
{
    public interface IChoosable : IApprovable
    {
        string Name { get; set; }

        string Description { get; set; }

        int DefaultHealth { get; set; }

        int DefaultProtection { get; set; }

        int DefaultDamage { get; set; }
    }
}

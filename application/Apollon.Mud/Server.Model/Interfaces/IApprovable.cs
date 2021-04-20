using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Model.Interfaces
{
    public interface IApprovable
    {
        Guid Id { get; }

        Status Status { get; set; }
    }
}

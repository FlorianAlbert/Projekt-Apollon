using Apollon.Mud.Server.Model.Implementations;
using System;

namespace Apollon.Mud.Server.Model.Interfaces
{
    public interface IApprovable
    {
        Guid Id { get; }

        Status Status { get; set; }
    }
}

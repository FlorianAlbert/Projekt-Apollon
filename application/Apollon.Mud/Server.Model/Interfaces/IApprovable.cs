using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Shared.Implementations.Dungeons;
using System;

namespace Apollon.Mud.Server.Model.Interfaces
{
    /// <summary>
    /// Model element that can get turned on and off
    /// </summary>
    public interface IApprovable
    {
        /// <summary>
        /// Id of the element
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Status wether turned on or turned off
        /// </summary>
        Status Status { get; set; }
    }
}

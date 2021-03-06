using System;
using System.Diagnostics.CodeAnalysis;

namespace Apollon.Mud.Shared
{
    /// <summary>
    /// Dto to submit a Dungeon Enter Request
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SubmitDungeonEnterRequestDto
    {
        /// <summary>
        /// UserId of the user that wants to access the dungeon
        /// </summary>
        public Guid RequestUserId { get; set; }

        /// <summary>
        /// Value wether access should get granted
        /// </summary>
        public bool GrantAccess { get; set; }
    }
}
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Server.Model.Implementations.User;

namespace Apollon.Mud.Server.Model.Implementations
{
    /// <summary>
    /// Result of login at the AuthorizationController
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoginResult
    {
        /// <summary>
        /// JwtToken for the user
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// User that logged in
        /// </summary>
        public DungeonUser User { get; set; }

        /// <summary>
        /// Wether the user is an Admin
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Status of the login
        /// </summary>
        public LoginResultStatus Status { get; set; }

    }
}
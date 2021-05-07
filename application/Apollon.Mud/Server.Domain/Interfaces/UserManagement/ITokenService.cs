using System;

namespace Apollon.Mud.Server.Domain.Interfaces.UserManagement
{
    /// <summary>
    /// Singleton service which stores the open confirmation and password-reset request-tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a new token for an email confirmation.
        /// </summary>
        /// <returns></returns>
        Guid GenerateNewConfirmationToken();

        /// <summary>
        /// Validates if the guid is the token for an open email confirmation.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        bool CheckConfirmationToken(Guid guid);

        /// <summary>
        /// Generates a new token for a password-reset.
        /// </summary>
        /// <returns></returns>
        Guid GenerateNewResetToken();

        /// <summary>
        /// Validates if the guid is the token of a open password-reset.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        bool CheckResetToken(Guid guid);
    }
}
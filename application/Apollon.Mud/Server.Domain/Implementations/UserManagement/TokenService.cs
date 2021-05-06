using System;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <summary>
    /// Singleton service which stores the open confirmation and password-reset request-tokens.
    /// </summary>
    public class TokenService
    {
        #region member
        /// <summary>
        /// The list of open confirmation-tokens.
        /// </summary>
        private readonly List<Guid> _openConfirmations = new List<Guid>();

        /// <summary>
        /// The list of open password-reset-tokens.
        /// </summary>
        private readonly List<Guid> _openResets = new List<Guid>();
        #endregion

        #region methods
        /// <summary>
        /// Generates a new token for an email confirmation.
        /// </summary>
        /// <returns></returns>
        public Guid GenerateNewConfirmationToken()
        {
            var guid = Guid.NewGuid();
            _openConfirmations.Add(guid);
            return guid;
        }

        /// <summary>
        /// Validates if the guid is the token for an open email confirmation.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool CheckConfirmationToken(Guid guid)
        {
            var res = _openConfirmations.Contains(guid);
            if (!res) return false;
            _openConfirmations.Remove(guid);
            return true;
        }

        /// <summary>
        /// Generates a new token for a password-reset.
        /// </summary>
        /// <returns></returns>
        public Guid GenerateNewResetToken()
        {
            var guid = Guid.NewGuid();
            _openResets.Add(guid);
            return guid;
        }

        /// <summary>
        /// Validates if the guid is the token of a open password-reset.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool CheckResetToken(Guid guid)
        {
            var res = _openResets.Contains(guid);
            if (!res) return false;
            _openResets.Remove(guid);
            return true;
        }
        #endregion
    }
}
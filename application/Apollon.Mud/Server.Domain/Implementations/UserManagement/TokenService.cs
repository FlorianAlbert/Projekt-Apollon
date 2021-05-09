using System;
using System.Collections.Generic;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <inheritdoc cref="ITokenService"/>
    public class TokenService : ITokenService
    {
        #region member
        /// <summary>
        /// The list of open confirmation-tokens.
        /// </summary>
        private readonly List<Guid> _openConfirmations = new();

        /// <summary>
        /// The list of open password-reset-tokens.
        /// </summary>
        private readonly List<Guid> _openResets = new();
        #endregion

        #region methods
        /// <inheritdoc cref="ITokenService.GenerateNewConfirmationToken"/>
        public Guid GenerateNewConfirmationToken()
        {
            var guid = Guid.NewGuid();
            _openConfirmations.Add(guid);
            return guid;
        }

        /// <inheritdoc cref="ITokenService.CheckConfirmationToken"/>
        public bool CheckConfirmationToken(Guid guid)
        {
            var res = _openConfirmations.Contains(guid);
            if (!res) return false;
            _openConfirmations.Remove(guid);
            return true;
        }

        /// <inheritdoc cref="ITokenService.GenerateNewResetToken"/>
        public Guid GenerateNewResetToken()
        {
            var guid = Guid.NewGuid();
            _openResets.Add(guid);
            return guid;
        }

        /// <inheritdoc cref="ITokenService.CheckResetToken"/>
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
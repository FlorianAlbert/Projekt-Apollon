using System;
using System.Collections.Generic;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    public class TokenService
    {
        private readonly List<Guid> _openConfirmations = new List<Guid>();
        private readonly List<Guid> _openResets = new List<Guid>();

        public Guid GenerateNewConfirmationToken()
        {
            var guid = Guid.NewGuid();
            _openConfirmations.Add(guid);
            return guid;
        }

        public bool CheckConfirmationToken(Guid guid)
        {
            var res = _openConfirmations.Contains(guid);
            if (!res) return false;
            _openConfirmations.Remove(guid);
            return true;
        }

        public Guid GenerateNewResetToken()
        {
            var guid = Guid.NewGuid();
            _openResets.Add(guid);
            return guid;
        }

        public bool CheckResetToken(Guid guid)
        {
            var res = _openResets.Contains(guid);
            if (!res) return false;
            _openResets.Remove(guid);
            return true;
        }
    }
}
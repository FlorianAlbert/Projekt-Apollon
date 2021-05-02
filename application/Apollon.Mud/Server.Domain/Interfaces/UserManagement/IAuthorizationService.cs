using System;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.User;

namespace Apollon.Mud.Server.Domain.Interfaces.UserManagement
{
    /// <summary>
    /// Service to validate the users credentials.
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        Task<LoginResult> Login(string email, string secret);
    }
}
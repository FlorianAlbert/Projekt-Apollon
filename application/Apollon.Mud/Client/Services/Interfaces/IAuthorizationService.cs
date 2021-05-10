using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// TODO
        /// </summary>
        HttpClient HttpClient { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        Task<bool> Login(string userId, string secret);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        Task<bool> Register(string userId, string secret);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ConfirmRegistration(Guid userId, string token);

        /// <summary>
        /// TODO
        /// </summary>
        void LogOut();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task<bool> RequestPasswordReset(string userEmail);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        Task<bool> ResetPassword(Guid userId, string token, string secret);
    }
}

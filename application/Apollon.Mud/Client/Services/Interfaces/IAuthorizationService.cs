using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// Interface for a service that provides Authorization
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// The HttpClient used by the service
        /// </summary>
        HttpClient HttpClient { get; }

        /// <summary>
        /// The CancellationTokenSource used by the service
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Signs a user in with his userId and Password
        /// </summary>
        /// <param name="userId">Email</param>
        /// <param name="secret">Password</param>
        /// <returns>True if successfull, otherwise false</returns>
        Task<bool> Login(string userId, string secret);

        /// <summary>
        /// Signs a user up with his userId and Password
        /// </summary>
        /// <param name="userId">Email</param>
        /// <param name="secret">Password</param>
        /// <returns>True if successfull, otherwise false</returns>
        Task<HttpStatusCode> Register(string userId, string secret);

        /// <summary>
        /// Confirms an users Email
        /// </summary>
        /// <param name="userId">The Unique User ID</param>
        /// <param name="token">The unique confirmation Token</param>
        /// <returns>True if successfull, otherwise false</returns>
        Task<bool> ConfirmRegistration(Guid userId, string token);

        /// <summary>
        /// Requests the reset of a password
        /// </summary>
        void LogOut();

        
        /// <param name="userEmail">The users Email connected with the Account</param>
        /// <returns>True if successfull, otherwise false</returns>
        Task<bool> RequestPasswordReset(string userEmail);

        /// <summary>
        /// Resets an users password
        /// </summary>
        /// <param name="userId">The unique user Id</param>
        /// <param name="token">The unique reset Code</param>
        /// <param name="secret">The new password</param>
        /// <returns>True if successfull, otherwise false</returns>
        Task<bool> ResetPassword(Guid userId, string token, string secret);
    }
}

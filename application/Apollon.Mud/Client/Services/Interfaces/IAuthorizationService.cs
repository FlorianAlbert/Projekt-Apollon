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
        /// Requests the reset of a password
        /// </summary>
        void LogOut();
    }
}

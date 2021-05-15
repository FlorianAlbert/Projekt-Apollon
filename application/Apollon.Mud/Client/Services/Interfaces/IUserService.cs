using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apollon.Mud.Shared.Dungeon.User;
using Apollon.Mud.Shared.UserManagement.Password;
using Apollon.Mud.Shared.UserManagement.Registration;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// Interface for a service that provides Authorization
    /// </summary>
    public interface IUserService
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
        /// Tries to register a use with the email and password from the RegistrationRequestDto.
        /// </summary>
        /// <param name="registrationRequestDto"></param>
        /// <returns></returns>
        Task<HttpStatusCode> RegistrateUser(string userId, string secret);

        /// <summary>
        /// Tries to confirm the user with the given userId and token.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ConfirmUserRegistration(Guid userId, string token);

        /// <summary>
        /// Deletes the user with the given userId. Can only be called from Admins.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid userId);

        /// <summary>
        /// Returns all registered users. Can only be called from Admins.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<DungeonUserDto>> GetAllUsers();

        /// <summary>
        /// Returns the user with the given userId. Can only be called from Admins.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DungeonUserDto> GetUser(Guid userId);

        /// <summary>
        /// Requests a password-reset for the user with the given email in the RequestPasswordResetDto.
        /// </summary>
        /// <param name="requestPasswordResetDto"></param>
        /// <returns></returns>
        Task<bool> RequestPasswordReset(string userEmail);

        /// <summary>
        /// Confirms the password-reset from the user with the given userId.
        /// </summary>
        /// <param name="passwortResetConfirmationDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> ConfirmPasswordReset(string password, string token, Guid userId);

        /// <summary>
        /// Changes the password from the user with the given userId.
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        Task<bool> ChangePassword(string oldPassword, string newPassword);

        /// <summary>
        /// Changes the Admin Role of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="approved"></param>
        /// <returns></returns>
        Task<bool> ChangeUserAdmin(Guid userId, bool approved);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Server.Model.Implementations.User;

namespace Apollon.Mud.Server.Domain.Interfaces.UserManagement
{
    /// <summary>
    /// Service which is responsible for all changes which are connected to an user, like registration, password-resets and so on.
    /// </summary>
    public interface IUserService 
    {
        /// <summary>
        /// Requests an user registration with the given email and password.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> RequestUserRegistration(string userEmail, string password);

        /// <summary>
        /// Confirms the user with the given id and uses the token for the confirmation.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ConfirmUserRegistration(Guid userId, string token);

        /// <summary>
        /// Deletes the user with the given id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid userId);

        /// <summary>
        /// Returns all users which are currently registered.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<DungeonUser>> GetAllUsers();

        /// <summary>
        /// Returns the user with the given id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DungeonUser> GetUser(Guid userId);

        /// <summary>
        /// Requests a password reset for the user with the given email.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task<bool> RequestPasswordReset(string userEmail);

        /// <summary>
        /// Confirms the password reset with the given token and sets the password to the new password.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="resetToken"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<bool> ConfirmPasswordReset(Guid userId, string resetToken, string newPassword);

        /// <summary>
        /// Changes the password of the user with the given userId to the new password.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<bool> ChangePassword(Guid userId, string oldPassword, string newPassword);
    }
}
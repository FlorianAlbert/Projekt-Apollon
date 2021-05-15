using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Server.Model.Implementations.User;

namespace Apollon.Mud.Server.Domain.Interfaces.UserManagement
{
    /// <summary>
    /// Service which is used for database-changing methods.
    /// </summary>
    public interface IUserDbService
    {
        /// <summary>
        /// Creates the user with the given password. If there is no admin currently registered, this user gets the admin role.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="asAdmin"></param>
        /// <returns></returns>
        Task<bool> CreateUser(DungeonUser user, string password, bool asAdmin = false);

        /// <summary>
        /// Returns the user with the given userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DungeonUser> GetUser(Guid userId);

        /// <summary>
        /// Returns all registered users.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<DungeonUser>> GetUsers();

        /// <summary>
        /// Updates the user and sets the password to the new password.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<bool> UpdateUser(DungeonUser user, string oldPassword, string newPassword);

        /// <summary>
        /// Deletes the user with the given id and all dungeons and avatars which he/her owns.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid userId);

        /// <summary>
        /// Returns the user with the given email.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task<DungeonUser> GetUserByEmail(string userEmail);

        /// <summary>
        /// Resets the password of the user with the given token and sets the password the the new password.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ResetPassword(DungeonUser user, string token, string password);

        /// <summary>
        /// Generates a token for the given user to reset his/her password.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetResetToken(DungeonUser user);

        /// <summary>
        /// Confirms the email of the user with the token.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ConfirmEmail(DungeonUser user, string token);

        /// <summary>
        /// Returns an email confirmation token for the given user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GetEmailConfirmationToken(DungeonUser user);

        /// <summary>
        /// Checks if an admin is currently registered.
        /// </summary>
        /// <returns></returns>
        Task<bool> IsAdminRegistered();

        /// <summary>
        /// Updates LastActive Timestamp of the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUserTimestamp(DungeonUser user);

        /// <summary>
        /// Checks if a user is in a special Role
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsUserInRole(string userId, string role);

        /// <summary>
        /// Returns all registered users in given role.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<DungeonUser>> GetUsersInRole(string role);

        /// <summary>
        /// Adds a user to given role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> AddUserToRole(string userId, string role);

        /// <summary>
        /// Removes a user from a given role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> RemoveUserFromRole(string userId, string role);
    }
}
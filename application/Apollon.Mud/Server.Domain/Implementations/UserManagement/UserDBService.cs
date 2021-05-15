using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <inheritdoc cref="IUserDbService"/>
    public class UserDbService: IUserDbService
    {
        #region member
        /// <summary>
        /// Manager to access and modify the content of the database.
        /// </summary>
        private readonly UserManager<DungeonUser> _userManager;
        #endregion


        public UserDbService(UserManager<DungeonUser> userManager)
        {
            _userManager = userManager;
        }

        #region methods
        /// <inheritdoc cref="IUserDbService.CreateUser"/>
        public async Task<bool> CreateUser(DungeonUser user, string password, bool asAdmin = false)
        {
            var creationSucceeded = await _userManager.CreateAsync(user, password);

            if (!creationSucceeded.Succeeded)
            {
                return false;
            }
            var addedToRoles = await _userManager.AddToRoleAsync(user, Roles.Player.ToString());

            if (!addedToRoles.Succeeded)
            {
                await RollbackUserCreation(user);
                return false;
            }

            if (asAdmin)
            {
                addedToRoles = await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                if (!addedToRoles.Succeeded)
                {
                    await RollbackUserCreation(user);
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc cref="IUserDbService.GetUser"/>
        [ExcludeFromCodeCoverage]
        public async Task<DungeonUser> GetUser(Guid userId)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        /// <inheritdoc cref="IUserDbService.GetUsers"/>
        [ExcludeFromCodeCoverage]
        public async Task<ICollection<DungeonUser>> GetUsers()
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.Users.ToListAsync();
        }

        /// <inheritdoc cref="IUserDbService.UpdateUser"/>
        [ExcludeFromCodeCoverage]
        public async Task<bool> UpdateUser(DungeonUser user, string oldPassword, string newPassword)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.DeleteUser"/>
        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.GetUserByEmail"/>
        [ExcludeFromCodeCoverage]
        public async Task<DungeonUser> GetUserByEmail(string userEmail)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.FindByEmailAsync(userEmail);
        }

        /// <inheritdoc cref="IUserDbService.ResetPassword"/>
        [ExcludeFromCodeCoverage]
        public async Task<bool> ResetPassword(DungeonUser user, string token, string password)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.GetResetToken"/>
        [ExcludeFromCodeCoverage]
        public async Task<string> GetResetToken(DungeonUser user)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        /// <inheritdoc cref="IUserDbService.ConfirmEmail"/>
        [ExcludeFromCodeCoverage]
        public async Task<bool> ConfirmEmail(DungeonUser user, string token)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.GetEmailConfirmationToken"/>
        [ExcludeFromCodeCoverage]
        public async Task<string> GetEmailConfirmationToken(DungeonUser user)
        {
            //Just pipes call to userManager and does not need a test for this "logic"
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        /// <inheritdoc cref="IUserDbService.IsAdminRegistered"/>
        public async Task<bool> IsAdminRegistered()
        {
            var admins = await _userManager.GetUsersInRoleAsync(Roles.Admin.ToString());
            return admins.Count > 0;
        }

        /// <summary>
        /// Undo the user creation.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal async Task RollbackUserCreation(DungeonUser user)
        {
            IdentityResult result;
            do
            {
                result = await _userManager.DeleteAsync(user);
            } while (!result.Succeeded);
        }

        /// <inheritdoc cref="IUserDbService.UpdateUserTimestamp"/>
        public async Task UpdateUserTimestamp(DungeonUser user)
        {
            user.LastActive = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
        }

        /// <inheritdoc cref="IUserDbService.IsUserInRole"/>
        public async Task<bool> IsUserInRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return false;

            return await _userManager.IsInRoleAsync(user, role);
        }

        /// <inheritdoc cref="IUserDbService.GetUsersInRole"/>
        public async Task<ICollection<DungeonUser>> GetUsersInRole(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }

        /// <inheritdoc cref="IUserDbService.AddUserToRole"/>
        public async Task<bool> AddUserToRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return false;

            var result = await _userManager.AddToRoleAsync(user, role);

            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.RemoveUserFromRole"/>
        public async Task<bool> RemoveUserFromRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, role);

            return result.Succeeded;
        }

        #endregion
    }
}
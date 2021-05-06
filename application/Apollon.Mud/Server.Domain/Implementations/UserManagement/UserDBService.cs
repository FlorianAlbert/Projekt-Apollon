using System;
using System.Collections.Generic;
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
            var creartionSucceeded = await _userManager.CreateAsync(user, password);

            if (!creartionSucceeded.Succeeded)
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
        public async Task<DungeonUser> GetUser(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        /// <inheritdoc cref="IUserDbService.GetUsers"/>
        public async Task<ICollection<DungeonUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        /// <inheritdoc cref="IUserDbService.UpdateUser"/>
        public async Task<bool> UpdateUser(DungeonUser user, string oldPassword, string newPassword)
        {
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
        public async Task<DungeonUser> GetUserByEmail(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail);
        }

        /// <inheritdoc cref="IUserDbService.ResetPassword"/>
        public async Task<bool> ResetPassword(DungeonUser user, string token, string password)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.GetResetToken"/>
        public async Task<string> GetResetToken(DungeonUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        /// <inheritdoc cref="IUserDbService.ConfirmEmail"/>
        public async Task<bool> ConfirmEmail(DungeonUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        /// <inheritdoc cref="IUserDbService.GetEmailConfirmationToken"/>
        public async Task<string> GetEmailConfirmationToken(DungeonUser user)
        {
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
        private async Task RollbackUserCreation(DungeonUser user) //ToDo in UML anpassen
        {
            IdentityResult result;
            do
            {
                result = await _userManager.DeleteAsync(user);
            } while (!result.Succeeded);
        }
        #endregion
    }
}
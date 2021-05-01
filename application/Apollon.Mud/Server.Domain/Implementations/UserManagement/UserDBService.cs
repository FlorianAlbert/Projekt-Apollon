using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class UserDBService: IUserDBService
    {
        //ToDo Tests

        /// <summary>
        /// ToDo
        /// </summary>
        private readonly UserManager<DungeonUser> _userManager;

        public UserDBService(UserManager<DungeonUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateUser(DungeonUser user, string password, bool asAdmin = false)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return false;
            }
            result = await _userManager.AddToRoleAsync(user, Roles.Player.ToString());

            if (!result.Succeeded)
            {
                await RollbackUserCreation(user);
                return false;
            }

            if (asAdmin)
            {
                result = await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                if (!result.Succeeded)
                {
                    await RollbackUserCreation(user);
                    return false;
                }
            }

            return true;
        }

        public async Task<DungeonUser> GetUser(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<ICollection<DungeonUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<bool> UpdateUser(DungeonUser user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<DungeonUser> GetUserByEmail(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail);
        }

        public async Task<bool> ResetPassword(DungeonUser user, string token, string password)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result.Succeeded;
        }

        public async  Task<string> GetResetToken(DungeonUser user)
        {
            //ToDo configure password reset token provider
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ConfirmEmail(DungeonUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<string> GetEmailConfirmationToken(DungeonUser user)
        {
            //ToDo TokenProvider konfigurieren?!
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<bool> IsAdminLoggedIn()
        {
            return await _userManager.Users.AnyAsync(x =>
                _userManager.GetRolesAsync(x).Result.Contains(Roles.Admin.ToString()));
        }

        private async Task RollbackUserCreation(DungeonUser user) //ToDo in UML anpassen
        {
            IdentityResult result;
            do
            {
                result = await _userManager.DeleteAsync(user);
            } while (!result.Succeeded);
        }
    }
}
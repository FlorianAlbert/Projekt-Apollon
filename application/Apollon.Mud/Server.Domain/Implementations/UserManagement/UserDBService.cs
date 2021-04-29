using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        //ToDo implement und Tests
        //ToDo Sollte das asynchron sein bzw. passt es auf Result zu warten?!

        /// <summary>
        /// ToDo
        /// </summary>
        private readonly UserManager<DungeonUser> _userManager;

        public UserDBService(UserManager<DungeonUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateUser(DungeonUser user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            return identityResult.Succeeded;
        }

        public async Task<DungeonUser> GetUser(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<ICollection<DungeonUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public bool UpdateUser(DungeonUser user, string oldPassword, string newPassword)
        {
            //ToDo passt das so?!
            return _userManager.ChangePasswordAsync(user, oldPassword, newPassword).Result.Succeeded;
        }

        public bool DeleteUser(Guid userId)
        {
            //ToDo wie funktioniert es mit GetUserAsync?!
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId.ToString());
            if (user == null) return false;
            return _userManager.DeleteAsync(user).Result.Succeeded;
        }

        public DungeonUser GetUserByEmail(string userEmail)
        {
            //ToDo wie funktioniert es mit GetUserAsync?!
            return _userManager.Users.FirstOrDefault(x => x.Email == userEmail);
        }
    }
}
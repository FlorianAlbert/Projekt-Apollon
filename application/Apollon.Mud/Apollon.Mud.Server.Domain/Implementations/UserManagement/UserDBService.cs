using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class UserDBService: IUserDBService
    {
        //ToDo implement und Tests

        /// <summary>
        /// ToDo
        /// </summary>
        private readonly UserManager<DungeonUser> _userManager;

        public UserDBService(UserManager<DungeonUser> userManager)
        {
            _userManager = userManager;
        }

        public bool CreateUser(DungeonUser user, string password)
        {
            //ToDo User wird ohne Passwort angelegt
            if (_userManager.CreateAsync(user).Result.Succeeded)
            {
                if (_userManager.AddPasswordAsync(user, password).Result.Succeeded)
                {
                    return true;
                }
                _userManager.DeleteAsync(user);
            }
            return false;
        }

        public DungeonUser GetUser(Guid userId)
        {
            //ToDo wie funktioniert es mit GetUserAsync?!
            return _userManager.Users.FirstOrDefault(x => x.Id == userId.ToString());
        }

        public ICollection<DungeonUser> GetUsers()
        {
            //ToDo passt das so?!
            return _userManager.Users.AsEnumerable() as ICollection<DungeonUser>;
        }

        public bool UpdateUser(DungeonUser user, string oldPassword, string newPassword)
        {
            //ToDo passt das so?!
            return _userManager.ChangePasswordAsync(user, oldPassword, newPassword).Result.Succeeded;
        }

        public bool DeleteUser(Guid userId)
        {
            //ToDo wie funktioniert es mit GetUserAsync?!
            //Sollte das asynchron sein bzw. passt es auf Result zu warten?!
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
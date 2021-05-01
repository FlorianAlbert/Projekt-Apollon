using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.DbContext;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Internal;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    public class UserService: IUserService
    {
        /// <summary>
        /// Service to send mails.
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Service to modify the user-db content.
        /// </summary>
        private readonly IUserDbService _userDbService;

        /// <summary>
        /// Service to modify the dungeon-db content.
        /// </summary>
        private readonly DungeonDbContext _dungeonDbContext;

        /// <summary>
        /// Flag which indicates if one administrator is registered.
        /// </summary>
        private readonly bool _adminRegistered;

        public UserService(IEmailService emailService, IUserDbService userDbService, DungeonDbContext dungeonDbContext)
        {
            _emailService = emailService;
            _userDbService = userDbService;
            _dungeonDbContext = dungeonDbContext;
            _adminRegistered = _userDbService.IsAdminLoggedIn().Result;
        }

        public async Task<bool> RequestUserRegistration(string userEmail, string password)
        {
            //ToDo wie wird ein ServiceUser angelegt?!
            var user = new DungeonUser()
            {
                Email = userEmail
            };
            var creationResult = await _userDbService.CreateUser(user, password, _adminRegistered);
            if (!creationResult) return false;
            var confirmationToken = await _userDbService.GetEmailConfirmationToken(user);
            //ToDo emailText und confirmationLink erstellen
            var emailText = "";
            await _emailService.SendEmail(userEmail, emailText, "Bestätigung Ihrer Email.");
            return true;
        }

        public async Task<bool> ConfirmUserRegistration(Guid userId, string token)
        {
            var user = await _userDbService.GetUser(userId);
            return await _userDbService.ConfirmEmail(user, token);
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            //ToDo werden auch alle abhängigen Daten beim Löschen eines Dungeons gelöscht?
            var usersAvatars = _dungeonDbContext.Avatars.Where(x => x.Id == userId);
            var usersDungeons = _dungeonDbContext.Dungeons.Where(x => x.DungeonOwner.Id == userId.ToString());
            //ToDo löschen von allen Black/White-Lists und DungeonMaster-Listen
            _dungeonDbContext.Avatars.RemoveRange(usersAvatars);
            _dungeonDbContext.Dungeons.RemoveRange(usersDungeons);
            return await _userDbService.DeleteUser(userId);
        }

        public async Task<ICollection<DungeonUser>> GetAllUsers()
        {
            return await _userDbService.GetUsers();
        }

        public async Task<DungeonUser> GetUser(Guid userId)
        {
            return await _userDbService.GetUser(userId);
        }

        public async Task<bool> RequestPasswordReset(string userEmail)
        {
            var user = await _userDbService.GetUserByEmail(userEmail);
            if (user == null) return false;
            var resetToken = await _userDbService.GetResetToken(user);
            //ToDo emailText und resetLink erstellen
            var emailText = "";
            await _emailService.SendEmail(userEmail, emailText, "Rücksetzung Ihres Passworts.");
            return true;
        }

        public async Task<bool> ConfirmPasswordReset(Guid userId, string token, string newPassword)
        {
            var user = await _userDbService.GetUser(userId);
            if (user == null) return false;
            return await _userDbService.ResetPassword(user, token, newPassword);
        }

        public async Task<bool> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userDbService.GetUser(userId);
            if (user == null) return false;
            return await _userDbService.UpdateUser(user, oldPassword, newPassword);
        }
    }
}
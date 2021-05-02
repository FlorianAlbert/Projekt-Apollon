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
        private bool _adminRegistered;

        public UserService(IEmailService emailService, IUserDbService userDbService, DungeonDbContext dungeonDbContext)
        {
            _emailService = emailService;
            _userDbService = userDbService;
            _dungeonDbContext = dungeonDbContext;
            _adminRegistered = _userDbService.IsAdminLoggedIn().Result;
        }

        public async Task<bool> RequestUserRegistration(string userEmail, string password)
        {
            var user = new DungeonUser()
            {
                Email = userEmail
            };
            _adminRegistered = await _userDbService.IsAdminLoggedIn();
            var creationResult = await _userDbService.CreateUser(user, password, !_adminRegistered);
            if (!creationResult) return false;
            var confirmationToken = await _userDbService.GetEmailConfirmationToken(user);
            var confirmationLink = $"http://mud.apollon-dungeons.de/Identity/Account/ConfirmAccount/{user.Id}/{confirmationToken}";
            var emailText = $"Hallo {user.UserName},\n\nbitte bestätige mit folgedem Link deine Email-Adresse:\n\n{confirmationLink}\n\n. Dein Apollon-Support-Team.";
            await _emailService.SendEmail(userEmail, emailText, "Bestätigung deiner Email.");
            return true;
        }

        public async Task<bool> ConfirmUserRegistration(Guid userId, string token)
        {
            var user = await _userDbService.GetUser(userId);
            return await _userDbService.ConfirmEmail(user, token);
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            //ToDo benutze DungeonDBService
            //ToDo löschen von allen Black/White-Lists und DungeonMaster-Listen --> cascading
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
            var resetLink = $"http://mud.apollon-dungeons.de/Identity/Account/ForgotPassword/{user.Id}/{resetToken}";
            var emailText = $"Hallo {user.UserName},\n\ndu hast das Zurücksetzen deines Passworts beantragt. Bitte folge dem nachstehenden " +
                            $"Link, um dein Passwort zurück zu setzen:\n\n{resetLink}\n\n. Dein Apollon-Support-Team.";
            await _emailService.SendEmail(userEmail, emailText, "Rücksetzung deines Passworts.");
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
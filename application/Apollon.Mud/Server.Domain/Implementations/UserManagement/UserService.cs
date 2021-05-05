using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.DbContext;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.User;


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
        /// ToDo
        /// </summary>
        private readonly TokenService _tokenService;

        /// <summary>
        /// Service to modify the dungeon-db content.
        /// </summary>
        private readonly IGameDbService _gameDbService;

        /// <summary>
        /// Flag which indicates if one administrator is registered.
        /// </summary>
        private bool _adminRegistered;

        public UserService(IEmailService emailService, IUserDbService userDbService, IGameDbService gameDbService, TokenService tokenService)
        {
            _emailService = emailService;
            _userDbService = userDbService;
            _gameDbService = gameDbService;
            _tokenService = tokenService;
            _adminRegistered = _userDbService.IsAdminLoggedIn().Result;
        }

        public async Task<bool> RequestUserRegistration(string userEmail, string password)
        {
            var user = new DungeonUser()
            {
                UserName = userEmail,
                Email = userEmail
            };
            var creationResult = await _userDbService.CreateUser(user, password, !_adminRegistered);
            if (!creationResult) return false;
            var token = _tokenService.GenerateNewConfirmationToken();
            var confirmationLink = $"http://mud.apollon-dungeons.de/Identity/Account/ConfirmAccount/{user.Id}/{token}";
            var emailText = $"Hallo {user.UserName},\n\nbitte bestätige mit folgendem Link deine Email-Adresse:\n\n{confirmationLink}\n\nDein Apollon-Support-Team.";
            await _emailService.SendEmail(userEmail, emailText, "Bestätigung deiner Email.");
            return true;
        }

        public async Task<bool> ConfirmUserRegistration(Guid userId, string token)
        {
            var confirmationToken = string.Empty;
            var user = await _userDbService.GetUser(userId);
            if (user == null) return false;
            if (!Guid.TryParse(token, out var guid)) return false;
            if (_tokenService.CheckConfirmationToken(guid))
            {
                confirmationToken = await _userDbService.GetEmailConfirmationToken(user);
            }
            return  await _userDbService.ConfirmEmail(user, confirmationToken);
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var deletedData= await _gameDbService.DeleteAllFromUser(userId);
            if(deletedData) return await _userDbService.DeleteUser(userId);
            return false;
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
            var resetToken = _tokenService.GenerateNewResetToken();
            var resetLink = $"http://mud.apollon-dungeons.de/Identity/Account/ForgotPassword/{user.Id}/{resetToken}";
            var emailText = $"Hallo {user.UserName},\n\ndu hast das Zurücksetzen deines Passworts beantragt. Bitte folge dem nachstehenden " +
                            $"Link, um dein Passwort zurück zu setzen:\n\n{resetLink}\n\n. Dein Apollon-Support-Team.";
            await _emailService.SendEmail(userEmail, emailText, "Rücksetzung deines Passworts.");
            return true;
        }

        public async Task<bool> ConfirmPasswordReset(Guid userId, string token, string newPassword)
        {
            var resetToken = string.Empty;
            var user = await _userDbService.GetUser(userId);
            if (user == null) return false;
            if (!Guid.TryParse(token, out var guid)) return false;
            if (_tokenService.CheckResetToken(guid))
            {
                resetToken = await _userDbService.GetResetToken(user);
            }
            return await _userDbService.ResetPassword(user, resetToken, newPassword);
        }

        public async Task<bool> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userDbService.GetUser(userId);
            if (user == null) return false;
            return await _userDbService.UpdateUser(user, oldPassword, newPassword);
        }
    }
}
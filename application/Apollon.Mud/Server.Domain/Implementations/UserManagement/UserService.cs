using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.DbContext;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.Dungeons.Inspectables;
using Apollon.Mud.Server.Model.Implementations.User;


namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <inheritdoc cref="IUserService"/>
    public class UserService: IUserService
    {
        #region member
        /// <summary>
        /// Service to send mails.
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Service to modify the user-db content.
        /// </summary>
        private readonly IUserDbService _userDbService;

        /// <summary>
        /// Service to check if the current request is valid.
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Service to modify the dungeon-db content.
        /// </summary>
        private readonly IGameDbService _gameDbService;

        /// <summary>
        /// Flag which indicates if one administrator is registered.
        /// </summary>
        private readonly bool _adminRegistered;
        #endregion

        public UserService(IEmailService emailService, IUserDbService userDbService, IGameDbService gameDbService, ITokenService tokenService)
        {
            _emailService = emailService;
            _userDbService = userDbService;
            _gameDbService = gameDbService;
            _tokenService = tokenService;
            _adminRegistered = _userDbService.IsAdminRegistered().Result;
        }

        #region methods
        /// <inheritdoc cref="IUserService.RequestUserRegistration"/>
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

        /// <inheritdoc cref="IUserService.ConfirmUserRegistration"/>
        public async Task<bool> ConfirmUserRegistration(Guid userId, string token)
        {
            var confirmationToken = string.Empty;
            var user = await _userDbService.GetUser(userId);
            if (user is null) return false;

            if (!Guid.TryParse(token, out var guid)) return false;
            if (_tokenService.CheckConfirmationToken(guid))
            {
                confirmationToken = await _userDbService.GetEmailConfirmationToken(user);
            }
            return await _userDbService.ConfirmEmail(user, confirmationToken);
        }

        /// <inheritdoc cref="IUserService.DeleteUser"/>
        public async Task<bool> DeleteUser(Guid userId)
        {
            return await _userDbService.DeleteUser(userId);
        }
        
        /// <inheritdoc cref="IUserService.GetAllUsers"/>
        [ExcludeFromCodeCoverage]
        public async Task<ICollection<DungeonUser>> GetAllUsers()
        {
            return await _userDbService.GetUsers();
        }

        /// <inheritdoc cref="IUserService.GetUser"/>
        [ExcludeFromCodeCoverage]
        public async Task<DungeonUser> GetUser(Guid userId)
        {
            return await _userDbService.GetUser(userId);
        }

        /// <inheritdoc cref="IUserService.RequestPasswordReset"/>
        public async Task<bool> RequestPasswordReset(string userEmail)
        {
            var user = await _userDbService.GetUserByEmail(userEmail);
            if (user is null) return false;

            var resetToken = _tokenService.GenerateNewResetToken();
            var resetLink = $"http://mud.apollon-dungeons.de/Identity/Account/ForgotPassword/{user.Id}/{resetToken}";

            var emailText = $"Hallo {user.UserName},\n\ndu hast das Zurücksetzen deines Passworts beantragt. Bitte folge dem nachstehenden " +
                            $"Link, um dein Passwort zurück zu setzen:\n\n{resetLink}\n\n. Dein Apollon-Support-Team.";

            await _emailService.SendEmail(userEmail, emailText, "Rücksetzung deines Passworts.");
            return true;
        }

        /// <inheritdoc cref="IUserService.ConfirmPasswordReset"/>
        public async Task<bool> ConfirmPasswordReset(Guid userId, string token, string newPassword)
        {
            var resetToken = string.Empty;
            var user = await _userDbService.GetUser(userId);
            if (user is null) return false;

            if (!Guid.TryParse(token, out var guid)) return false;
            if (_tokenService.CheckResetToken(guid))
            {
                resetToken = await _userDbService.GetResetToken(user);
            }
            return await _userDbService.ResetPassword(user, resetToken, newPassword);
        }

        /// <inheritdoc cref="IUserService.ChangePassword"/>
        public async Task<bool> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userDbService.GetUser(userId);
            if (user is null) return false;
            return await _userDbService.UpdateUser(user, oldPassword, newPassword);
        }
        #endregion
    }
}
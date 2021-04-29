using System;
using System.Security.Claims;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        /// <summary>
        /// ToDo
        /// </summary>
        private IUserDBService _userDbService;

        /// <summary>
        /// ToDo
        /// </summary>
        private SignInManager<DungeonUser> _signInManager;

        /// <summary>
        /// ToDo
        /// </summary>
        private string _tokenSecret; //ToDo über Config eintragen lassen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userDbService"></param>
        /// <param name="signInManager"></param>
        public AuthorizationService(IUserDBService userDbService, SignInManager<DungeonUser> signInManager)
        {
            _userDbService = userDbService;
            _signInManager = signInManager;
        }

        public LoginResult Login(string email, string secret)
        {
            var user = _userDbService.GetUserByEmail(email);
            if (user == null) return new LoginResult
            {
                Status = LoginResultStatus.BadRequest
            };

            var task = _signInManager.CheckPasswordSignInAsync(user, secret, false);
            if (task.Result.Succeeded)
            {
                //ToDo generate Token
                
                var token = "";
                return new LoginResult
                {
                    User = user,
                    Token = token,
                    Status = LoginResultStatus.OK
                };
            }

            return new LoginResult
            {
                Status = LoginResultStatus.Unauthorized
            };
        }
    }
}
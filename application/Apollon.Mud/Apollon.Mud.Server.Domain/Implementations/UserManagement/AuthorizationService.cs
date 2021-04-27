using System;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class AuthorizationService: IAuthorizationService
    {
        //ToDo implement
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
            var loginTask = _signInManager.CheckPasswordSignInAsync(user, secret, true);
            if (loginTask.Result.Succeeded)
            {
                //ToDo generate Token
                var token = "";
                return new LoginResult
                {
                    User = user,
                    Token = token
                };
            }
            //ToDo was zurückgeben, wenn login fehlschlägt?!
            return null;
        }
    }
}
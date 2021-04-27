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
        private IUserDBService _UserDbService;
        private SignInManager<DungeonUser> _SingInManager;
        /// <summary>
        /// 
        /// </summary>
        private string _TokenSecret; //ToDo über Config eintragen lassen

        public LoginResult Login(string email, string secret)
        {
            var user = _UserDbService;
            //ToDo
            return new LoginResult
            {

            };
        }
    }
}
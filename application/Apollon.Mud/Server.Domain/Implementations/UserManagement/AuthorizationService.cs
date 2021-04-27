using System;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
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
        public IUserDBService UserDbService { get; init; }
        public SignInManager<DungeonUser> SingInManager { get; init; }
        public string TokenSecret { get; init; }
        public LoginResult Login(Guid userId, string secret)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity;

namespace Apollon.Mud.Server.Domain.Interfaces.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public interface IAuthorizationService
    {
        //ToDo sind in UML privat --> sollte hier geändert werden oder?
        IUserDBService UserDbService { get; init; }
        SignInManager<DungeonUser> SingInManager { get; init; }
        string TokenSecret { get; init; }

        LoginResult Login(Guid userId, string secret); //ToDo wo ist LoginResult?!
    }
}
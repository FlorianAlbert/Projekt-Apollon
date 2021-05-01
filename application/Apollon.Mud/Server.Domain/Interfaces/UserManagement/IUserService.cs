using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Server.Model.Implementations.User;

namespace Apollon.Mud.Server.Domain.Interfaces.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public interface IUserService 
    {
        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> RequestUserRegistration(string userEmail, string password);//ToDo in UML anpassen //ToDo checken ob Passwort selbst passt?!

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ConfirmUserRegistration(Guid userId, string token);//ToDo in UML anpassen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid userId);//ToDo in UML anpassen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <returns></returns>
        Task<ICollection<DungeonUser>> GetAllUsers();//ToDo in UML anpassen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DungeonUser> GetUser(Guid userId);//ToDo in UML anpassen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task<bool> RequestPasswordReset(string userEmail);//ToDo in UML anpassen

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="resetToken"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<bool> ConfirmPasswordReset(Guid userId, string resetToken, string newPassword);//ToDo in UML anpassen //ToDo checken ob Passwort selbst passt?!

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task<bool> ChangePassword(Guid userId, string oldPassword, string newPassword);//ToDo in UML anpassen //ToDo checken ob Passwort selbst passt?!
    }
}
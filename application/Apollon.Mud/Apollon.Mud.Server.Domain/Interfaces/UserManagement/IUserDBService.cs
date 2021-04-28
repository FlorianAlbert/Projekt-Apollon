using System;
using System.Collections.Generic;
using Apollon.Mud.Server.Model.Implementations.User;

namespace Apollon.Mud.Server.Domain.Interfaces.UserManagement
{
    /// <summary>
    /// ToDO
    /// </summary>
    public interface IUserDBService
    {
        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool CreateUser(DungeonUser user, string password); //ToDo in UML anpassen und absprechen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DungeonUser GetUser(Guid userId);

        /// <summary>
        /// ToDo
        /// </summary>
        /// <returns></returns>
        ICollection<DungeonUser> GetUsers();

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool UpdateUser(DungeonUser user, string oldPassword, string newPassword); //ToDo in UML anpassen und absprechen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool DeleteUser(Guid userId);

        /// <summary>
        /// ToDO
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        DungeonUser GetUserByEmail(string userEmail);
    }
}
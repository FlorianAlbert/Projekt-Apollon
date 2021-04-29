using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<bool> CreateUser(DungeonUser user, string password, bool asAdmin); //ToDo in UML anpassen und absprechen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DungeonUser> GetUser(Guid userId); //ToDo in UML anpassen und absprechen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <returns></returns>
        Task<ICollection<DungeonUser>> GetUsers(); //ToDo in UML anpassen und absprechen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> UpdateUser(DungeonUser user, string oldPassword, string newPassword); //ToDo in UML anpassen und absprechen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid userId); //ToDo in UML anpassen und absprechen

        /// <summary>
        /// ToDO
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task<DungeonUser> GetUserByEmail(string userEmail); //ToDo in UML anpassen und absprechen
    }
}
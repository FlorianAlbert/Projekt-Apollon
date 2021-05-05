using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Server.Model.Interfaces;

namespace Apollon.Mud.Server.Domain.Interfaces.Shared
{
    /// <summary>
    /// ToDo
    /// </summary>
    public interface IGameDbService
    {
        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> ValidateUserIsDungeonMaster(Guid dungeonId, Guid userId);  // TODO: In UML dungeonId ergänzen!!!

        /// <summary>
        /// ToDo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<ICollection<T>> GetAll<T>() where T : class, IApprovable;

        /// <summary>
        /// ToDo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Get<T>(Guid id) where T : class, IApprovable;

        /// <summary>
        /// ToDo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete<T>(Guid id) where T : class, IApprovable;

        /// <summary>
        /// ToDo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> NewOrUpdate<T>(T item) where T : class, IApprovable;

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteAllFromUser(Guid userId);
    }
}
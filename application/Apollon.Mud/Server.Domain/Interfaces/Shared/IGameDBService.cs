using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Server.Model.Interfaces;

namespace Apollon.Mud.Server.Domain.Interfaces.Shared
{
    /// <summary>
    /// Service which offer the CRUD-functionality
    /// </summary>
    public interface IGameDbService
    {
        /// <summary>
        /// Returns if the user with the userId is allowed to play as dungeon master.
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> ValidateUserIsDungeonMaster(Guid dungeonId, Guid userId);

        /// <summary>
        /// Gets all entities from the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<ICollection<T>> GetAll<T>() where T : class, IApprovable;

        /// <summary>
        /// Gets the entity from the type T with the given id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Get<T>(Guid id) where T : class, IApprovable;

        /// <summary>
        /// Deletes the entity of type T with the given id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete<T>(Guid id) where T : class, IApprovable;

        /// <summary>
        /// Updated the entity of type T or creates a new entry if there is no entity with the exact primary key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> NewOrUpdate<T>(T item) where T : class, IApprovable;
    }
}
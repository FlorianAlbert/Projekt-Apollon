using System;
using Apollon.Mud.Server.Model.Interfaces;

namespace Apollon.Mud.Server.Domain.Interfaces.Shared
{
    /// <summary>
    /// ToDo
    /// </summary>
    public interface IGameDbService
    {
        bool ValidateUserIsDungeonMaster(Guid dungeonId, Guid userId);  // TODO: In UML dungeonId ergänzen!!!

        T[] GetAll<T>() where T : class, IApprovable;

        T Get<T>(Guid id) where T : class, IApprovable;

        bool Delete<T>(Guid id) where T : class, IApprovable;

        bool NewOrUpdate<T>(T item) where T : class, IApprovable;
    }
}
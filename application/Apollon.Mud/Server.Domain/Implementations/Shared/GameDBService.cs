using Apollon.Mud.Server.Domain.Interfaces.Shared;
using System;

namespace Apollon.Mud.Server.Domain.Implementations.Shared
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class GameDbService: IGameDbService
    {
        //ToDo implement
        public bool ValidateUserIsDungeonMaster(Guid dungeonId, Guid userId)
        {
            throw new NotImplementedException();
        }

        bool IGameDBService.Delete<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        T IGameDBService.Get<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        T[] IGameDBService.GetAll<T>()
        {
            throw new NotImplementedException();
        }

        bool IGameDBService.NewOrUpdate<T>(T item)
        {
            throw new NotImplementedException();
        }
    }
}
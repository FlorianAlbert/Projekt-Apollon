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

        bool IGameDbService.Delete<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        T IGameDbService.Get<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        T[] IGameDbService.GetAll<T>()
        {
            throw new NotImplementedException();
        }

        bool IGameDbService.NewOrUpdate<T>(T item)
        {
            throw new NotImplementedException();
        }
    }
}
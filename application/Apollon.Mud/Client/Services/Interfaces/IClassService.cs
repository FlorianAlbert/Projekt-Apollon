using Apollon.Mud.Shared.Dungeon.Class;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IClassService
    {
        /// <summary>
        /// TODO
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="classDto"></param>
        /// <returns></returns>
        Task<Guid> CreateNewClass(ClassDto classDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="classDto"></param>
        /// <returns></returns>
        Task<ClassDto> UpdateClass(ClassDto classDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="classDto"></param>
        /// <returns></returns>
        /// 
        Task<bool> DeleteClass(Guid classId, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        Task<ICollection<ClassDto>> GetAllClasses(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        Task<ClassDto> GetClass(Guid dungeonId, Guid actionId);


    }
}

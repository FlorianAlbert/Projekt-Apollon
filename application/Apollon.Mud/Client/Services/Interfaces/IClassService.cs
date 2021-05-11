using Apollon.Mud.Shared.Dungeon.Class;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// An interface to provides CRUD Functions for Dungeon Classes
    /// </summary>
    public interface IClassService
    {
        /// <summary>
        /// The Rest Http Client injected into the class
        /// </summary
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Creates Cancellation Tokens for each Http Request
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Sends the given class to the backend and saves its connection to the given Dungeon in the Database
        /// </summary>
        /// <param name="classDto">The Class to create</param>
        /// <param name="dungeonId">The dungeon that contains the class</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        Task<Guid> CreateNewClass(ClassDto classDto, Guid dungeonId);

        /// <summary>
        /// Updates the given class in the Database
        /// </summary>
        /// <param name="classDto">The class with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the class</param>
        /// <returns>The old Class in case the Database transaction failed, otherwise null</returns>
        Task<ClassDto> UpdateClass(ClassDto classDto, Guid dungeonId);

        /// <summary>
        /// Deletes the given class in the Database
        /// </summary>
        /// <param name="classId">The class to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the class</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<bool> DeleteClass(Guid dungeonId, Guid classId);

        /// <summary>
        /// Gets all classes of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested classes</param>
        /// <returns>A Collection of the requested Classes, otherwise null</returns>
        Task<ICollection<ClassDto>> GetAllClasses(Guid dungeonId);

        /// <summary>
        /// Gets one class from a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested class</param>
        /// <param name="classId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        Task<ClassDto> GetClass(Guid dungeonId, Guid classId);


    }
}

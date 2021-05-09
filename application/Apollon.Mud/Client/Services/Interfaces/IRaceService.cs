using Apollon.Mud.Shared.Dungeon.Race;
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
    public interface IRaceService
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
        /// <param name="raceDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewRace(RaceDto raceDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="raceDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<RaceDto> UpdateRace(RaceDto raceDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="raceId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        /// 
        Task<bool> DeleteRace(Guid dungeonId, Guid raceId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<ICollection<RaceDto>> GetAllRaces(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="raceId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        Task<RaceDto> GetRace(Guid dungeonId, Guid raceId);

    }
}

using Apollon.Mud.Shared.Dungeon.Npc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface INpcService
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
        /// <param name="npcDto, dungeonId"></param>
        /// <returns></returns>
        Task<Guid> CreateNewNpc(NpcDto npcDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="npcDto"></param>
        /// <returns></returns>
        Task<NpcDto> UpdateNpc(NpcDto npcDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="npcDto"></param>
        /// <returns></returns>
        /// 
        Task<bool> DeleteNpc(NpcDto npcDto, Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        Task<ICollection<NpcDto>> GetAllNpcs(Guid dungeonId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        Task<NpcDto> GetNpc(Guid dungeonId, Guid actionId);


    }
}

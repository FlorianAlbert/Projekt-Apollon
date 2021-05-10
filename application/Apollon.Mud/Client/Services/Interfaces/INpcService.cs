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
    /// Interface to provide CRUD Functions for Dungeon NPCS
    /// </summary>
    public interface INpcService
    {
        /// <summary>
        /// The Rest Http Client injected into the class
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Creates Cancellation Tokens for each Http Request
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Sends the given dungeon to the backend and persists it in the Database
        /// </summary>
        /// <param name="npcDto">The Dungeon to create</param>
        /// <param name="dungeonId">The Dungeon that contains the npc</param>
        /// <returns>The Guid if the DB Transaction was successfull, otherwise an empty Guid</returns>
        Task<Guid> CreateNewNpc(NpcDto npcDto, Guid dungeonId);

        /// <summary>
        /// Updates the given npc in the Database
        /// </summary>
        /// <param name="npcDto">The Npc with updated information</param>
        /// <param name="dungeonId">The Dungeon that contains the Npc</param>
        /// <returns>The old Npc in case the Database transaction failed, otherwise null</returns>
        Task<NpcDto> UpdateNpc(NpcDto npcDto, Guid dungeonId);

        /// <summary>
        /// Deletes the given Npc in the Database
        /// </summary>
        /// <param name="npcId">The id of the Npc to delete</param>
        /// <param name="dungeonId">The Dungeon that contains the npc</param>
        /// <returns>Wether the DB transaction was successfull</returns>
        Task<bool> DeleteNpc(Guid dungeonId, Guid npcId);

        /// <summary>
        /// Gets all Npcs of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested npcs</param>
        /// <returns>A Collection of the requested Npcs, otherwise null</returns>
        Task<ICollection<NpcDto>> GetAllNpcs(Guid dungeonId);

        /// <summary>
        /// Gets one Npc of a dungeon
        /// </summary>
        /// <param name="dungeonId">The ID of the dungeon containing the requested Npc</param>
        /// <param name="npcId">The ID of the requested class</param>
        /// <returns>The requested class, otherwise null</returns>
        Task<NpcDto> GetNpc(Guid dungeonId, Guid npcId);
    }
}

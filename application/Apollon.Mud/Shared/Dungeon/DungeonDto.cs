using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Shared.Dungeon.Room;
using Apollon.Mud.Shared.Dungeon.User;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon
{
    /// <summary>
    /// Class which represents the data representation of IDungeon.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DungeonDto
    {

        /// <summary>
        /// The name of the dungeon.
        /// </summary>
        [JsonProperty("DungeonName")]
        public string DungeonName { get; set; }

        /// <summary>
        /// The description of the dungeon.
        /// </summary>
        [JsonProperty("DungeonDescription")]
        public string DungeonDescription { get; set; }

        /// <summary>
        /// The epoch of the dungeon.
        /// </summary>
        [JsonProperty("DungeonEpoch")]
        public string DungeonEpoch { get; set; }

        /// <summary>
        /// The default rooms id of the dungeon.
        /// </summary>
        [JsonProperty("DefaultRoom")]
        public RoomDto DefaultRoom { get; set; }

        /// <summary>
        /// The visibility of the dungeon. 
        /// </summary>
        [JsonProperty("Visibility")]
        public int Visibility { get; set; }

        /// <summary>
        /// The unique id of the dungeon.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the dungeon is currently playable.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The possible dungeon masters of the dungeon.
        /// </summary>
        [JsonProperty("DungeonMasters")]
        public ICollection<DungeonUserDto> DungeonMasters { get; set; }

        /// <summary>
        /// The current Dungeon Master playing the game
        /// </summary>
        [JsonProperty("DungeonMasters")]
        public DungeonUserDto CurrentMaster { get; set; }

        /// <summary>
        /// The owner of the dungeon.
        /// </summary>
        [JsonProperty("DungeonOwner")]
        public DungeonUserDto DungeonOwner { get; set; }

        /// <summary>
        /// The users which are allowed to play the dungeon.
        /// </summary>
        [JsonProperty("WhiteList")]
        public ICollection<DungeonUserDto> WhiteList { get; set; }

        /// <summary>
        /// The users which are explicitly disallowed to play the dungeon.
        /// </summary>
        [JsonProperty("BlackList")]
        public ICollection<DungeonUserDto> BlackList { get; set; }

        /// <summary>
        /// The users which are currently requesting to play the dungeon.
        /// </summary>
        [JsonProperty("OpenRequests")]
        public ICollection<DungeonUserDto> OpenRequests { get; set; }

        /// <summary>
        /// The Timestamp when the dungeon got played by an avatar the last time
        /// </summary>
        [JsonProperty("LastActive")]
        public DateTime LastActive { get; set; }

    }
}

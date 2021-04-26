﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Apollon.Mud.Shared.Dungeon.Room;
using Apollon.Mud.Shared.Dungeon.User;
using Newtonsoft.Json;

namespace Apollon.Mud
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class DungeonDto
    {

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DungeonName")]
        public string DungeonName { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DungeonDescription")]
        public string DungeonDescription { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DungeonEpoch")]
        public string DungeonEpoch { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DefaultRoom")]
        public RoomDto DefaultRoom { get; set; }

        /// <summary>
        /// ToDo 
        /// </summary>
        [JsonProperty("Visibility")]
        public int Visibility { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DungeonMasters")]
        public ICollection<DungeonUserDto> DungeonMasters { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DungeonOwner")]
        public DungeonUserDto DungeonOwner { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("WhiteList")]
        public ICollection<DungeonUserDto> WhiteList { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("BlackList")]
        public ICollection<DungeonUserDto> BlackList { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("OpenRequests")]
        public ICollection<DungeonUserDto> OpenRequests { get; set; }

    }
}

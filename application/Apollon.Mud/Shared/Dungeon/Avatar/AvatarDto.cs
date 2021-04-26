using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Mud.Shared.Dungeon.Class;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Shared.Dungeon.Race;
using Apollon.Mud.Shared.Dungeon.Room;
using Apollon.Mud.Shared.Dungeon.User;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Avatar
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class AvatarDto
    {
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
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Race")]
        public RaceDto Race { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Class")]
        public ClassDto Class { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Gender")]
        public int Gender { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("CurrentHealth")]
        public int CurrentHealth { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("HoldingItem")]
        public TakeableDto HoldingItem { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Armor")]
        public WearableDto Armor { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("CurrentRoom")]
        public RoomDto CurrentRoom { get; set; }

    }
}

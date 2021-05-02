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
    /// Class which represents the data representation of IAvatar.
    /// </summary>
    public class AvatarDto
    {
        /// <summary>
        /// The unique id of the avatar.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status of the avatar, which represents if the avatar is currently playing or not.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The name of the avatar.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The race of the avatar.
        /// </summary>
        [JsonProperty("Race")]
        public RaceDto Race { get; set; }

        /// <summary>
        /// The class of the avatar.
        /// </summary>
        [JsonProperty("Class")]
        public ClassDto Class { get; set; }

        /// <summary>
        /// The gender of the avatar.
        /// </summary>
        [JsonProperty("Gender")]
        public int Gender { get; set; }

        /// <summary>
        /// The current health of the avatar.
        /// </summary>
        [JsonProperty("CurrentHealth")]
        public int CurrentHealth { get; set; }

        /// <summary>
        /// The item which the avatar is currently holding.
        /// </summary>
        [JsonProperty("HoldingItem")]
        public TakeableDto HoldingItem { get; set; }

        /// <summary>
        /// The armor which the avatar currently wears.
        /// </summary>
        [JsonProperty("Armor")]
        public WearableDto Armor { get; set; }

        /// <summary>
        /// The room in which the avatar is currently located.
        /// </summary>
        [JsonProperty("CurrentRoom")]
        public RoomDto CurrentRoom { get; set; }

    }
}

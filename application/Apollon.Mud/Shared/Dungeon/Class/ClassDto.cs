using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.Dungeon.Class
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class ClassDto
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
        [JsonProperty("InventoryTakeableDtos")]
        public ICollection<TakeableDto> InventoryTakeableDtos { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("InventoryUsableDtos")]
        public ICollection<UsableDto> InventoryUsableDtos { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("InventoryConsumableDtos")]
        public ICollection<ConsumableDto> InventoryConsumableDtos { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("InventoryWearableDtos")]
        public ICollection<WearableDto> InventoryWearableDtos { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DefaultHealth")]
        public int DefaultHealth { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DefaultProtection")]
        public int DefaultProtection { get; set; }

        /// <summary>
        /// ToDo
        /// </summary>
        [JsonProperty("DefaultDamage")]
        public int DefaultDamage { get; set; }
    }
}

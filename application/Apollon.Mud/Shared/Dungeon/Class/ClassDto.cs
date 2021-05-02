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
    /// Class which represents the data representation of IClass.
    /// </summary>
    public class ClassDto
    {
        /// <summary>
        /// The unique id of the class.
        /// </summary>
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The status which represents, if the class is currently playable.
        /// </summary>
        [JsonProperty("Status")]
        public int Status { get; set; }

        /// <summary>
        /// The part of the start-inventory, which includes the takeables.
        /// </summary>
        [JsonProperty("InventoryTakeableDtos")]
        public ICollection<TakeableDto> InventoryTakeableDtos { get; set; }

        /// <summary>
        /// The part of the start-inventory, which includes the usables.
        /// </summary>
        [JsonProperty("InventoryUsableDtos")]
        public ICollection<UsableDto> InventoryUsableDtos { get; set; }

        /// <summary>
        /// The part of the start-inventory, which includes the consumables.
        /// </summary>
        [JsonProperty("InventoryConsumableDtos")]
        public ICollection<ConsumableDto> InventoryConsumableDtos { get; set; }

        /// <summary>
        /// The part of the start-inventory, which includes the wearables.
        /// </summary>
        [JsonProperty("InventoryWearableDtos")]
        public ICollection<WearableDto> InventoryWearableDtos { get; set; }

        /// <summary>
        /// The name of the class.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The description of the class.
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// The default health of the class.
        /// </summary>
        [JsonProperty("DefaultHealth")]
        public int DefaultHealth { get; set; }

        /// <summary>
        /// The default protection of the class.
        /// </summary>
        [JsonProperty("DefaultProtection")]
        public int DefaultProtection { get; set; }

        /// <summary>
        /// The default damage of the class.
        /// </summary>
        [JsonProperty("DefaultDamage")]
        public int DefaultDamage { get; set; }
    }
}

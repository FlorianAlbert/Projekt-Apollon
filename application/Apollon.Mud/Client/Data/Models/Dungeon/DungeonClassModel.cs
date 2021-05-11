using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;

namespace Apollon.Mud.Client.Data.Models.Dungeon
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when creating or changing a class of a dungeon
    /// </summary>
    class DungeonClassModel
    {
        /// <summary>
        /// The class has to have a name
        /// </summary>
        [Required(ErrorMessage = "Du musst der Klasse einen Namen geben")]
        [RegularExpression("^(?!([Nn]eue [Kk]lasse)).*$", ErrorMessage = "Dieser Name ist nicht zugelassen")]
        public string Name { get; set; }

        /// <summary>
        /// The class has to have a description
        /// </summary>
        [Required(ErrorMessage = "Du musst eine Beschreibung der Klasse angeben")]
        public string Description { get; set; }

        /// <summary>
        /// The class has to have a default health value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Lebenspunktewert angeben")]
        [Range(1,99, ErrorMessage ="Der Wert muss zwischen 1 und 99 liegen")]
        public int? Health { get; set; }

        /// <summary>
        /// The class has to have a default damage value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Schadenswert angeben")]
        [Range(1, 99, ErrorMessage = "Der Wert muss zwischen 1 und 99 liegen")]
        public int? Damage { get; set; }

        /// <summary>
        /// The class has to have a default protection value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Verteidigungswert angeben")]
        [Range(1, 99, ErrorMessage = "Der Wert muss zwischen 1 und 99 liegen")]
        public int? Protection { get; set; }


        /// <summary>
        /// A Class can have a StartInventory of up to 5 Items, but it isn't mandatory
        /// </summary>
        /// 
        /// <summary>
        /// The part of the start-inventory, which includes the takeables.
        /// </summary>
        public List<TakeableDto> InventoryTakeableDtos { get; set; } = new List<TakeableDto>();

        /// <summary>
        /// The part of the start-inventory, which includes the usables.
        /// </summary>
        public List<UsableDto> InventoryUsableDtos { get; set; } = new List<UsableDto>();

        /// <summary>
        /// The part of the start-inventory, which includes the consumables.
        /// </summary>
        public List<ConsumableDto> InventoryConsumableDtos { get; set; } = new List<ConsumableDto>();

        /// <summary>
        /// The part of the start-inventory, which includes the wearables.
        /// </summary>
        public List<WearableDto> InventoryWearableDtos { get; set; } = new List<WearableDto>();

        /// <summary>
        /// The dungeon has to have a status. ToDo Abhilfe
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Status geben")]
        public string Status { get; set; }

    }
}

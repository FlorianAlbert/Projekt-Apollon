using Apollon.Mud.Shared.Dungeon.Inspectable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Consumable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Usable;
using Apollon.Mud.Shared.Dungeon.Inspectable.Takeable.Wearable;
using Apollon.Mud.Shared.Dungeon.Npc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when creating or changing a Race of a dungeon
    /// </summary>
    class DungeonRoomModel
    {
        /// <summary>
        /// Every Room has to have a name
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Raum einen Namen geben")]
        [RegularExpression("^(?!\b([Kk]ein [Nn]achbar|[Nn]euer [Rr]aum)\b)", ErrorMessage = "Dieser Name ist nicht zugelassen")]
        //[RegularExpression("^(?!([Kk]ein [Nn]achbar|[Nn]euer [Rr]aum)$)", ErrorMessage = "Dieser Name ist nicht zugelassen")]
        public string Name { get; set; }

        /// <summary>
        /// Every Room has to have a Description
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Raum eine Beschreibung geben")]
        public string Description { get; set; }

        /// <summary>
        /// Every Room can have a neighbouring Room north of it
        /// </summary>
        [Required(ErrorMessage = "Bitte gib einen Nachbarn an, oder wähle \"Kein Nachbar\" aus")]
        public string RoomNorth { get; set; }

        /// <summary>
        /// Every Room can have a neighbouring Room east of it
        /// </summary>
        [Required(ErrorMessage = "Bitte gib einen Nachbarn an, oder wähle \"Kein Nachbar\" aus")]
        public string RoomEast { get; set; }

        /// <summary>
        /// Every Room can have a neighbouring Room south of it
        /// </summary>
        [Required(ErrorMessage = "Bitte gib einen Nachbarn an, oder wähle \"Kein Nachbar\" aus")]
        public string RoomSouth { get; set; }

        /// <summary>
        /// Every Room can have a neighbouring Room west of it
        /// </summary>
        [Required(ErrorMessage = "Bitte gib einen Nachbarn an, oder wähle \"Kein Nachbar\" aus")]
        public string RoomWest { get; set; }

        /// <summary>
        /// This is the list of purely investigable items.
        /// </summary>
        public List<InspectableDto> Inspectables { get; set; }

        /// <summary>
        /// This is the list of pickable items
        /// </summary>
        public List<TakeableDto> Takeables { get; set; }

        /// <summary>
        /// This is the list of purely consumable items
        /// </summary>
        public List<ConsumableDto> Consumables { get; set; }

        /// <summary>
        /// This is the list of wearable items
        /// </summary>
        public List<WearableDto> Wearables { get; set; }

        /// <summary>
        /// This is the list of purely usable items
        /// </summary>
        public List<UsableDto> Usables { get; set; }

        /// <summary>
        /// This is the list of NPC
        /// </summary>
        public List<NpcDto> Npcs { get; set; }

        /// <summary>
        /// The status indicates whether the dungeon is active
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Status geben")]
        public string Status { get; set; }
    }
}

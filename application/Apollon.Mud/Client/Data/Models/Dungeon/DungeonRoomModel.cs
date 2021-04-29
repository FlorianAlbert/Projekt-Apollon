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
        public string Name { get; set; }

        /// <summary>
        /// Every Room has to have a Description
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Raum eine Beschreibung geben")]
        public string Description { get; set; }

        /// <summary>
        /// Every Room can have a neighbouring Room north of it
        /// </summary>
        public string RoomNorth { get; set; }

        /// <summary>
        /// Every Room can have a neighbouring Room east of it
        /// </summary>
        public string RoomEast { get; set; }

        /// <summary>
        /// Every Room can have a neighbouring Room south of it
        /// </summary>
        public string RoomSouth { get; set; }

        /// <summary>
        /// Every Room can have a neighbouring Room west of it
        /// </summary>
        public string RoomWest { get; set; }
    }
}

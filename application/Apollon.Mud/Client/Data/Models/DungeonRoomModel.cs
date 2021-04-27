using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    class DungeonRoomModel
    {
        /// <summary>
        /// TODO 
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Raum einen Namen geben")]
        public string Name { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Raum eine Beschreibung geben")]
        public string Description { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string RoomNorth { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string RoomEast { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string RoomSouth { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string RoomWest { get; set; }
    }
}

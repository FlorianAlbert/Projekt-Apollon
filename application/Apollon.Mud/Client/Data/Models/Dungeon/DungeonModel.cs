using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when creating or changing a NPC of a dungeon
    /// </summary>
    class DungeonModel
    {
        /// <summary>
        /// Every Dungeon has to have a Name
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Namen geben")]
        public string Name { get; set; }

        /// <summary>
        /// Every Dungeon has to have a Description
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon eine Beschreibung geben")]
        public string Description { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon eine Epoche geben")]
        public string Epoch { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Startraum geben")]
        public string DefaultRoom { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon eine Sichtbarkeit geben")]
        public string Visibility { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Status geben")]
        public string Status { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    /// <summary>
    /// This is the model to validate the form a user has to fill out when creating or changing a NPC of a dungeon
    /// </summary>
    class DungeonModel
    {
        /// <summary>
        /// Every dungeon has to have a Name
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Namen geben")]
        public string Name { get; set; }

        /// <summary>
        /// Every dungeon has to have a Description
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon eine Beschreibung geben")]
        public string Description { get; set; }

        /// <summary>
        /// The dungeon requires an era
        /// to determine the theme of the dungeon.
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon eine Epoche geben")]
        public string Epoch { get; set; }

        /// <summary>
        /// Every dungeon has to have a default start room.
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Startraum geben")]
        public string DefaultRoom { get; set; }

        /// <summary>
        /// A dungeon has to have a visibility,
        /// which can be private or public.
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon eine Sichtbarkeit geben")]
        public string Visibility { get; set; }

        /// <summary>
        /// The status indicates whether the dungeon is active
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Status geben")]
        public string Status { get; set; }


    }
}

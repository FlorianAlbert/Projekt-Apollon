﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when creating or changing a NPC of a dungeon
    /// </summary>
    class DungeonNpcModel
    {
        /// <summary>
        /// Every NPC has to have a Name
        /// </summary>
        [Required(ErrorMessage = "Du musst dem NPC einen Namen geben")]
        public string Name { get; set; }

        /// <summary>
        /// Every NPC has to have a Description
        /// </summary>
        [Required(ErrorMessage = "Du musst dem NPC eine Beschreibung geben")]
        public string Description { get; set; }

        /// <summary>
        /// Every NPC has to have a standard answer (current state)
        /// </summary>
        [Required(ErrorMessage = "Du musst dem NPC einen Text geben")]
        public string Text { get; set; }


    }
}

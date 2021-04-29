using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    class DungeonNpcModel
    {
        /// <summary>
        /// TODO 
        /// </summary>
        [Required(ErrorMessage = "Du musst dem NPC einen Namen geben")]
        public string Name { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem NPC eine Beschreibung geben")]
        public string Description { get; set; }
        
        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem NPC einen Text geben")]
        public string Text { get; set; }


    }
}

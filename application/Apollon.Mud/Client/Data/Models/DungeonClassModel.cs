using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    class DungeonClassModel
    {
        /// <summary>
        /// TODO 
        /// </summary>
        [Required(ErrorMessage = "Du musst der Klasse einen Namen geben")]
        public string Name { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst eine Beschreibung der Klasse angeben")]
        public string Description { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Lebenspunktewert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Lebenspunkte können nur eine Ganzzahl sein")]
        public string Health { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Schadenswert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Der Schaden kann nur eine Ganzzahl sein")]
        public string Damage { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Verteidigungswert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Die Verteidigung kann nur eine Ganzzahl sein")]
        public string Protection { get; set; }
    }
}

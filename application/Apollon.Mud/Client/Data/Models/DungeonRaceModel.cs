using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    class DungeonRaceModel
    {
        /// <summary>
        /// TODO 
        /// </summary>
        [Required(ErrorMessage = "Du musst der Rasse einen Namen geben")]
        public string Name { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst eine Beschreibung der Rasse angeben")]
        public string Description { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Lebenspunktewert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Lebenspunkte können nur als Ganzzahl angegeben betragen")]
        public string Health { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Schadenswert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Der Schaden kann nur als Ganzzahl angegeben werden")]
        public string Damage { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Verteidigungswert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Die Verteidigung kann nur als Ganzzahl angegeben werden")]
        public string Protection { get; set; }

        /// <summary>
        /// TODO Template ändern
        /// </summary>
        public List<string> startInventory { get; set; }
    }
}


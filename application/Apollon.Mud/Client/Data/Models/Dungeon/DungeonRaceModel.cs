using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models.Dungeon
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when creating or changing a Race of a dungeon
    /// </summary>
    class DungeonRaceModel
    {
        /// <summary>
        /// Every Race has to have a name
        /// </summary>
        [Required(ErrorMessage = "Du musst der Rasse einen Namen geben")]
        [RegularExpression("^(?!([Nn]eue [Rr]asse)).*$", ErrorMessage = "Dieser Name ist nicht zugelassen")]
        public string Name { get; set; }

        /// <summary>
        /// Every Race has to have a description
        /// </summary>
        [Required(ErrorMessage = "Du musst eine Beschreibung der Rasse angeben")]
        public string Description { get; set; }

        /// <summary>
        /// Every Race has to have a deftaul health value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Lebenspunktewert angeben")]
        [Range(1, 99, ErrorMessage = "Der Wert muss zwischen 1 und 99 liegen")]
        public int? Health { get; set; }

        /// <summary>
        /// Every Race has to have a deftaul damage value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Schadenswert angeben")]
        [Range(1, 99, ErrorMessage = "Der Wert muss zwischen 1 und 99 liegen")]
        public int? Damage { get; set; }

        /// <summary>
        /// Every Race has to have a deftaul Health value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Verteidigungswert angeben")]
        [Range(1, 99, ErrorMessage = "Der Wert muss zwischen 1 und 99 liegen")]
        public int? Protection { get; set; }

        /// <summary>
        /// The status indicates whether the dungeon is active
        /// </summary>
        [Required(ErrorMessage = "Du musst der Rasse einen Status geben")]
        public string Status { get; set; }
    }
}


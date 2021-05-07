using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
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
        [RegularExpression("(^(?![Nn]eue [Rr]asse)|(^([[Nn]eue [Rr]asse).+))", ErrorMessage = "Dieser Name ist nicht zugelassen")]
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
        [RegularExpression("[0-9]*", ErrorMessage = "Lebenspunkte können nur als Ganzzahl angegeben werden")]
        public string Health { get; set; }

        /// <summary>
        /// Every Race has to have a deftaul damage value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Schadenswert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Der Schaden kann nur als Ganzzahl angegeben werden")]
        public string Damage { get; set; }

        /// <summary>
        /// Every Race has to have a deftaul Health value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Verteidigungswert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Die Verteidigung kann nur als Ganzzahl angegeben werden")]
        public string Protection { get; set; }

        /// <summary>
        /// TODO Abhilfe wie sonst auch
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Status geben")]
        public string Status { get; set; }
    }
}


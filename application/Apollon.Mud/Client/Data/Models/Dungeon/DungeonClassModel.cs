using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when creating or changing a class of a dungeon
    /// </summary>
    class DungeonClassModel
    {
        /// <summary>
        /// The class has to have a name
        /// </summary>
        [Required(ErrorMessage = "Du musst der Klasse einen Namen geben")]
        [RegularExpression("(^(?![Nn]eue [Kk]lasse)|(^([Nn]eue [Kk]lasse).+))", ErrorMessage = "Dieser Name ist nicht zugelassen")]
        public string Name { get; set; }

        /// <summary>
        /// The class has to have a description
        /// </summary>
        [Required(ErrorMessage = "Du musst eine Beschreibung der Klasse angeben")]
        public string Description { get; set; }

        /// <summary>
        /// The class has to have a default health value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Lebenspunktewert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Lebenspunkte können nur eine Ganzzahl sein")]
        public string Health { get; set; }

        /// <summary>
        /// The class has to have a default damage value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Schadenswert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Der Schaden kann nur eine Ganzzahl sein")]
        public string Damage { get; set; }

        /// <summary>
        /// The class has to have a default protection value, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Standard Verteidigungswert angeben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Die Verteidigung kann nur eine Ganzzahl sein")]
        public string Protection { get; set; }

        /// <summary>
        /// A Class can have a StartInventory of up to 5 Items, but it isn't mandatory
        /// </summary>
        public List<string> StartInventory { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Dungeon einen Status geben")]
        public string Status { get; set; }
    }
}

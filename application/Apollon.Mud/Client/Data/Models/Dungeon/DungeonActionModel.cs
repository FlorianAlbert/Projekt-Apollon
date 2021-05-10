using Apollon.Mud.Shared.Dungeon.Requestable;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models.Dungeon
{
    /// <summary>
    /// The model to validate the form a user has to fill out when creating a dungeon model
    /// </summary>
    public class DungeonActionModel
    {
        /// <summary>
        /// A readable pattern is required and can't include "Neuer Befehl"
        /// </summary>
        [Required(ErrorMessage = "Du musst ein lesbares Pattern angeben")]
        [RegularExpression("^(?!([Nn]euer [Bb]efehl)).*$", ErrorMessage = "Dieser Name ist nicht zugelassen")]
        public string PatternForPlayer { get; set; }

        /// <summary>
        /// A regex that the users input will be matched with
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Regulären Ausdruck zur Validierung angeben")]
        public string MessageRegex { get; set; }

        /// <summary>
        /// The Status, active or inactive
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Status angeben")]
        public string Status { get; set; }

    }
}

using Apollon.Mud.Shared.Dungeon.Requestable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Data.Models.Dungeon
{
    public class DungeonActionModel
    {
        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst ein lesbares Pattern angeben")]
        [RegularExpression("^(?!([Nn]euer [Bb]efehl)).*$", ErrorMessage = "Dieser Name ist nicht zugelassen")]
        public string PatternForPlayer { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Regulären Ausdruck zur Validierung angeben")]
        public string MessageRegex { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst einen Status angeben")]
        public string Status { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public List<RequestableDto> RequestableList { get; set; } = new List<RequestableDto>();
    }
}

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
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Health { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Damage { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Protection { get; set; }

        /// <summary>
        /// TODO Template ändern
        /// </summary>
        public List<string> startInventory { get; set; }
    }
}

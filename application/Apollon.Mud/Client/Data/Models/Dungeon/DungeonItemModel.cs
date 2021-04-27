using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    class DungeonItemModel
    {
        #region Inspectable
        /// <summary>
        /// TODO
        ///</summary>
        [Required(ErrorMessage = "Du musst dem Item einen Namen geben")]
        public string Name { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item eine Beschreibung geben")]
        public string Description { get; set; }

        #region Takeable

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item ein Gewicht zuordnen")]
        [RegularExpression("[0-9]*", ErrorMessage = "Gewicht kann nur als Ganzzahl angegeben werden")]
        public string Weight { get; set; }

        #region Consumable

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item eine Effektbeschreibung geben")]
        public string EffectDescription { get; set; }

        #endregion

        #region Usable

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item einen Schadensboost geben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Schadensboost kann nur als Ganzzahl angegeben werden")]
        public string DamageBoost { get; set; }

        #endregion

        #region wearable

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item einen Vertidgungsboost geben")]
        [RegularExpression("[0-9]*", ErrorMessage = "Verteidigungsboost kann nur als Ganzzahl angegeben werden")]
        public string ProtectionBoost { get; set; }

        #endregion

        #endregion

        #endregion

    }
}
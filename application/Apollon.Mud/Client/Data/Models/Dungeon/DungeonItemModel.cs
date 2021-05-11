using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models.Dungeon
{
    /// <summary>
    /// The Model to validate the form a user has to fill out when creating or changing an Item of a dungeon
    /// </summary>
    class DungeonItemModel
    {
        #region Inspectable

        /// <summary>
        /// The status indicates whether the dungeon is active
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item einen Status geben")]
        public string Status { get; set; }

        /// <summary>
        /// Every Item hast to have a name
        ///</summary>
        [Required(ErrorMessage = "Du musst dem Item einen Namen geben")]
        [RegularExpression("^(?!([Nn]eues [Ii]tem)).*$", ErrorMessage = "Dieser Name ist nicht zugelassen")]

        public string Name { get; set; }

        /// <summary>
        /// Every Item has to have a description
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item eine Beschreibung geben")]
        public string Description { get; set; }

        #region Takeable

        /// <summary>
        /// Every Takeable has to have a weight, which has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item ein Gewicht zuordnen")]
        [Range(1, 99, ErrorMessage = "Der Wert muss zwischen 1 und 99 liegen")]
        public int? Weight { get; set; }

        #region Consumable

        /// <summary>
        /// Every Consumable has to have a Description of its effect
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item eine Effektbeschreibung geben")]
        public string EffectDescription { get; set; }

        #endregion

        #region Usable

        /// <summary>
        /// Every Usable has to have a Damage Boost, whcih has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item einen Schadensboost geben")]
        [Range(-25, 25, ErrorMessage = "Der Wert muss zwischen -25 und +25 liegen")]
        public int? DamageBoost { get; set; }

        #endregion

        #region wearable

        /// <summary>
        /// Every wearable has to have a Protection Boost, whcih has to be an Integer
        /// </summary>
        [Required(ErrorMessage = "Du musst dem Item einen Vertidgungsboost geben")]
        [Range(-25, 25, ErrorMessage = "Der Wert muss zwischen -25 und +25 liegen")]
        public int? ProtectionBoost { get; set; }

        #endregion

        #endregion

        #endregion

    }
}
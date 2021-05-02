using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when creating a new Avatar
    /// </summary>
    public class AvatarCreationModel
    {
        /// <summary>
        /// He has to give his Avatar a name.
        /// </summary>
        [Required(ErrorMessage = "Bitte gib einen Namen ein!")]
        public string Name { get; set; }

        /// <summary>
        /// He has to choose a class.
        /// </summary>
        [Required(ErrorMessage = "Bitte wähle eine Klasse!")]
        public string Class { get; set; }

        /// <summary>
        /// He has to choose a race.
        /// </summary>
        [Required(ErrorMessage = "Bitte wähle eine Rasse!")]
        public string Race { get; set; }

    }
}

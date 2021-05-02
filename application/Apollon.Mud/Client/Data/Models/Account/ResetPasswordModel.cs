using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when requesting a reset link for a forgotten password
    /// </summary>
    public class ResetPasswordModel
    {
        /// <summary>
        /// TODO
        /// </summary>
        [Required]
        [MinLength(5, ErrorMessage = "Dein Passwort muss mindestens 5 Zeichen lang sein")]
        public string NewPassword { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required]
        [CompareProperty("NewPassword", ErrorMessage = "Die Passwörter stimmen nicht überein")]
        public string NewPasswordConfirmation { get; set; }
    }
}

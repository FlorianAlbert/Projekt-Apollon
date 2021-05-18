using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models.Account
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when requesting a reset link for a forgotten password
    /// </summary>
    public class ResetPasswordModel
    {
        /// <summary>
        /// A error message will appear,
        /// if the set password
        /// is shorter than five characters long.
        /// </summary>
        [Required]
        [MinLength(8, ErrorMessage = "Dein Passwort muss mindestens 8 Zeichen lang sein")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{1,50}", 
            ErrorMessage = "Du musst mindestens einen Groß-, einen Kleinbuchstaben, ein Sonderzeichen sowie eine Zahl eingeben")]
        public string NewPassword { get; set; }

        /// <summary>
        /// A error message will appear,
        /// if the two set passwords
        /// aren't identical.
        /// </summary>
        [Required]
        [CompareProperty("NewPassword", ErrorMessage = "Die Passwörter stimmen nicht überein")]
        public string NewPasswordConfirmation { get; set; }
    }
}

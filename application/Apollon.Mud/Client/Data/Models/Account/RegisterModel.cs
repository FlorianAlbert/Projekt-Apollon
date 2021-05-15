using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models.Account
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when signing up
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// He has to give his E-Mail, which has to be a valid E-Mail Adress
        /// </summary>
        [Required(ErrorMessage = "Bitte gib Deine E-Mail Adresse an")]
        [EmailAddress(ErrorMessage = "Bitte gib eine gültige E-Mail Adresse ein")]
        [ValidateComplexType]
        public string Email { get; set; }

        /// <summary>
        /// He has to create a password which has to be at least 5 characters long
        /// </summary>
        [Required(ErrorMessage = "Bitte gib ein Passwort ein!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{1,50}", ErrorMessage = "Du musst mindestens einen Groß-, einen Kleinbuchstaben, ein Sonderzeichen sowie eine Zahl eingeben")]
        [MinLength(8, ErrorMessage = "Dein Passwort muss mindestens 8 Zeichen lang sein")]
        public string Password { get; set; }

        /// <summary>
        /// He has to confirm his password, to prevent typos
        /// </summary>
        [Required(ErrorMessage = "Bitte bestätige Dein Passwort")]
        [CompareProperty("Password", ErrorMessage = "Die Passwörter stimmen nicht überein")]
        public string PasswordVerification { get; set; }
    }
}

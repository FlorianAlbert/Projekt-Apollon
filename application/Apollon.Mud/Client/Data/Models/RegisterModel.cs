using System;
using System.ComponentModel.DataAnnotations;
namespace Apollon.Mud.Client.Data.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Bitte gib Deine E-Mail Adresse an")]
        [EmailAddress(ErrorMessage = "Bitte gib eine gültige E-Mail Adresse ein")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bitte gib ein Passwort ein!")]
        [MinLength(5, ErrorMessage = "Dein Passwort muss mindestens 5 Zeichen lang sein")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bitte bestätige Dein Passwort")]
        [CompareProperty("Password", ErrorMessage = "Die Passwörter stimmen nicht überein")]
        public string PasswordVerification { get; set; }
    }
}

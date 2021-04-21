using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bitte gib deine E-Mail Adresse ein!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bitte gib dein Passwort ein!")]
        public string Password { get; set; }
    }
}

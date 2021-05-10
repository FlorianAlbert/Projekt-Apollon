using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models.Account
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when logging in
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// He has to give his E-Mail, which has to be a valid E-Mail Adress
        /// </summary>
        [Required(ErrorMessage = "Bitte gib deine E-Mail Adresse ein!")]
        [EmailAddress(ErrorMessage = "Bitte gib eine gültige E-Mail Adresse ein")]
        public string Email { get; set; }

        /// <summary>
        /// He has to give his password
        /// </summary>
        [Required(ErrorMessage = "Bitte gib dein Passwort ein!")]
        public string Password { get; set; }
    }
}

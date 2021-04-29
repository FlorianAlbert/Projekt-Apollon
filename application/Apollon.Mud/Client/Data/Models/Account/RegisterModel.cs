﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
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
        public string Email { get; set; }

        /// <summary>
        /// He has to create a password which has to be at least 5 characters long
        /// </summary>
        [Required(ErrorMessage = "Bitte gib ein Passwort ein!")]
        [MinLength(5, ErrorMessage = "Dein Passwort muss mindestens 5 Zeichen lang sein")]
        public string Password { get; set; }

        /// <summary>
        /// He has to confirm his password, to prevent typos
        /// </summary>
        [Required(ErrorMessage = "Bitte bestätige Dein Passwort")]
        [CompareProperty("Password", ErrorMessage = "Die Passwörter stimmen nicht überein")]
        public string PasswordVerification { get; set; }
    }
}

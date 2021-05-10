using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models.Account
{
    /// <summary>
    /// This is the Model to validate the form a user has to fill out when requesting a reset link for a forgotten password
    /// </summary>
    public class ForgotPasswordModel
    {
        /// <summary>
        /// He has to give his E-Mail, which has to be a valid E-Mail Adress
        /// </summary>
        [Required(ErrorMessage = "Bitte gib Deine E-Mail Adresse an")]
        [EmailAddress(ErrorMessage = "Bitte gib eine gültige E-Mail Adresse ein")]
        public string Email { get; set; }

        /// <summary>
        /// He has to confirm his E-Mail to prevent typos. It is required and has to be identical to the first E-Mail field
        /// </summary>
        [Required(ErrorMessage = "Bitte bestätige Deine E-Mail Adresse")]
        [CompareProperty("Email", ErrorMessage = "Die E-Mail Adressen stimmen nicht überein")]
        public string EmailVerification { get; set; }
    }
}

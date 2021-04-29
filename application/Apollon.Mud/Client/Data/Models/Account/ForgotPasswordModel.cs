using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    public class ForgotPasswordModel
    {
        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Bitte gib Deine E-Mail Adresse an")]
        [EmailAddress(ErrorMessage = "Bitte gib eine gültige E-Mail Adresse ein")]
        public string Email { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        [Required(ErrorMessage = "Bitte bestätige Deine E-Mail Adresse")]
        [CompareProperty("Email", ErrorMessage = "Die E-Mail Adressen stimmen nicht überein")]
        public string EmailVerification { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Apollon.Mud.Client.Data.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Bitte gib Deine E-Mail Adresse an")]
        [EmailAddress(ErrorMessage = "Bitte gib eine gültige E-Mail Adresse ein")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bitte bestätige Deine E-Mail Adresse")]
        [CompareProperty("Email", ErrorMessage = "Die E-Mail Adresse stimmen nicht überein")]
        public string EmailVerification { get; set; }
    }
}

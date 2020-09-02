using System.ComponentModel.DataAnnotations;

namespace Apae.Models.Account
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

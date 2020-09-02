using System.ComponentModel.DataAnnotations;

namespace Apae.Models.Account
{
    public class LoginModel
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
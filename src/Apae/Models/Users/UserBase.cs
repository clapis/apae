using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Apae.Models.Users
{
    public abstract class UserBase
    {
        [Required(ErrorMessage = "{0} é compulsório")]
        [EmailAddress(ErrorMessage = "{0} nao é valido")]
        [StringLength(200, ErrorMessage = "{0} deve ter menos de 200 caracteres")]
        [Remote(action: "ValidateEmail", controller: "Users", AdditionalFields = "Id")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nome é compulsório")]
        [StringLength(100, ErrorMessage = "Nome deve ter menos de 100 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Sobrenome é compulsório")]
        [StringLength(100, ErrorMessage = "Sobrenome deve ter menos de 100 caracteres")]
        public string LastName { get; set; }

        [StringLength(100, ErrorMessage = "Telefone deve ter menos de 100 caracteres")]
        public string PhoneNumber { get; set; }
    }
}

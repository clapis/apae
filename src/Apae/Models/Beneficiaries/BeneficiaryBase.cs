using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Apae.Models.Beneficiaries
{
    public abstract class BeneficiaryBase
    {
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "{0} é compulsório")]
        [StringLength(20, ErrorMessage = "{0} deve ter menos de 20 caracteres")]
        [Remote(action: "ValidateCPF", controller: "Beneficiaries", AdditionalFields = "Id")]
        public string CPF { get; set; }

        [Display(Name = "NIS CadÚnico")]
        [Required(ErrorMessage = "{0} é compulsório")]
        [StringLength(20, ErrorMessage = "{0} deve ter menos de 20 caracteres")]
        [Remote(action: "ValidateNIS", controller: "Beneficiaries", AdditionalFields = "Id")]
        public string NIS { get; set; }

        [Required(ErrorMessage = "Data de nascimento é compulsório")]
        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "Nome é compulsório")]
        [StringLength(100, ErrorMessage = "Nome deve ter menos de 100 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Sobrenome é compulsório")]
        [StringLength(100, ErrorMessage = "Sobrenome deve ter menos de 100 caracteres")]
        public string LastName { get; set; }

        [StringLength(100, ErrorMessage = "Telefone deve ter menos de 100 caracteres")]
        public string PhoneNumber { get; set; }

        [StringLength(1000, ErrorMessage = "Telefone deve ter menos de 1000 caracteres")]
        public string Address { get; set; }

        public string Notes { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
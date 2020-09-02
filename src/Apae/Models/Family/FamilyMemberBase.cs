using System;
using System.ComponentModel.DataAnnotations;

namespace Apae.Models.Family
{
    public class FamilyMemberBase
    {
        [Required]
        public int BeneficiaryId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é compulsório")]
        [StringLength(100, ErrorMessage = "{0} deve ter menos de 100 caracteres")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "{0} é compulsório")]
        [StringLength(100, ErrorMessage = "{0} deve ter menos de 100 caracteres")]
        public string LastName { get; set; }

        [Display(Name = "Data de Nascimento")]
        //[Required(ErrorMessage = "{0} é compulsório")]
        public DateTime? Dob { get; set; }

        [Display(Name = "Observações")]
        [StringLength(1000, ErrorMessage = "{0} deve ter menos de 1000 caracteres")]
        public string Notes { get; set; }
    }
}

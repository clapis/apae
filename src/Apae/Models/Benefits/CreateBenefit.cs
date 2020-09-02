using System;
using System.ComponentModel.DataAnnotations;

namespace Apae.Models.Benefits
{
    public class CreateBenefit
    {
        [Required]
        public int BeneficiaryId { get; set; }

        [Required(ErrorMessage = "Data da entrega é compulsório")]
        public DateTime DeliveredOn { get; set; }

        [Display(Name = "Observações")]
        [StringLength(1000, ErrorMessage = "{0} deve ter menos de 1000 caracteres")]
        public string Notes { get; set; }

    }
}

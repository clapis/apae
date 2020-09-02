using System;
using System.Collections.Generic;

namespace Apae.Models.Beneficiaries
{
    public class BeneficiaryDetails : BeneficiaryBase
    {
        public int Id { get; set; }

        public List<BeneficiaryBenefit> Benefits { get; set; }

        public List<BeneficaryFamilyMember> Family { get; set; }
    }

    public class BeneficiaryBenefit
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public DateTime DeliveredOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RegisteredBy { get; set; }
    }

    public class BeneficaryFamilyMember
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string Notes { get; set; }
    }
}

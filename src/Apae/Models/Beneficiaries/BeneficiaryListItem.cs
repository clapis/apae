using System.Collections.Generic;

namespace Apae.Models.Beneficiaries
{
    public class BeneficiaryListItem
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NIS { get; set; }
        public string CPF { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public List<BeneficiaryListItemFamilyMember> Family { get; set; }
    }

    public class BeneficiaryListItemFamilyMember
    {
        public string FullName { get; set; }
    }
}

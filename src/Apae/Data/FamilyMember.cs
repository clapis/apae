using System;
using Apae.Data.Auditing;

namespace Apae.Data
{
    public class FamilyMember : IAuditable
    {
        public int Id { get; set; }

        public int BeneficiaryId { get; set; }
        public Beneficiary Beneficiary { get; set; }

        public string CPF { get; set; }
        public string NIS { get; set; }
        public DateTime? Dob { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public User LastModifiedBy { get; set; }


        public string FullName => $"{FirstName} {LastName}";
    }
}

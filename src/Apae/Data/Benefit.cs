using System;
using Apae.Data.Auditing;

namespace Apae.Data
{
    public class Benefit : IAuditable
    {
        public int Id { get; set; }

        public BenefitType BenefitType { get; set; }
        public DateTime DeliveredOn { get; set; }
        public string Notes { get; set; }

        public int BeneficiaryId { get; set; }
        public Beneficiary Beneficiary { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public User LastModifiedBy { get; set; }
    }
}
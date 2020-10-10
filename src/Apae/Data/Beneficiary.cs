using System;
using System.Collections.Generic;
using Apae.Data.Auditing;

namespace Apae.Data
{
    public class Beneficiary : IAuditable
    {
        public int Id { get; set; }

        public string CPF { get; set; }
        public string NIS { get; set; }
        public DateTime? Dob { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }

        public List<Benefit> Benefits { get; set; } = new List<Benefit>();
        public List<FamilyMember> Family { get; set; } = new List<FamilyMember>();

        public DateTime CreatedOn { get; set; } 
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public Guid LastModifiedById { get; set; }
        public User LastModifiedBy { get; set; }
    }
}

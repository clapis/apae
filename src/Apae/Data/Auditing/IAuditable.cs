using System;

namespace Apae.Data.Auditing
{
    public interface IAuditable
    {
        DateTime CreatedOn { get; set; }
        Guid CreatedById { get; set; }
        User CreatedBy { get; set; }

        DateTime LastModifiedOn { get; set; }
        Guid LastModifiedById { get; set; }
        User LastModifiedBy { get; set; }
    }
}

using System;
using Microsoft.AspNetCore.Identity;

namespace Apae.Data
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}

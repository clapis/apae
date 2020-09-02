using System.Collections.Generic;

namespace Apae.Authorization
{
    public static class Roles
    {
        public const string Admin = "Admin";


        public static IEnumerable<string> GetAll()
        {
            yield return Admin;
        }
    }
}

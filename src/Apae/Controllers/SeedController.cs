using System.Linq;
using System.Threading.Tasks;
using Apae.Data;
using Microsoft.AspNetCore.Mvc;
using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.AspNetCore.Authorization;
using Apae.Authorization;

namespace Apae.Controllers
{
    [Route("seed")]
    [Authorize(Roles = Roles.Admin)]
    public class SeedController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SeedController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Seed(int n = 100)
        {
            var users = Enumerable
                .Range(0, n)
                .Select(i => new Faker("es"))
                .Select(faker => new Beneficiary
                {
                    CPF = faker.Person.Cpf(),
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    PhoneNumber = faker.Phone.PhoneNumber(),
                    Address = faker.Address.StreetAddress()
                }).ToList();

            _db.Beneficiaries.AddRange(users);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Beneficiaries");
        }
    }
}

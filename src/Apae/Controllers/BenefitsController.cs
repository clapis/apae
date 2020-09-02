using System;
using System.Threading.Tasks;
using Apae.Authorization;
using Apae.Data;
using Apae.Extensions;
using Apae.Models.Benefits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apae.Controllers
{
    [Route("beneficios")]
    public class BenefitsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BenefitsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBenefit model)
        {
            var beneficiary = await _db.Beneficiaries.FindAsync(model.BeneficiaryId);

            if (beneficiary == null) return NotFound();

            beneficiary.Benefits.Add(new Benefit
            {
                Notes = model.Notes,
                BenefitType = BenefitType.BasketOfBasicGoods,
                DeliveredOn = model.DeliveredOn.FromBrazilToUtc()
            }); ;

            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Beneficiaries", new {id = beneficiary.Id});
        }

        [ValidateAntiForgeryToken]
        [HttpPost("{id:int}/remover")]
        [Authorize(Policy = Policies.CanDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            var benefit = await _db.Benefits.FindAsync(id);

            if (benefit == null) return NotFound();

            _db.Benefits.Remove(benefit);

            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Beneficiaries", new { id = benefit.BeneficiaryId });
        }


    }
}

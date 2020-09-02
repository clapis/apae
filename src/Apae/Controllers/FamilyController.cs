using System.Threading.Tasks;
using Apae.Data;
using Apae.Models.Family;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Apae.Controllers
{
    [Route("familiares")]
    public class FamilyController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public FamilyController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFamilyMember model)
        {
            var beneficiary = await _db.Beneficiaries.FindAsync(model.BeneficiaryId);

            if (beneficiary == null) return NotFound();

            var member = _mapper.Map<FamilyMember>(model);

            beneficiary.Family.Add(member);

            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Beneficiaries", new {id = beneficiary.Id});
        }

        [HttpGet("{id:int}/editar")]
        public async Task<IActionResult> Update(int id)
        {
            var person = await _db.FamilyMembers.FindAsync(id);

            if (person == null) return NotFound();

            var viewmodel = _mapper.Map<UpdateFamilyMember>(person);

            return View(viewmodel);
        }


        [ValidateAntiForgeryToken]
        [HttpPost("{id:int}/editar")]
        public async Task<IActionResult> Update(int id, UpdateFamilyMember model)
        {
            if (!ModelState.IsValid) return View(model);

            var person = await _db.FamilyMembers.FindAsync(id);

            if (person == null) return NotFound();

            _mapper.Map(model, person);

            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Beneficiaries", new { id = person.BeneficiaryId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost("{id:int}/remover")]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _db.FamilyMembers.FindAsync(id);

            if (person == null) return NotFound();

            _db.FamilyMembers.Remove(person);

            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Beneficiaries", new { id = person.BeneficiaryId });
        }



    }
}

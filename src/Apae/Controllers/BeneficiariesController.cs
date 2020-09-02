using System.Collections.Generic;
using System.Threading.Tasks;
using Apae.Data;
using Apae.Models.Beneficiaries;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Apae.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

namespace Apae.Controllers
{
    [Route("beneficiarios")]
    public class BeneficiariesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;

        public BeneficiariesController(IMapper mapper, ApplicationDbContext db)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var items = await _db.Beneficiaries
                                .Include(b => b.Family)
                                .ToListAsync();

            var viewmodel = _mapper.Map<List<BeneficiaryListItem>>(items);

            return View(viewmodel);
        }

        [HttpGet("cadastrar")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("cadastrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBeneficiary model)
        {
            if (!ModelState.IsValid) return View(model);

            var person = _mapper.Map<Beneficiary>(model);

            _db.Beneficiaries.Add(person);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            
            var person = await _db.Beneficiaries
                .Include(b => b.Family)
                .ThenInclude(b => b.CreatedBy)
                .SingleOrDefaultAsync(x => x.Id == id);

            // load benefits for last 3 months
            _db.Entry(person)
                .Collection(p => p.Benefits)
                .Query()
                .Where(p => p.DeliveredOn > DateTime.UtcNow.AddMonths(-3))
                .Load();

            if (person == null) return NotFound();

            var viewmodel = _mapper.Map<BeneficiaryDetails>(person);

            return View(viewmodel);
        }


        [HttpGet("{id:int}/editar")]
        public async Task<IActionResult> Update(int id)
        {
            var person = await _db.Beneficiaries.FindAsync(id);

            if (person == null) return NotFound();

            var viewmodel = _mapper.Map<UpdateBeneficiary>(person);

            return View(viewmodel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("{id:int}/editar")]
        public async Task<IActionResult> Update(int id, UpdateBeneficiary model)
        {
            if (!ModelState.IsValid) return View(model);

            var person = await _db.Beneficiaries.FindAsync(id);

            if (person == null) return NotFound();

            _mapper.Map(model, person);

            await _db.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }

        [ValidateAntiForgeryToken]
        [HttpPost("{id:int}/remover")]
        [Authorize(Policy = Policies.CanDelete)]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _db.Beneficiaries.FindAsync(id);

            if (person == null) return NotFound();

            _db.Beneficiaries.Remove(person);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Route("validar/nis")]
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> ValidateNIS(int id, string nis)
        {
            // TODO: sanitize input & create an index on NIS
            var isUnique = !(await _db.Beneficiaries.AnyAsync(b => b.NIS == nis && b.Id != id));

            return isUnique ? Json(true) : Json("NIS CadÚnico já está cadastrado");
        }


        [Route("validar/cpf")]
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> ValidateCPF(int id, string cpf)
        {
            // TODO: sanitize input & create an index on CPF
            var isUnique = !(await _db.Beneficiaries.AnyAsync(b => b.CPF == cpf && b.Id != id));

            return isUnique ? Json(true) : Json("CPF já está cadastrado");
        }

    }
}

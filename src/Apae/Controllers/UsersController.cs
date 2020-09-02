using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apae.Authorization;
using Apae.Data;
using Apae.Extensions;
using Apae.Models.Users;
using Apae.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Apae.Controllers
{
    [Route("usuarios")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public UsersController(
            IMapper mapper,
            UserManager<User> userManager,
            IEmailSender emailSender)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var viewmodel = _mapper.Map<List<UserListItem>>(users);

            return View(viewmodel);
        }

        [HttpGet("cadastrar")]
        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Create(CreateUser model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                Email = model.Email, 
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                throw new Exception($"Create user failed: {result}");

            var current = await _userManager.FindByIdAsync(User.GetUserId().ToString());

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token }, protocol: HttpContext.Request.Scheme);

            await _emailSender.SendInvitationAsync(user, current.FullName, callbackUrl);

            return RedirectToAction("Index");
        }

        [HttpGet("{id:guid}/editar")]
        public async Task<IActionResult> Update(Guid id)
        { 
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound();

            var viewmodel = _mapper.Map<UpdateUser>(user);

            return View(viewmodel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("{id:guid}/editar")]
        public async Task<IActionResult> Update(Guid id, UpdateUser model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }


        [ValidateAntiForgeryToken]
        [HttpPost("{userId:guid}/remover")]
        [Authorize(Policy = Policies.CanDelete)]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new Exception($"Delete user failed: {result}");

            return RedirectToAction("Index");
        }

        [Route("validar/email")]
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> ValidateEmail(Guid id, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var isUnique = user == null || user.Id == id;

            return isUnique ? Json(true) : Json("Email já está cadastrado");
        }

    }
}
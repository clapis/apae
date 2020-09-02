using System.Threading.Tasks;
using Apae.Data;
using Apae.Models.Account;
using Apae.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Apae.Controllers
{
    [Route("{controller}")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;


        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email ou senha incorretos");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet("{action}")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("{action}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return RedirectToAction("ForgotPasswordConfirmation", new { email = model.Email });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var callbackUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token }, protocol: HttpContext.Request.Scheme);

            await _emailSender.SendResetPasswordAsync(user, callbackUrl);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        [HttpGet("{action}")]
        public IActionResult ForgotPasswordConfirmation(string email)
        {
            return View(model: email);
        }


        [AllowAnonymous]
        [HttpGet("{action}")]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                return RedirectToAction("ForgotPassword");

            return View();
        }



        [AllowAnonymous]
        [HttpPost("{action}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
            {
                AddIdentityResultErrors(result);

                return View(model);
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }

        [AllowAnonymous]
        [HttpGet("{action}")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }

        private void AddIdentityResultErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
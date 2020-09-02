using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Apae.Models;
using Microsoft.AspNetCore.Authorization;

namespace Apae.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Beneficiaries");
        }

        [AllowAnonymous]
        [HttpGet("error/{status:int?}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? status = 500)
        {
            return View(new ErrorViewModel
            {
                Status = status,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}

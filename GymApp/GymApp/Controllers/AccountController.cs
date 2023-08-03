using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}

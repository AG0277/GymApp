
using GymApp.Data;
using GymApp.Models;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApplicationDbContext context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel LoginViewModel)
        {

            if(!ModelState.IsValid)
                return View(LoginViewModel);

            var user = await userManager.FindByEmailAsync(LoginViewModel.Email);

            if(user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, LoginViewModel.Password);
                if(passwordCheck)
                {
                    var result = await signInManager.PasswordSignInAsync(user, LoginViewModel.Password, false, false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Meal");
                    }
                }
                TempData["Error"] = "Wrong credentials, please try again";
                return View(LoginViewModel);
            }
            TempData["Error"] = "Could not find Email, please try again";
            return View(LoginViewModel);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel RegisterViewModel)
        {
            if (!ModelState.IsValid)
                return View(RegisterViewModel);


            var user = await userManager.FindByEmailAsync(RegisterViewModel.Email);

            if (user != null)
            {
                TempData["Error"] = "Email adress is already used";
                return View(RegisterViewModel);
            }

            var newUser = new AppUser()
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };

            var userResponse = await userManager.CreateAsync(newUser, RegisterViewModel.Password);
            if (userResponse.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, UserRoles.User);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in userResponse.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(RegisterViewModel);
            }
        }


        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
    }
}

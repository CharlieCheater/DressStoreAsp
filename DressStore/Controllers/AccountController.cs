using DressStore.Global;
using DressStore.Infrastructure;
using DressStore.Infrastructure.Models;
using DressStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DressStore.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly ApplicationContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user == null)
            {
                ModelState.AddModelError("Username", ErrorMessages.UserBadLogin);
                return View(viewModel);
            }
            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, true, false);
            if (result.Succeeded)
            {
                return Redirect(viewModel.ReturnUrl ?? "/");
            }
            ModelState.AddModelError("Username", ErrorMessages.UserBadLogin);
            return View(viewModel);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SignOut(LoginViewModel viewModel)
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}

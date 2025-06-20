using System.Security.Claims;
using HeadacheTracker.Models;
using HeadacheTracker.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeadacheTracker.Controllers {

    [Authorize]

    public class AccountController : Controller {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/") {
            var model = new LoginViewModel {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login) {
            if (ModelState.IsValid) {
                AppUser userToLogin = await _userManager.FindByNameAsync(login.UserName);
                if (userToLogin != null) {
                    var signInResult = await _signInManager.PasswordSignInAsync(userToLogin, login.Password, login.Remember, false);
                    if (signInResult.Succeeded) {
                        return Redirect(login.ReturnUrl ?? "/");
                    }
                }
            }
            ModelState.AddModelError("", "Neplatné přihlašovací údaje.");
            login.Password = string.Empty;
            return View(login);
        }

        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

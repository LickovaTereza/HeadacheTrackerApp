using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HeadacheTracker.Models;
using HeadacheTracker.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HeadacheTracker.Controllers {
    public class UsersController : Controller {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        private IPasswordValidator<AppUser> _passwordValidator;
        private AppDbContext _dbContext;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator, AppDbContext dbContext) {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin, AdminView")]
        public IActionResult Index() {
            return View(_userManager.Users);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserViewModel newUser) {
            if (ModelState.IsValid) {
                AppUser userToAdd = new AppUser {
                    UserName = newUser.Name,
                    Email = newUser.Email

                };
                // pokus o zápis nového uživatele do databáze
                IdentityResult result = await _userManager.CreateAsync(userToAdd, newUser.Password);
                if (result.Succeeded) {
                    return RedirectToAction("Index", "Home");

                }
                else {
                    AddIdentityErrors(result);
                }
            }
            return View(newUser);
        }

        [AllowAnonymous]
        private void AddIdentityErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditAsync(string id) {
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("AdminView");

            // Pokud nejsem admin a chci upravit někoho jiného
            if (!isAdmin && currentUser.Id != id) {
                return Forbid(); // nebo RedirectToAction("AccessDenied")
            }

            var userToEdit = await _userManager.FindByIdAsync(id);
            if (userToEdit == null) {
                return NotFound();
            }

            return View(userToEdit);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, string email, string password) {
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUser.Id != id) {
                return Forbid(); 
            }

            var userToEdit = await _userManager.FindByIdAsync(id);
            if (userToEdit == null) {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(email)) {
                userToEdit.Email = email;
            }
            else {
                ModelState.AddModelError("", "E-mail nemůže být prázdný.");
            }

            IdentityResult validPass = null;
            if (!string.IsNullOrEmpty(password)) {
                validPass = await _passwordValidator.ValidateAsync(_userManager, userToEdit, password);
                if (validPass.Succeeded) {
                    userToEdit.PasswordHash = _passwordHasher.HashPassword(userToEdit, password);
                }
                else {
                    AddIdentityErrors(validPass);
                }
            }
            else {
                ModelState.AddModelError("", "Heslo nemůže být prázdné.");
            }

            if (ModelState.IsValid && (validPass == null || validPass.Succeeded)) {
                IdentityResult result = await _userManager.UpdateAsync(userToEdit);
                if (result.Succeeded) {
                    if (isAdmin) return RedirectToAction("Index");
                    return RedirectToAction("Index", "Home"); // běžného uživatele pošle jinam
                }
                else {
                    AddIdentityErrors(result);
                }
            }
            return View(userToEdit);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id) {
            var userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete == null) {
                ModelState.AddModelError("", "Uživatel nenalezen.");
                return View("Index", _userManager.Users);
            }

            // 1. Smazat všechny HeadacheRecords uživatele
            var headacheRecords = _dbContext.HeadacheRecords.Where(hr => hr.UserId == id);
            _dbContext.HeadacheRecords.RemoveRange(headacheRecords);

            // 2. Smazat všechny Treatments uživatele
            var treatments = _dbContext.Treatments.Where(t => t.UserId == id);
            _dbContext.Treatments.RemoveRange(treatments);

            // 3. Smazat všechny Triggers uživatele
            var triggers = _dbContext.Triggers.Where(tr => tr.UserId == id);
            _dbContext.Triggers.RemoveRange(triggers);

            // 4. Smazat všechny Medications uživatele
            var medications = _dbContext.Medications.Where(m => m.UserId == id);
            _dbContext.Medications.RemoveRange(medications);

            // Uložit změny (vymazání závislých dat)
            await _dbContext.SaveChangesAsync();

            // 5. Smazat uživatele
            IdentityResult result = await _userManager.DeleteAsync(userToDelete);

            if (result.Succeeded) {
                return RedirectToAction("Index");
            }
            else {
                AddIdentityErrors(result);
            }

            return View("Index", _userManager.Users);
        }
    }
}

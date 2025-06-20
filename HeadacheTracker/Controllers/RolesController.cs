using AspNetCoreMVC_SchoolSystem.Models;
using HeadacheTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeadacheTracker.Controllers {

    [Authorize(Roles = "Admin, AdminView")]

    public class RolesController : Controller {
        RoleManager<IdentityRole> _roleManager;
        UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() {
            return View(_roleManager.Roles.OrderBy(role => role.Name));
        }
        
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name) {
            if (ModelState.IsValid) {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    AddIdentityErrors(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id) {
            IdentityRole roleToDelete = await _roleManager.FindByIdAsync(id);
            if (roleToDelete != null) {
                var result = await _roleManager.DeleteAsync(roleToDelete);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    AddIdentityErrors(result);
                }
            }
            ModelState.AddModelError("", "Role not found");
            return View("Index");
        }

        public async Task<IActionResult> EditAsync(string id) {
            IdentityRole roleToEdit = await _roleManager.FindByIdAsync(id);
            if (roleToEdit == null) {
                return NotFound();
            }

            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users) {
                if (await _userManager.IsInRoleAsync(user, roleToEdit.Name)) {
                    members.Add(user);
                }
                else {
                    nonMembers.Add(user);
                }
            }

            return View(new RoleState {
                Members = members,
                NonMembers = nonMembers,
                Role = roleToEdit,
            });
        }


        [HttpPost]
        public async Task<IActionResult> EditAsync(RoleModification roleModification) {
            if (ModelState.IsValid) {
                foreach (string userId in roleModification.AddIds ?? new string[] { }) {
                    AppUser userToAdd = await _userManager.FindByIdAsync(userId);
                    if (userToAdd != null) {
                        IdentityResult result = await _userManager.AddToRoleAsync(userToAdd, roleModification.RoleName);
                        if (!result.Succeeded) {
                            AddIdentityErrors(result);
                        }
                    }
                }
                foreach (string userId in roleModification.DeleteIds ?? new string[] { }) {
                    AppUser userToDelete = await _userManager.FindByIdAsync(userId);
                    if (userToDelete != null) {
                        IdentityResult result = await _userManager.RemoveFromRoleAsync(userToDelete, roleModification.RoleName);
                        if (!result.Succeeded) {
                            AddIdentityErrors(result);
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Špatně zadaná změna, zkontroluj údaje.");
            return RedirectToAction("Index");
        }

        private void AddIdentityErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}

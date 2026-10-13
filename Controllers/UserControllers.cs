using homework.Data;
using homework.Enums;
using homework.Models;
using homework.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace homework.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            ApplicationDbContext db,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _db.Users
                .Where(user => !user.IsDelete)
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                    CreatedAt = user.CreatedAt
                })
                .ToListAsync();

            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreatUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatUserViewModel input)
        {
            if (!ModelState.IsValid)
                return View(input);

            var user = new User
            {
                UserName = input.Email,
                Email = input.Email,
                EmailConfirmed = true,
                PhoneNumber = input.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UserType = input.UserType
            };

            var createResult =
                await _userManager.CreateAsync(user, input.Password);

            if (!createResult.Succeeded)
            {
                AddErrors(createResult);
                return View(input);
            }

            string roleName;

            if (input.UserType == UserType.Admin)
                roleName = "Admin";
            else if (input.UserType == UserType.Employee)
                roleName = "Employee";
            else
            {
                await _userManager.DeleteAsync(user);
                ModelState.AddModelError("", "Choose a valid user role.");
                return View(input);
            }

            var roleResult =
                await _userManager.AddToRoleAsync(user, roleName);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                AddErrors(roleResult);
                return View(input);
            }

            TempData["msg"] =
                $"User created successfully with the {roleName} role.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null || user.IsDelete)
                return NotFound();

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["error"] =
                    "You cannot delete the account you are currently using.";

                return RedirectToAction(nameof(Index));
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var admins =
                    await _userManager.GetUsersInRoleAsync("Admin");

                if (admins.Count(admin => !admin.IsDelete) <= 1)
                {
                    TempData["error"] =
                        "The last Admin account cannot be deleted.";

                    return RedirectToAction(nameof(Index));
                }
            }

            user.IsDelete = true;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            TempData["msg"] = "User deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> InitRoles()
        {
            var roles = new List<string> { "Admin", "Employee" };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var result = await _roleManager.CreateAsync(
                        new IdentityRole(roleName));

                    if (!result.Succeeded)
                    {
                        TempData["error"] = string.Join(
                            " ",
                            result.Errors.Select(error => error.Description));

                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            TempData["msg"] = "Admin and Employee roles are ready.";
            return RedirectToAction(nameof(Index));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
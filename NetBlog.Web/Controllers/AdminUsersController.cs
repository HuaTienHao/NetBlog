using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Web.Models.ViewModels;
using NetBlog.Web.Services;

namespace NetBlog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminUsersController(IUserService userService, UserManager<IdentityUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List(
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageSize = 5,
            int pageNumber = 1)
        {
            var totalRecords = await _userService.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
                pageNumber--;

            if (pageNumber < 1)
                pageNumber++;

            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            var users = await _userService.GetAll(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {   
                usersViewModel.Users.Add(new User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress = user.Email,
                    IsAdmin = (await _userManager.GetRolesAsync(user)).Any(r => r.Contains("Admin"))
                });
            }

            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("List");
            }

            var identityUser = new IdentityUser
            {
                UserName = userViewModel.Username,
                Email = userViewModel.Email
            };
            var identityResult = await _userManager.CreateAsync(identityUser, userViewModel.Password);

            if (identityResult is not null)
            {
                if (identityResult.Succeeded)
                {
                    var roles = new List<string>
                    {
                        "User"
                    };

                    if (userViewModel.AdminRoleCheckBox)
                    {
                        roles.Add("Admin");
                    }

                    identityResult = await _userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List");
                    }
                }
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is not null)
            {
                var identityResult = await _userManager.DeleteAsync(user);

                if (identityResult is not null && identityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }

            return View();
        }
    }
}

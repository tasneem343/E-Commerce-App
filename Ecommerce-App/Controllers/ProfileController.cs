using DataAccessLayer.Entities;
using Ecommerce_App.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_App.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var viewModel = new UserProfileViewModel
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                UserType = user.UserType
            };

            return View(viewModel);
        }
        [HttpGet]
        public async Task< IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Content("User not found");
            var viewModel = new EditUserProfileViewModel
            {
                FullName = user.FullName,
                Address = user.Address
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProfile(EditUserProfileViewModel uservm)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Content("User not found");
                user.FullName = uservm.FullName;
                user.Address = uservm.Address;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Profile");
            }
            return RedirectToAction("EditProfile",uservm);

        }
    }

}

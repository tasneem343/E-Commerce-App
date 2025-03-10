using DataAccessLayer.Entities;
using Ecommerce_App.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> Manager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        public AccountController(
            UserManager<ApplicationUser> manager,
            SignInManager<ApplicationUser> signIn
            )
        {
            Manager = manager;
            SignInManager = signIn;
        }
        
        [HttpGet]
        public IActionResult Register()
        {

            return View("Register");
        }
        [HttpPost]

        public async Task<IActionResult> SaveRegister(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = register.UserName;
                user.Email = register.Email;
                user.PasswordHash = register.Password;
                

                //save database
                IdentityResult result = await Manager.CreateAsync(user, register.Password);



                //cookie
                if (result.Succeeded)
                {
                    //assign role 
                    var roleResult = await Manager.AddToRoleAsync(user, "Admin");
                    if (roleResult.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false);
                        return RedirectToAction("SignOut");
                    }
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View("Register", register);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveLogin(LoginUserViewModel userViewModel)
        {
            if (ModelState.IsValid == true)
            {
                //check found 
                ApplicationUser appUser =
                    await Manager.FindByNameAsync(userViewModel.Name);
                if (appUser != null)
                {
                    bool found =
                         await Manager.CheckPasswordAsync(appUser, userViewModel.Password);
                    if (found == true)
                    {
                        List<Claim> Claims = new List<Claim>();
                        Claims.Add(new Claim("UserAddress", appUser.Address));
                        Claims.Add(new Claim("Email", appUser.Email));
                        Claims.Add(new Claim("UserType", appUser.UserType));
                        Claims.Add(new Claim("FullName", appUser.FullName));

                        await SignInManager.SignInWithClaimsAsync(appUser, userViewModel.RememberMe, Claims);
                        return RedirectToAction("Index", "Home");
                    }

                }
                ModelState.AddModelError("", "Username OR PAssword wrong");
                //create cookie
            }
            return View("Login", userViewModel);
        }

        public async Task<IActionResult> SignOut()
        {
            await SignInManager.SignOutAsync();
            return View("Login");
        }
    }
}
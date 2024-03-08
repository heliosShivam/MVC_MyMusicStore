using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_MyMusicStore.Models;
using MVC_MyMusicStore.Models.UserModels;

namespace MVC_MyMusicStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _sm;
        private readonly UserManager<AppUser> _um;

        public AccountController(SignInManager<AppUser> sm, UserManager<AppUser> um)
        {
            _sm = sm;
            _um = um;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login user)
        {
            if (ModelState.IsValid)
            {
                var result = await _sm.PasswordSignInAsync(user.Username!, user.Password!, user.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Please enter correct credentials");
                    return View(user);
                }
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register Nuser)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Name = Nuser.Name,
                    Email = Nuser.Email,
                    UserName = Nuser.Email
                };

                var result = await _um.CreateAsync(user, Nuser.Password!);

                if (result.Succeeded)
                {

                    await _sm.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }
            }


            return View(Nuser);
        }


        public async Task<IActionResult> Logout()
        {
            await _sm.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

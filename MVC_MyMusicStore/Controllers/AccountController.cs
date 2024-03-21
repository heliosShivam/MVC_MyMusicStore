using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
                var lastVisitedPage = HttpContext.Session.GetString("LastVisitedPage");
                Console.WriteLine("Visited" + lastVisitedPage);
                if (result.Succeeded && !string.IsNullOrEmpty(lastVisitedPage))
                {

                    HttpContext.Session.Remove("LastVisitedPage");
                    Console.WriteLine("Visited after login" + lastVisitedPage);

                    return Redirect(lastVisitedPage);
                }
                else if(result.Succeeded && lastVisitedPage == null)

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


        /*start of reset password by generating a reset link*/


        [HttpGet]
        public IActionResult GetResetLink()
        {
            return View();
        }

        //Generate token and assign it to link

        [HttpPost]
        public async Task<IActionResult> GetResetLink (string email)
        {
            
            if(ModelState.IsValid)
            {
                var user = await _um.FindByEmailAsync(email);
                if (user != null)
                {
                    var resetToken = await _um.GeneratePasswordResetTokenAsync(user);

                    
                    return RedirectToAction("ResetPasswordLink", new {email, resetToken}); ;
                }
                else
                {
                    ModelState.AddModelError("", "Please enter correct email");
                    return View();
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Please enter valid email");
                return View();
            }
            
        }

        //view the link on a page and on clicking go to next page to set new password

        [HttpGet]
        public async Task<IActionResult> ResetPasswordLink(string email, string resetToken)
        {
            //var resetLink = Url.Action("ResetPassword", "Account", new { email, resetToken }, Request.Scheme);
            //ViewData["ResetLink"] = resetLink;
            //return View();

            ViewBag.Email = email;
            ViewBag.ResetToken = resetToken;

            return View();
        }

        //it is final page which is used to set new password with help of resetpassword inbuilt method

        [HttpGet]
        public IActionResult ResetPassword(string resetToken ,string email)
        {
            ViewBag.ResetToken = resetToken; ViewBag.Email = email;
            Console.WriteLine("reset token : " + resetToken);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string resetToken, string newPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _um.FindByEmailAsync(email);

                if (user != null)
                {
                    // Use the provided reset token
                    var result = await _um.ResetPasswordAsync(user, resetToken, newPassword);

                    if (result.Succeeded)
                    {
                        Console.WriteLine("Successssssssssssssss");
                        // Password reset successful
                        return RedirectToAction("SuccessfullReset");
                    }
                    else
                    {
                        //Error in reset
                        Console.WriteLine("ERROR UPDATING PASSWORD");
                        
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    // User not found
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    Console.WriteLine($"Property: {value.AttemptedValue}, Error: {error.ErrorMessage}");
                }
            }

            // Return the same view with model if ModelState is not valid
            Console.WriteLine("Invalid Model State");
            return View();
        }



        /*[HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _um.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var resetToken = await _um.GeneratePasswordResetTokenAsync(user);
                    Console.WriteLine("Reset Token : " + resetToken);
                    var result = await _um.ResetPasswordAsync(user, resetToken, model.NewPassword);

                    if (result.Succeeded)
                    {
                        // Password reset successful
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        // Password reset failed
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    // User not found
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }


            return View(model);
        }*/

        //on successfull reset redirect here
        public IActionResult SuccessfullReset()
        {
            return View();
        }


        //logout
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("LastVisitedPage");
            await _sm.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

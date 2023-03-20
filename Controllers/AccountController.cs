using MeetGreet.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MeetGreet.Controllers
{
    public class AccountController : Controller
    {

        // TODO: Apparently my SignInManager/UserManager and my framework version is mixed up. Gotta fix that.
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
        }



        /// <summary>
        /// Returns to the view "Login".
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Depending on the status of the users attempt to login, they'll either be redirected to
        /// the home page ("Home/Index" if login succeeded) or back to the Login page ("Account/Login" if failed).
        /// </summary>
        /// <param name="loginCredentials"></param>
        /// <param name="returnURL"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        // GET: Login
        public async Task<ActionResult> Authentication(UserInfo loginCredentials, string returnURL)
        {
            
            var result = await signInManager.PasswordSignInAsync(loginCredentials.UserName, loginCredentials.HashedPassword, true, true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Login");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GroupShareKitSample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Sdl.Community.GroupShareKit;
using Sdl.Community.GroupShareKit.Http;


namespace GroupShareKitSample.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        public UserManagerKit _userManager;

        public static IEnumerable<string> AllScopes =
           new[]
           {
                "ManagementRestApi",
                "ProjectServerRestApi",
                "MultiTermRestApi",
                "MultiTermRestApi"
           };
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Account account)
        {

            if (ModelState.IsValid )
            {
                if (Uri.IsWellFormedUriString(account.GsLink, UriKind.Absolute))
                {
                    var userStore= new UserStore();
                    _userManager = new UserManagerKit(userStore);

                    var gsClient = await _userManager.GetGroupShareClient(account);

                    var user = await userStore.FindAsync(account.UserName, account.Password,gsClient);
                    ModelState.Clear();
                   
                    if (user != null)
                    {
                        
                        await SignInAsync(user, false);

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Wrong username or password ");

                }
               ModelState.AddModelError("","Incorrect link format");
                return View("Login");
            }
            ModelState.AddModelError("", "All fields are required");
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private async Task  SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent}, identity);
        }

        
    }

  
}
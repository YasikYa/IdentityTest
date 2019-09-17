using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityTest.Models;
using System.Threading.Tasks;
using IdentityTest.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;

namespace IdentityTest.Controllers
{
    [Authorize]
    public class AccauntController : Controller
    {
        // GET: Accaunt
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Acces denied." });
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }
        
        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            AppUser user = await UserManager.FindAsync(model.Name, model.Password);

            if(user == null)
            {
                ModelState.AddModelError("", "Wrong E-mail or Password");
            }
            else
            {
                ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);

                ident.AddClaims(ClaimsProvider.GetClaims(ident));

                AuthManager.SignOut();
                AuthManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, ident);
                return Redirect(returnUrl);
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GoogleLogin(string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback", new { returnUrl = returnUrl })
            };

            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }

        [AllowAnonymous]
        public async Task<ActionResult> GoogleLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
            AppUser user = await UserManager.FindAsync(loginInfo.Login);

            if(user == null)
            {
                user = new AppUser
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.DefaultUserName,
                };

                IdentityResult result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
                else
                {
                    result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }

            ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                DefaultAuthenticationTypes.ApplicationCookie);

            ident.AddClaims(loginInfo.ExternalIdentity.Claims);

            AuthManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, ident);

            return Redirect(returnUrl ?? "/");
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}
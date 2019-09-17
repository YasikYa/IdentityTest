using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityTest.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using IdentityTest.Models;
using Microsoft.AspNet.Identity;

namespace IdentityTest.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorFromResult(result);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            AppUser user = UserManager.FindById(id);
            if(user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new []{ "User not found" });
            }
        }

        public ActionResult Edit(string id)
        {
            AppUser user = UserManager.FindById(id);
            if(user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string Id, string Email, string password)
        {
            AppUser user = await UserManager.FindByIdAsync(Id);
            if(user != null)
            {
                user.Email = Email;
                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                {
                    AddErrorFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if(password != string.Empty)
                {
                    validPass = await UserManager.PasswordValidator.ValidateAsync(password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorFromResult(validPass);
                    }
                }

                if((validEmail.Succeeded && validPass == null) || 
                    (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }
            return View(user);
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private void AddErrorFromResult(IdentityResult result)
        {
            foreach(string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityTest.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using IdentityTest.Models;

namespace IdentityTest.Controllers
{
    public class RoleAdminController : Controller
    {
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<AppRoleManager>();
            }
        }

        // GET: RoleAdmin
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new AppRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddModelError(result);
                }
            }
            return View(name);
        }

        public ActionResult Delete(string id)
        {
            AppRole role = RoleManager.FindById(id);
            if(role != null)
            {
                IdentityResult result = RoleManager.Delete(role);
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
                return View("Error", new string[] { "Role not found" });
            }
        }

        public ActionResult Edit(string id)
        {
            AppRole role = RoleManager.FindById(id);

            var memebersId = role.Users.Select(u => u.UserId).ToArray();

            var members = UserManager.Users.Where(u => memebersId.Any(x => x == u.Id));

            var nonMembers = UserManager.Users.Except(members);

            EditRoleModel model = new EditRoleModel
            {
                Role = role,
                usersInRole = members,
                usersNotInRole = nonMembers

            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach(var id in model.IdsToAdd ?? new string[] { })
                {
                    result = UserManager.AddToRole(id, model.RoleName);
                    if (!result.Succeeded)
                    {
                        AddModelError(result);
                    }
                }

                foreach(var id in model.IdsToDelete ?? new string[] { })
                {
                    result = UserManager.RemoveFromRole(id, model.RoleName);
                    if (!result.Succeeded)
                    {
                        AddModelError(result);
                    }
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
            ModelState.AddModelError("", "Role not found");
            return View("Error");
        }

        private void AddModelError(IdentityResult result)
        {
            foreach(string error in result.Errors)
            {
                ModelState.AddModelError("", "error");
            }
        }
    }
}
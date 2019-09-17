using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityTest.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace IdentityTest.Controllers
{
    public class HomeController : Controller
    {
        AppIdentityDbContext context = new AppIdentityDbContext();

        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            return View(GetData("Index"));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult OtherAction()
        {
            return View("Index", GetData("OtherAction"));
        }

        [Authorize]
        public ActionResult Upload()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            string fileName = upload.FileName;
            
            string userName = this.User.Identity.Name;
            var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

            var user = userManager.FindByName(userName);
            var file = new Entity.File()
            {
                FileName = fileName,
                UserId = user.Id
            };

            context.Files.Add(file);

            context.SaveChanges();
            SaveFile(upload, file.FileId.ToString());
            return RedirectToAction("FileUsers");
        }

        public ActionResult FileUsers()
        {
            var files = context.Files.Include("User").ToList();
            return View(files);
        }

        private Dictionary<string, object> GetData(string action)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();

            model["Action"] = action;
            model["User"] = HttpContext.User.Identity.Name;
            model["Authenticated"] = HttpContext.User.Identity.IsAuthenticated;
            model["AuthType"] = HttpContext.User.Identity.AuthenticationType;
            model["Is in role - Admin"] = HttpContext.User.IsInRole("Admin");

            return model;
        }

        private void SaveFile(HttpPostedFileBase file, string fileId)
        {
            string path = @"E:\Programming\Tests\IdentityTest\IdentityTest\UserFiles\" + fileId + ".pdf";
            file.SaveAs(path);
        }
    }
}
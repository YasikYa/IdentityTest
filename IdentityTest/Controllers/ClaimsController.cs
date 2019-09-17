using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

namespace IdentityTest.Controllers
{
    public class ClaimsController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ClaimsIdentity ident = HttpContext.User.Identity as ClaimsIdentity;
            
            if(ident == null)
            {
                return View("Error", new string[] { "No available claims." });
            }
            else
            {
                return View(ident.Claims);
            }
        }
    }
}
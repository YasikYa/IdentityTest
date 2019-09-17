using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Reflection;
using System.Security.Claims;

namespace IdentityTest.Infrastructure
{
    public static class IdentityHelpers
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, string userId)
        {
            AppUserManager manager = HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();

            return new MvcHtmlString(manager.FindByIdAsync(userId).Result.UserName);
        }

        public static MvcHtmlString ClaimType(this HtmlHelper html, string claimType)
        {
            FieldInfo[] fields = typeof(ClaimTypes).GetFields();

            foreach(FieldInfo field in fields)
            {
                if(field.GetValue(null).ToString() == claimType)
                {
                    return new MvcHtmlString(field.Name);
                }
            }

            return new MvcHtmlString(string.Format("{0}", claimType.Split('/', '.').Last()));
        }
    }
}
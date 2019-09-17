using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using IdentityTest.Entity;

namespace IdentityTest.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Files = new List<File>();
        }

        public virtual ICollection<File> Files { get; set; }
    }
}
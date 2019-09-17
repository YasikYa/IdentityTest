using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityTest.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace IdentityTest.Entity
{
    public class File
    {
        public int FileId { get; set; }

        public string UserId { get; set; }

        public string FileName { get; set; }
        
        public virtual AppUser User { get; set; }
    }
}
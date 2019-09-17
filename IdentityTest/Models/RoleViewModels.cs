using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IdentityTest.Models
{
    public class EditRoleModel
    {
        public AppRole Role { get; set; }
        public IEnumerable<AppUser> usersInRole { get; set; }
        public IEnumerable<AppUser> usersNotInRole { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
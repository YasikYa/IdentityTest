using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using IdentityTest.Models;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace IdentityTest.Infrastructure
{
    public class CustomUserValidator : UserValidator<AppUser>
    {
        private string userNamePattern = @"^[a-zA-Z0-9а-яА-Я]+$";

        public CustomUserValidator(AppUserManager manager) : base(manager)
        {

        }

        public override async Task<IdentityResult> ValidateAsync(AppUser item)
        {
            IdentityResult result = await base.ValidateAsync(item);

            if(!Regex.IsMatch(item.UserName, userNamePattern))
            {
                var errors = result.Errors.ToList();
                errors.Add("Только русские и английские буквы и цифры достуаны в имени пользователя.");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}
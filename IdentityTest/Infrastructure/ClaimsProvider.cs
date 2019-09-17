using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace IdentityTest.Infrastructure
{
    public class ClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ClaimsIdentity user)
        {
            List<Claim> claims = new List<Claim>();

            if(user.Name.ToLower() == "yaros")
            {
                claims.Add(CreateClaim(ClaimTypes.Gender, "male"));
            }
            return claims;
        }

        private static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String, "RemoteClaims");
        }
    }
}
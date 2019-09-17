using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using IdentityTest.Infrastructure;
using Microsoft.Owin.Security.Google;

namespace IdentityTest
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<AppIdentityDbContext>(AppIdentityDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Accaunt/Login")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                AuthenticationType = "Google",
                ClientId = "66593486357-kl3j10vdv7i1cg9ngrtfepab8iail25q.apps.googleusercontent.com",
                ClientSecret = "QYAY0BAdlmBYhtDJLR32-MDv",
                Caption = "Athorize with Google",
                CallbackPath = new PathString("/GoogleLoginCallback"),
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Passive,
                SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType(),
                BackchannelTimeout = TimeSpan.FromSeconds(60),
                BackchannelHttpHandler = new System.Net.Http.WebRequestHandler(),
                BackchannelCertificateValidator = null,
                Provider = new GoogleOAuth2AuthenticationProvider(),
                
            });
        }
    }
}
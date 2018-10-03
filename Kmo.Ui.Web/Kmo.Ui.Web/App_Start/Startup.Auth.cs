using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Kmo.Libs.Identity;

namespace Kmo
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, IdentityUser>(
                        validateInterval: TimeSpan.FromMinutes(Application.DefaultWebTimeout),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
                ,
                ExpireTimeSpan = TimeSpan.FromMinutes(Application.DefaultWebTimeout)
            });
        }
    }
}
using System;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;


namespace GroupShareKitSample
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromHours(11)

            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);



        }
    }
}
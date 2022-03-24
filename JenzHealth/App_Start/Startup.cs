using JenzHealth.Areas.Admin.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var _settings = new ApplicationSettingsService().GetApplicationSettings();
            int timeout = _settings.SessionTimeOut;
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout"),
                ExpireTimeSpan = TimeSpan.FromMinutes(timeout),
                ReturnUrlParameter = "/Account/Login"
            });
        }
    }
}
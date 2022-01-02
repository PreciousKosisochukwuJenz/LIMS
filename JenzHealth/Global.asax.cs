using JenzHealth.App_Start;
using JenzHealth.Areas.Admin.Components;
using JenzHealth.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace JenzHealth
{
    public class MvcApplication : System.Web.HttpApplication
    {

        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DatabaseEntities context = new DatabaseEntities();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;
            log4net.Config.XmlConfigurator.Configure();
            var exists = context.Database.Exists();
            if (!exists)
            {
                context.Database.CreateIfNotExists();
                Seed.DatabaseSeed(context);
                Nav.StorePermissions(Nav.ApplicationMenu);
            }
        }
        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            log.Error("Error trying to do something", ex);
        }
    }
}

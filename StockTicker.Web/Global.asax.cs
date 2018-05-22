using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StockTicker.Lib.Common.Exceptions;
using StockTicker.Lib.Common.Utils;

namespace StockTicker.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Logging.Setup(Server.MapPath("~/log4net.config"));
            Logging.Info("Application started", false);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Logging.Info("Begin request");
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Logging.Info("End request");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Logging.Info("Application end", false);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exp = Server.GetLastError();

            if (exp == null)
                return;

            Logging.Error(exp?.ToString());
        }
    }
}
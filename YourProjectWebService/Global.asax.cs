using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static YourProjectWebService.Models.Database;

namespace YourProjectWebService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Entry point to the application which sets up all out configurations
        /// and runs our create database method.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            CreateDataBase();//Creates the database if it doesn't exist.
        }
    }
}

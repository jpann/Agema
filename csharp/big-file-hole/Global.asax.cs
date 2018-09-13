using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace big_file_hole
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var path = Server.MapPath("~/UploadedFiles");
            Trace.WriteLine($"Uploaded Files: {path}");
            if (!Directory.Exists(Server.MapPath("~/UploadedFiles")))
                Directory.CreateDirectory(Server.MapPath("~/UploadedFiles"));
        }
    }
}

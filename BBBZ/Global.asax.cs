using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BBBZ
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Error += Application_Error;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();

            try 
            {
                System.IO.File.AppendAllText(
                    Server.MapPath("~\\server_log.txt"), ((error is HttpException) ?((HttpException)error).GetHttpCode() : -1) +
                    "\r\n" + error.Message + "\r\n-------------------------------\r\n"); 
            } catch { }
            try
            {
                if (Request.Url.AbsolutePath.Contains("/Error/") == false)
                    Session["PreviousUrl"] = Request.Url.AbsolutePath;
                Session["errorCode"] = (error is HttpException) ? ((HttpException)error).GetHttpCode() : 500;
                Session["errorMessage"] = (error == null) ? "error has occure" : error.Message;
            }
            catch { }
            try
            {
                Server.ClearError();
                Response.Redirect("~/Error");
            }
            catch { }
        }
    }
}

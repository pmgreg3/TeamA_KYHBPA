using KYHBPA_TeamA.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KYHBPA_TeamA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        protected void Application_EndRequest()
        {
            if (Context.Response.StatusCode == 404)
            {
                Response.Clear();

                var errorRoute = new RouteData();
                errorRoute.Values["controller"] = "Error";
                errorRoute.Values["action"] = "ErrorPage";

                IController c = new ErrorController();
                c.Execute(new RequestContext(new HttpContextWrapper(Context), errorRoute));
            }
            // TODO: IMPLEMENT 500 > ERROR PAGE
            //else if (Context.Response.StatusCode >= 500)
            //{
            //    Response.Clear();

            //    var errorRoute = new RouteData();
            //    errorRoute.Values["controller"] = "Error";
            //    errorRoute.Values["action"] = "FiveOhError";

            //    IController c = new ErrorController();
            //    c.Execute(new RequestContext(new HttpContextWrapper(Context), errorRoute));
            //}
        }
    }
}

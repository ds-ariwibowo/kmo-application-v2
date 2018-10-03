using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Kmo
{
    public class Global : HttpApplication
    {
        public object FilterConfig { get; private set; }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            Kmo.Application.GlobalUseApi = false;
            Kmo.Application.UpdateDefaultConnectionString(Kmo.Application.DefaultWebConnectionString);
            //
            AreaRegistration.RegisterAllAreas();
            Kmo.FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kmo.Modules._900_CEO
{
    public class ceoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "900-CEO";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "900CEO_Area",
               "Modules/900-CEO/{controller}/{action}/{par1}/{par2}/{par3}/{par4}/{par5}",
               new
               {
                   action = "Index",
                   par1 = UrlParameter.Optional,
                   par2 = UrlParameter.Optional,
                   par3 = UrlParameter.Optional,
                   par4 = UrlParameter.Optional,
                   par5 = UrlParameter.Optional
               }
            );
        }
    }
}
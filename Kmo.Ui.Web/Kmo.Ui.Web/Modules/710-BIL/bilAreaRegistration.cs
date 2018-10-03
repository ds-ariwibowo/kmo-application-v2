using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kmo.Modules._710_BIL
{
    public class bilAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "710-BIL";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "710BIL_Area",
               "Modules/710-BIL/{controller}/{action}/{par1}/{par2}/{par3}/{par4}/{par5}",
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kmo.Modules._810_BDC
{
    public class bdcAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "810-BDC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "810BDC_Area",
               "Modules/810-BDC/{controller}/{action}/{par1}/{par2}/{par3}/{par4}/{par5}",
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
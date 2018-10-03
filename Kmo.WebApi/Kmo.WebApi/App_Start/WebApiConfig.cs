using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Kmo.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //DisableJsonFormatter();

            config.Routes.MapHttpRoute(
                name: "SysApi",
                routeTemplate: "sysapi/{controller}/{action}/{par1}/{par2}/{par3}/{par4}/{par5}/{par6}",
                defaults: new
                {
                    par1 = RouteParameter.Optional,
                    par2 = RouteParameter.Optional,
                    par3 = RouteParameter.Optional,
                    par4 = RouteParameter.Optional,
                    par5 = RouteParameter.Optional,
                    par6 = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "BdcApi",
                routeTemplate: "bdcapi/{controller}/{action}/{par1}/{par2}/{par3}/{par4}/{par5}/{par6}",
                defaults: new
                {
                    par1 = RouteParameter.Optional,
                    par2 = RouteParameter.Optional,
                    par3 = RouteParameter.Optional,
                    par4 = RouteParameter.Optional,
                    par5 = RouteParameter.Optional,
                    par6 = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
               name: "PmmApi",
               routeTemplate: "pmmapi/{controller}/{action}/{par1}/{par2}/{par3}/{par4}/{par5}/{par6}",
               defaults: new
               {
                   par1 = RouteParameter.Optional,
                   par2 = RouteParameter.Optional,
                   par3 = RouteParameter.Optional,
                   par4 = RouteParameter.Optional,
                   par5 = RouteParameter.Optional,
                   par6 = RouteParameter.Optional
               }
           );

            config.Routes.MapHttpRoute(
               name: "BdmApi",
               routeTemplate: "bdmapi/{controller}/{action}/{par1}/{par2}/{par3}/{par4}/{par5}/{par6}",
               defaults: new
               {
                   par1 = RouteParameter.Optional,
                   par2 = RouteParameter.Optional,
                   par3 = RouteParameter.Optional,
                   par4 = RouteParameter.Optional,
                   par5 = RouteParameter.Optional,
                   par6 = RouteParameter.Optional
               }
           );

            config.Routes.MapHttpRoute(
               name: "CeoApi",
               routeTemplate: "ceoapi/{controller}/{action}/{par1}/{par2}/{par3}/{par4}/{par5}/{par6}",
               defaults: new
               {
                   par1 = RouteParameter.Optional,
                   par2 = RouteParameter.Optional,
                   par3 = RouteParameter.Optional,
                   par4 = RouteParameter.Optional,
                   par5 = RouteParameter.Optional,
                   par6 = RouteParameter.Optional
               }
           );
        }

        public static void DisableJsonFormatter()
        {
            var jsonMediaTypeFormatters = GlobalConfiguration.Configuration.Formatters
                .Where(x => x.SupportedMediaTypes
                .Any(y => y.MediaType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            // Remove formatters from global config.
            foreach (var formatter in jsonMediaTypeFormatters)
            {
                GlobalConfiguration.Configuration.Formatters.Remove(formatter);
            }
        }
    }
}

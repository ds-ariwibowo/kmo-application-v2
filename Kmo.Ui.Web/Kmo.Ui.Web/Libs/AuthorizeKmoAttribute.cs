using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Kmo.Modules._650_SYS.UserManagement;

namespace Kmo
{
    public class AuthorizeKmoAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var baseAuth = base.AuthorizeCore(httpContext);
            if (!baseAuth)
            {
                return false;
            }

            var uId = httpContext.User.Identity.GetUserId();
            var controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            var action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();

            var isExist = Services.IsExistUserWebModules(uId, controller + "/" + action);
            if (isExist)
            {
                httpContext.Session["MainController"] = controller + "/" + action;
                return base.AuthorizeCore(httpContext);
            }
            else
            {
                if (HttpContext.Current.Session["MainController"] != null)
                {
                    if ((controller + "/" + action).ToString().Contains(HttpContext.Current.Session["MainController"].ToString()))
                        return base.AuthorizeCore(httpContext);
                    else
                        return false;
                }
                else
                {
                    httpContext.Session["Menu"] = null;
                    httpContext.Session["MainController"] = null;
                    return false;
                }
            }

        }
    }
}
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using Kmo.Modules._650_SYS.UserManagement;
using Kmo.Modules._650_SYS.UserManagement.Models;

namespace Kmo.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        [AuthorizeKmo]
        // GET: User
        public ActionResult Index()
        {
            var data = Services.ListOfUser(true);
            return View("~/Views/User/Index.cshtml", data);          
        }

        [HttpGet]
        public JsonResult GetUser(string userId)
        {
            var data = Services.GetUser(userId);          
            return Json(data, JsonRequestBehavior.AllowGet);
        }
              
        [HttpPost]
        public JsonResult SubmitUser(ta_User data, string header)
        {
            if (header == "Add User")
                Services.AddUser(data);
            else
               Services.UpdateUser(data);

            return null;
        }
        
        [HttpPost]
        public JsonResult VoidUser(string userId)
        {
            Services.VoidUser(userId);
            return null;
        }
               
        [HttpPost]
        public JsonResult ResetToDefaultPassword(string userId)
        {
            Services.ResetToDefaultPassword(userId);
            return null;
        }

        // GET: ManageUserPartialView
        [HttpGet]
        public ActionResult ManageUserPartialView()
        {
            return PartialView("~/Views/User/Manage.cshtml");
        }

        [HttpGet]
        public JsonResult GetWebModules()
        {
            var data = Services.GetWebModules(true);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserWebModules(string userId)
        {
            var data = Services.GetUserWebModules(userId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        

    }
}
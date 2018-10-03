using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Web.Helpers;
using Kmo.Modules._710_BIL.InvoiceRas;

namespace Kmo.Modules._710_BIL.InvoiceRas
{
    [Authorize]
    public class InvoiceRasController : Controller
    {
        [AuthorizeKmo]
        // GET: InvoiceRas
        public ActionResult Index()
        {
            var data = Services.ListOfInvRas(Application.AppSettingsValue("HoldingCompany"));
            return View("~/Modules/710-BIL/InvoiceRas/Views/Index.cshtml", data);
        }

        // GET: ManageInvRasPartialView
        [HttpGet]
        public ActionResult ManageInvRasPartialView()
        {
            return PartialView("~/Modules/710-BIL/InvoiceRas/Views/Manage.cshtml");
        }


        [HttpPost]
        public JsonResult SaveInvRas(Models.ta_INV_RA data)
        {
            var inv = Services.GetInvRas(Application.AppSettingsValue("HoldingCompany"), data.InvRasId);
            if (inv == null)
                //Services.SubmitInvRas(data);
                Services.GenerateInvRas(Application.AppSettingsValue("HoldingCompany"), data.LooId, data.LooIssue );
            else
                Services.UpdateInvRas(data);
            return null;
        }

        [HttpGet]
        public JsonResult GetInvRas(string invRasId)
        {          
            var data = Services.GetInvRas(Application.AppSettingsValue("HoldingCompany"), invRasId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
              

        [HttpGet]
        public JsonResult ListOfLooForInvRas()
        {
            var data = Services.ListOfLooForInvRas(Application.AppSettingsValue("HoldingCompany"));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLooForInvRas(string looIssue, string looId)
        {
            var data = Services.GetLooForInvRas(Application.AppSettingsValue("HoldingCompany"), looIssue, looId);
            if (string.IsNullOrEmpty(data.InvRasId))
            {
                // new
                data.InvRasId = Services.NewInvRasID(Application.GlobalUseApi); ;
                data.InvRasMemoNo = 0;
                data.InvRasOther = string.Empty;
                data.CreatedDate = DateTime.Now.Date;
                data.InvRasDate = DateTime.Now.Date;
                data.InvRasDueDate = DateTime.Now.Date;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        
         [HttpGet]
        public JsonResult GetGlobalOption()
        {
            var data = _900_CEO.LeaseAgreement.Services.GetGlobalOption();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
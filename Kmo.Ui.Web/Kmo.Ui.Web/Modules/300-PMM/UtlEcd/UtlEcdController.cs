using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kmo.Modules._300_PMM.UtlEcd;
using Kmo.Modules._810_BDC.SuiteAvailabilityStatus;

namespace Kmo.Modules._300_PMM.UtlEcd
{
    [Authorize]
    public class UtlEcdController : Controller
    {
        [AuthorizeKmo]
        // GET: UtlEcd
        public ActionResult IndexApa()
        {
            var data = Services.ListOfUtlEcd(Application.AppSettingsValue("HoldingCompany"));
            return View("~/Modules/300-PMM/UtlEcd/Views/IndexApa.cshtml", data);
        }

        [AuthorizeKmo]
        public ActionResult IndexUtl()
        {
            var data = Services.ListOfUtlEcd(Application.AppSettingsValue("HoldingCompany"));
            return View("~/Modules/300-PMM/UtlEcd/Views/IndexUtl.cshtml", data);
        }

        [AuthorizeKmo]
        public ActionResult IndexUtlEcd()
        {
            ViewBag.Module = "UTLECD";
            var data = _810_BDC.SuiteAvailabilityStatus.Services.ListOfFloorWithoutConstraint(Application.AppSettingsValue("HoldingCompany"));
            return View("~/Modules/810-BDC/SuiteAvailabilityStatus/Views/Index.cshtml", data);
        }


        // GET: ManageUtlEcdPartialView
        [HttpGet]
        public ActionResult ManageUtlEcdPartialView()
        {
            return PartialView("~/Modules/300-PMM/UtlEcd/Views/Manage.cshtml");
        }

        [HttpPost]
        
        public JsonResult CalculateEcd(DateTime date)
        {
            Services.CalculateEcd(Application.AppSettingsValue("HoldingCompany"), date.Month, date.Year);
            Services.PostingRptUtlEcd(Application.AppSettingsValue("HoldingCompany"), date.Month, date.Year);
            return null;
        }

        [HttpPost]
        public JsonResult SubmitUtlEcd(Models.ta_UTLECD data)
        {
            var ecd = Services.GetUtlEcd(Application.AppSettingsValue("HoldingCompany"), data.Date.Year, data.Date.Month);
            if (ecd != null && ecd.PostInvoice)
            {
                var msg = "ECD on period " + data.Date.Month.ToString().PadLeft(2, '0') + "-" + data.Date.Year.ToString() + " already submitted into Invoice ! ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Services.DeleteUtlEcd(data);
                Services.NewUtlEcd(data);
                return null;
            }
        }

        

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kmo.Modules._710_BIL.InvoicePln;

namespace Kmo.Modules._710_BIL.InvoicePln
{
    [Authorize]
    public class InvoicePlnController : Controller
    {
        [AuthorizeKmo]
        // GET: InvoicePln
        public ActionResult Index()
        {
            var data = Services.ListOfInvPln(false);
            return View("~/Modules/710-BIL/InvoicePln/Views/Index.cshtml", data);
        }

        // GET: ManageInvPlnPartialView
        [HttpGet]
        public ActionResult ManageInvPlnPartialView()
        {
            return PartialView("~/Modules/710-BIL/InvoicePln/Views/Manage.cshtml");
        }
            
        [HttpGet]
        public JsonResult GetAvailablePlnBill()
        {
            var data = Services.ListOfPlnBill();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPlnBill(int year, int month, string floorId, int suitId, string clientId, string invPlnRev, string invPlnId)
        {
            var data = Services.GetPlnBill(year, month, floorId, suitId, clientId, invPlnRev, invPlnId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitInvPln(Models.ta_INV_PLN data)
        {         
            Services.SubmitInvPln(data);
            if (data.InvPlnRev != "00")
                Services.VoidInvpln(Convert.ToString(Convert.ToInt32(data.InvPlnRev) - 1).PadLeft(2, '0'), data.InvPlnId);
            else
                _300_PMM.UtlEcd.Services.SetUtlEcdInvoiced(data.HoldingCompanyId, data.FloorId, data.SuitId, data.Month, data.Year);

            return null;
        }

        

    }
}
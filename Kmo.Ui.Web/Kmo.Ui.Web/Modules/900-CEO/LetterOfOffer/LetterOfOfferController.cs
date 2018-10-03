using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kmo.Modules._800_BDM.ClientFactSheet;

namespace Kmo.Modules._900_CEO.LetterOfOffer
{
    [Authorize]
    public class LetterOfOfferController : Controller
    {
        [AuthorizeKmo]
        // GET: LetterOfOffer
        public ActionResult Index()
        {

            var data = Services.ListOflooWithoutConstraint(Application.AppSettingsValue("HoldingCompany"), "!Void");
            return View("~/Modules/900-CEO/LetterOfOffer/Views/Index.cshtml", data);
        }

        [HttpPost]
        public JsonResult SubmitLoo(string looIssue, string looId)
        {
            Services.UpdateLooStatus(Application.AppSettingsValue("HoldingCompany"), looIssue, looId, "Submitted");
            return null;
        }

        [HttpPost]
        public JsonResult VoidLoo(string looIssue, string looId)
        {
            Services.UpdateLooStatus(Application.AppSettingsValue("HoldingCompany"), looIssue, looId, "Void");
            return null;
        }            
        

        [HttpPost]
        public JsonResult AddNewLoo(Models.ta_LOO data, string oldIssue)
        {
            Services.NewLOO(false, data);
            if (oldIssue.Length == 2)
                Services.UpdateLooStatus(Application.AppSettingsValue("HoldingCompany"), oldIssue, data.LooId, "Void");
            return null;
        }


        // GET: ManageLooPartialView
        [HttpGet]
        public ActionResult ManageLooPartialView()
        {
            return PartialView("~/Modules/900-CEO/LetterOfOffer/Views/Manage.cshtml");
        }


        [HttpGet]
        public Task<JsonResult> GetLoo(string looIssue, string looId)
        {
            return Task.Factory.StartNew(() =>
            {
                var returnVal = new Libs.KmoAjaxResult()
                {
                    success = false
                };

                try
                {
                    Models.vi_LOO loo = new Models.vi_LOO();
                    var data = Services.Getloo(Application.AppSettingsValue("HoldingCompany"), looIssue, looId);
                    if (data != null)
                    {
                        // revise               
                        data.LooIssue = (Convert.ToInt32(data.LooIssue) + 1).ToString().PadLeft(2, '0');
                        loo = data;
                    }
                    else
                    {
                        // new
                        loo.LooId = Services.NewLOOID(Application.AppSettingsValue("HoldingCompany"), Application.GlobalUseApi);
                        loo.LooIssue = "00";
                        loo.Cid = string.Empty;
                        loo.LooSubjectDate = DateTime.Now.Date;
                        loo.FittingOutDate = DateTime.Now.Date;
                        loo.AgreementCommencementDate = DateTime.Now.Date;
                        loo.RentCommencementDate = DateTime.Now.Date;
                        loo.ServChargeCommencementDate = DateTime.Now.Date;
                        loo.LeaseExpiryDate = DateTime.Now.Date;
                        loo.FloorCondition = "-";
                        loo.RentPayableAdvanced = 3;
                        loo.SecurityDepositDuration = 3;
                        loo.ServiceChargeDepositDuration = 3;
                        loo.RentalRate = 0;
                        loo.ServiceCharge = 0;
                        loo.SecurityUnitPrice = 0;
                        loo.ServiceChargeUnitPrice = 0;
                        loo.ParkCarReservedPerUnitPrice = 7000000;
                        loo.ParkCarUnreservedUnitPrice = 4000000;
                        loo.ParkMotorbikeUnitPrice = 150000;
                        loo.TelephoneDepositUnitPrice = 3000000;
                        loo.ParkCarReservedDuration = 1;
                        loo.ParkCarUnreservedDuration = 1;
                        loo.ParkMotorbikeDuration = 1;
                        loo.TelephoneDepositDuration = 1;
                        loo.ParkCarReservedQty = 1;
                        loo.ParkCarUnreservedQty = 1;
                        loo.ParkMotorbikeQty = 1;
                        loo.TelephoneDepositQty = 1;                        
                    }
                  
                    //throw new Exception("Raise Error if any");
                    returnVal.data = loo;
                    returnVal.success = true;
                }
                catch (Exception x)
                {
                    returnVal.message = x.Message;
                }

                return Json(returnVal, JsonRequestBehavior.AllowGet);
            });
        }


        [HttpGet]
        public JsonResult GetLooSpecialConditions(string looIssue, string looId)
        {
            var data = Services.ListOfSpecialConditions(Application.AppSettingsValue("HoldingCompany"), looIssue, looId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLooAmend(string looIssue, string looId)
        {
            var data = Services.ListOfAmandements(Application.AppSettingsValue("HoldingCompany"), looIssue, looId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLooSuites(string looIssue, string looId)
        {
            var data = Services.ListOfLOOSuites(Application.AppSettingsValue("HoldingCompany"), looIssue, looId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLooPayments(string looIssue, string looId)
        {
            var data = Services.ListOfPayments(Application.AppSettingsValue("HoldingCompany"), looIssue, looId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: LooLogPartialView
        [HttpGet]
        public ActionResult LooLogPartialView(string looId)
        {
            var data = Services.ListOfLoglooWithoutConstraint(Application.AppSettingsValue("HoldingCompany"), looId);
            return PartialView("~/Modules/900-CEO/LetterOfOffer/Views/Log.cshtml", data);
        }
        


    }
}
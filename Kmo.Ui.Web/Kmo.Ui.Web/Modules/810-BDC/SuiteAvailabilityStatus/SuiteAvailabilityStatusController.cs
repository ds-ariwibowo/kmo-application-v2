using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kmo.Modules._810_BDC.SuiteAvailabilityStatus;

namespace Kmo.Modules._810_BDC.SuiteAvailabilityStatus
{
    [Authorize]
    public class SuiteAvailabilityStatusController : Controller
    {
        [AuthorizeKmo]
        // GET: SuiteAvailabilityStatus
        public ActionResult Index()
        {
            ViewBag.Module = "SAS";
            var data = Services.ListOfFloorWithoutConstraint(Application.AppSettingsValue("HoldingCompany"));
            return View("~/Modules/810-BDC/SuiteAvailabilityStatus/Views/Index.cshtml", data);
        }

        public ActionResult ViewFloorECD(string floorId)
        {
            ViewBag.FloorId = floorId;
            var data = Services.ListOfFloorECD(Application.AppSettingsValue("HoldingCompany"), floorId);
            return View("~/Modules/810-BDC/SuiteAvailabilityStatus/Views/ViewFloorECD.cshtml", data);
        }

        

        public ActionResult ViewSuites(string floorId, string module)
        {
            ViewBag.Module = module;
            ViewBag.FloorId = floorId;           
            var floor = Services.GetFloor(Application.AppSettingsValue("HoldingCompany"), floorId);
            ViewBag.TotalArea = floor.TotalArea;

            var data = Services.ListOfSuiteWithoutConstraint(Application.AppSettingsValue("HoldingCompany"), floorId, true);
            return View("~/Modules/810-BDC/SuiteAvailabilityStatus/Views/ViewSuites.cshtml", data);
        }

        public ActionResult ViewSuiteECD(string floorId, string suiteName, int suiteId)
        {
            ViewBag.FloorId = floorId;
            ViewBag.SuiteId = suiteId;
            ViewBag.SuiteName = suiteName;
            var data = Services.ListOfSuiteECD(Application.AppSettingsValue("HoldingCompany"), floorId, suiteId);
            return View("~/Modules/810-BDC/SuiteAvailabilityStatus/Views/ViewSuiteECD.cshtml", data);
        }



        [HttpPost]
        public JsonResult EditFloor(Models.ta_FLOOR data)
        {
            Services.EditFloor(false, data);
            return null;
        }

        [HttpPost]
        public JsonResult CancelSuite(string floorId, int suiteId)
        {
            Services.CancelSuite(Application.AppSettingsValue("HoldingCompany"), floorId, suiteId);
            return null;
        }


        [HttpPost]
        public JsonResult AddNewSuite(Models.ta_SUIT data, int oldSuiteId)
        {
            // default Meter Factor = 1
            data.MeterFactor = 1;

            Services.NewSuite(false, data);
            Services.ReorderSuite(Application.AppSettingsValue("HoldingCompany"), data.FloorId);

            if (oldSuiteId > 0)
                Services.CancelSuite(Application.AppSettingsValue("HoldingCompany"), data.FloorId, oldSuiteId);
            return null;
        }

        [HttpPost]
        public JsonResult SubmitFloorECD(Models.ta_FLOORS_ECD_AcOutdoor data)
        {
            var ecd = Services.GetFloorECD(Application.AppSettingsValue("HoldingCompany"), data.FloorId, data.Date.Year, data.Date.Month);
            if (ecd != null && ecd.PostInvoice)
            {
                var msg = "ECD on period " + data.Date.Month.ToString().PadLeft(2, '0') + "-" + data.Date.Year.ToString() + " already submitted into Invoice ! ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Services.DeleteECDFloor(data);

                //Models.ta_FLOORS_ECD_AcOutdoor1 ecd = new Models.ta_FLOORS_ECD_AcOutdoor1();
                //ecd = Services.GetFloorECD(Application.AppSettingsValue("HoldingCompany"), data.FloorId, data.Date.Year, data.Date.Month, data.Date.Day);

                Models.ta_FLOORS_ECD_AcOutdoor1 ecdlast = new Models.ta_FLOORS_ECD_AcOutdoor1();
                ecdlast = Services.GetLastFloorECD(Application.AppSettingsValue("HoldingCompany"), data.FloorId, data.Date.Year, data.Date.Month);

                if (ecdlast != null)
                    data.Usages = data.Reading - ecdlast.Reading;
                if (data.Usages < 0)
                    data.Usages = 0;

                //if (ecd == null)
                Services.NewFloorECD(false, data);
                //else
                //    Services.EditFloorECD(false, data);

                return null;
            }
        }

        [HttpPost]
        public JsonResult SubmitSuiteECD(Models.ta_SUITS_ECD data)
        {
            var ecd = Services.GetSuiteECD(Application.AppSettingsValue("HoldingCompany"), data.FloorId, data.SuitId, data.Date.Year, data.Date.Month);
            if (ecd != null && ecd.PostInvoice)
            {
                var msg = "ECD on period " + data.Date.Month.ToString().PadLeft(2, '0') + "-" + data.Date.Year.ToString() + " already submitted into Invoice ! ";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Services.DeleteECDSuite(data);

                //Models.ta_SUITS_ECD1 ecd = new Models.ta_SUITS_ECD1();
                //ecd = Services.GetSuiteECD(Application.AppSettingsValue("HoldingCompany"), data.FloorId, data.Date.Year, data.Date.Month, data.Date.Day);

                Models.ta_SUITS_ECD1 ecdlast = new Models.ta_SUITS_ECD1();
                ecdlast = Services.GetLastSuiteECD(Application.AppSettingsValue("HoldingCompany"), data.FloorId, data.SuitId, data.Date.Year, data.Date.Month);

                if (ecdlast != null)
                {
                    data.AcUsages = data.AcReading - ecdlast.AcReading;
                    if (data.AcUsages < 0)
                        data.AcUsages = 0;
                    data.NonAcUsages = data.NonAcReading - ecdlast.NonAcReading;
                    if (data.NonAcUsages < 0)
                        data.NonAcUsages = 0;
                }

                //if (ecd == null)
                Services.NewSuiteECD(false, data);
                //else
                //    Services.EditSuiteECD(false, data);

                return null;
            }
        }

        // GET: ManageFloorPartialView
        [HttpGet]
        public ActionResult ManageFloorPartialView()
        {
            return PartialView("~/Modules/810-BDC/SuiteAvailabilityStatus/Views/ManageFloor.cshtml");
        }
        

        // GET: ManageSasPartialView
        [HttpGet]
        public ActionResult ManageSasPartialView()
        {
            return PartialView("~/Modules/810-BDC/SuiteAvailabilityStatus/Views/ManageSuites.cshtml");
        }

        // GET: ManageFloorEdcPartialView
        [HttpGet]
        public ActionResult ManageFloorEcdPartialView()
        {
            return PartialView("~/Modules/810-BDC/SuiteAvailabilityStatus/Views/ManageFloorECD.cshtml");
        }

        // GET: ManageSuiteEdcPartialView
        [HttpGet]
        public ActionResult ManageSuiteEcdPartialView()
        {
            return PartialView("~/Modules/810-BDC/SuiteAvailabilityStatus/Views/ManageSuiteECD.cshtml");
        }


        [HttpGet]
        public Task<JsonResult> GetSuite(string floorId, int suiteId)
        {
            return Task.Factory.StartNew(() =>
            {
                var returnVal = new Libs.KmoAjaxResult()
                {
                    success = false
                };

                try
                {
                    Models.vi_SAS suite = new Models.vi_SAS();
                    suite = Services.GetSuite(Application.AppSettingsValue("HoldingCompany"), floorId, suiteId);
                    if (suite == null)
                    {
                        // new
                        suite = new Models.vi_SAS();
                        suite.SuiteOrder = 0;
                        suite.SuiteName = string.Empty;
                        suite.SuiteArea = 0;
                        suite.ElectricityCapacity = 0;
                        suite.Zone = string.Empty;
                    }
                    suite.SuitId = Services.NewSuiteId(Application.AppSettingsValue("HoldingCompany"), Application.GlobalUseApi, floorId);

                    //throw new Exception("Raise Error if any");
                    returnVal.data = suite;
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
        public JsonResult CheckSuiteByName(string suiteName, int suiteId)
        {
            Models.vi_SAS suite = new Models.vi_SAS();
            suite = Services.CheckSuiteByName(Application.AppSettingsValue("HoldingCompany"), suiteName, suiteId, true);
            if (suite == null)
            {
                suite = new Models.vi_SAS();
                suite.SuiteName = string.Empty;
            }
            return Json(suite, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetFloor(string floorId)
        {
            var data = Services.GetFloor(Application.AppSettingsValue("HoldingCompany"),floorId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFloorList()
        {
            var data = Services.ListOfFloorWithoutConstraint(Application.AppSettingsValue("HoldingCompany"));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSuiteList(string floorId)
        {
            var data = Services.ListOfSuiteWithoutConstraint(Application.AppSettingsValue("HoldingCompany"), floorId, true);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllSuite()
        {
            var data = Services.ListOfAllSuiteWithoutConstraint(Application.AppSettingsValue("HoldingCompany"), true);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        


        [HttpGet]
        public JsonResult GetFloorECDList(string floorId)
        {
            var data = Services.ListOfFloorECD(Application.AppSettingsValue("HoldingCompany"), floorId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSuiteECDList(string floorId, int suiteId)
        {
            var data = Services.ListOfSuiteECD(Application.AppSettingsValue("HoldingCompany"), floorId, suiteId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
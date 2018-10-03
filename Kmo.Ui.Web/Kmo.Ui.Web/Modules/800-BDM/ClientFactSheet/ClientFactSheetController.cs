using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kmo.Modules._800_BDM.ClientFactSheet
{
    [Authorize]
    public class ClientFactSheetController : Controller
    {
        [AuthorizeKmo]
        // GET: ClientFactSheet
        public ActionResult Index()
        {
            //Cara Isi binary dan ambil binary
            //var binData = new Modules._650_SYS.DataStorage.Models.ta_BinaryData();
            //var binDataID = Application.BinToStorage(Application.GlobalUseApi, binData).Result;
            //var binDataFromStrore = Application.BinFromStorage(false, binDataID).Result;

            //Panggil Option Dari Database
            //var tdl = Application.GlobalOption<decimal>("PNG_TDL");

            //Panggil Services
            //var data = Services.GetAllClients(false, Application.DefaultWebHoldingCompanyId);
            //var data = Services.ListOfClients().Where(r => r.HoldingCompanyId == Application.AppSettingsValue("HoldingCompany"));

            var data = Services.ListOfClientsWithoutConstraint(Application.AppSettingsValue("HoldingCompany"), "!Void");
            return View("~/Modules/800-BDM/ClientFactSheet/Views/Index.cshtml", data);
        }

        [HttpPost]
        public JsonResult VoidCfs(string clientIssue, string clientId)
        {
            Services.VoidClients(Application.AppSettingsValue("HoldingCompany"), clientIssue, clientId);
            return null;
        }


        [HttpPost]
        public JsonResult AddNewCfs(Models.ta_CLIENT data, string oldIssue)
        {          
            Services.NewClient(false, data);
            if (oldIssue.Length == 2)
                Services.VoidClients(Application.AppSettingsValue("HoldingCompany"), oldIssue, data.ClientId);
            return null;
        }

       
        // GET: ManageCfsPartialView
        [HttpGet]
        public ActionResult ManageCfsPartialView()
        {
            return PartialView("~/Modules/800-BDM/ClientFactSheet/Views/Manage.cshtml");
        }


        [HttpGet]
        public Task<JsonResult> GetCfs(string clientIssue, string clientId)
        {
            return Task.Factory.StartNew(() =>
            {
                var returnVal = new Libs.KmoAjaxResult()
                {
                    success = false
                };

                try
                {
                    Models.ta_CLIENT1 client = new Models.ta_CLIENT1();
                    var data = Services.GetClient(Application.AppSettingsValue("HoldingCompany"), clientIssue, clientId);
                    if (data != null)
                    {
                        // revise               
                        data.ClientIssue= (Convert.ToInt32(data.ClientIssue) + 1).ToString().PadLeft(2, '0');
                        client = data;
                    }
                    else
                    {
                        // new
                        client.ClientId = Services.NewClientID(Application.AppSettingsValue("HoldingCompany"), Application.GlobalUseApi);
                        client.ClientIssue = "00";
                        var lob = "XX";
                        var propName = "10";
                        var looYear = client.ClientId.Substring(6, 2);
                        var ctrYear = "00";
                        var seq = Services.NewCidSequence(Application.GlobalUseApi, looYear);
                        client.Cid = string.Format("{0}-{1}-{2}-{3}-{4}", lob, propName, seq, looYear, ctrYear);
                        client.CreatedDate = DateTime.Now.Date;
                        client.RentStartingDate = DateTime.Now.Date;
                        client.CompanyName = string.Empty;
                        client.CompanyAddress = string.Empty;
                        client.CompanyEmail = string.Empty;                      
                        client.City = string.Empty;
                        client.ZipCode = string.Empty;
                        client.Telephone = string.Empty;
                        client.RequiredSpaceArea = 0;
                        client.ProposedSuite = string.Empty;
                        client.ProposedArea = 0;
                        client.RentPeriod = 0;
                        client.IspFirstMedia = false;
                        client.IspExcelcomindo = false;
                        client.IspOthers = false;
                        client.IspTelkom = false;
                        client.FreeTax = false;
                    }
                   
                    if (string.IsNullOrEmpty(client.Fax))
                        client.Fax = string.Empty;
                    if (string.IsNullOrEmpty(client.Npwp))
                        client.Npwp = string.Empty;
                    if (string.IsNullOrEmpty(client.LineOfBusinessOthersValue))
                        client.LineOfBusinessOthersValue = string.Empty;
                    if (string.IsNullOrEmpty(client.IspOthersValue))
                        client.IspOthersValue = string.Empty;
                    if (string.IsNullOrEmpty(client.RefCompanyValue))
                        client.RefCompanyValue = string.Empty;
                    if (string.IsNullOrEmpty(client.RefIndividualValue))
                        client.RefIndividualValue = string.Empty;
                    if (string.IsNullOrEmpty(client.RefKmoRentalSignageValue))
                        client.RefKmoRentalSignageValue = string.Empty;
                    if (string.IsNullOrEmpty(client.RefKmoWebsiteValue))
                        client.RefKmoWebsiteValue = string.Empty;
                    if (string.IsNullOrEmpty(client.RefNewspaperValue))
                        client.RefNewspaperValue = string.Empty;
                    if (string.IsNullOrEmpty(client.RefOthersValue))
                        client.RefOthersValue = string.Empty;

                    //throw new Exception("Raise Error if any");
                    returnVal.data = client;
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
        public Task<JsonResult> GetCfsPic(string clientIssue, string clientId)
        {
            return Task.Factory.StartNew(() =>
            {
                var returnVal = new Libs.KmoAjaxResult()
                {
                    success = false
                };

                try
                {
                    var data = Services.ListOfPics(Application.AppSettingsValue("HoldingCompany"), clientIssue, clientId);
                    //throw new Exception("Raise Error if any");
                    returnVal.data = data;
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
        public JsonResult GetCfsSpecialConditions(string clientIssue, string clientId)
        {          
            var specialConditions = Services.ListOfSpecialConditions(Application.AppSettingsValue("HoldingCompany"), clientIssue, clientId);
            return Json(specialConditions, JsonRequestBehavior.AllowGet);
        }

        // GET: CfsLogPartialView
        [HttpGet]
        public ActionResult CfsLogPartialView(string clientId)
        {
            var data = Services.ListOfLogClientsWithoutConstraint(Application.AppSettingsValue("HoldingCompany"), clientId);
            return PartialView("~/Modules/800-BDM/ClientFactSheet/Views/Log.cshtml", data);
        }
        

        [HttpGet]
        public JsonResult GetCfsList()
        {
            var data = Services.ListOfClientsWithoutConstraint(Application.AppSettingsValue("HoldingCompany"), "WIP");
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCfsLastRevision(string clientId)
        {
            var data = Services.GetClientLastRevision(Application.AppSettingsValue("HoldingCompany"), clientId, "WIP");
            return Json(data, JsonRequestBehavior.AllowGet);
        }


    }
}
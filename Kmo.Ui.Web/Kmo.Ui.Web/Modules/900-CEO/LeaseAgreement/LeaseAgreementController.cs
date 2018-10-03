using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Web.Helpers;
using Kmo.Modules._800_BDM.ClientFactSheet;
using Kmo.Modules._900_CEO.LetterOfOffer;

namespace Kmo.Modules._900_CEO.LeaseAgreement
{
    [Authorize]
    public class LeaseAgreementController : Controller
    {
        [AuthorizeKmo]
        // GET: LetterOfOffer
        public ActionResult Index()
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToAction("Login", "Account");
            //else
            //{
                var data = Services.ListOfCtrWithoutConstraint("!Void");
                return View("~/Modules/900-CEO/LeaseAgreement/Views/Index.cshtml", data);
            //}
        }

        [AuthorizeKmo]
        // GET: PreINVSEC
        public ActionResult IndexPreInv()
        {          
            var data = Services.ListOfPreInvSecWithoutConstraint("!Void");
            return View("~/Modules/900-CEO/LeaseAgreement/Views/IndexPreInv.cshtml", data);
        }


        [HttpPost]
        public JsonResult SubmitCtr(string ctrId)
        {
            Services.UpdateCtrStatus(ctrId, "Submitted");
            return null;
        }

        //[HttpPost]
        //public JsonResult VoidLoo(string ctrId)
        //{
        //    Services.VoidCtr(ctrId);
        //    return null;
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {         

            string _imgname = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["Images"];

                if (pic.ContentLength > 0)
                {
                    var content = new byte[pic.ContentLength];
                    pic.InputStream.Read(content, 0, pic.ContentLength);
                    
                    Models.ta_CTR_Attachment attach = new Models.ta_CTR_Attachment();
                    attach.CtrId = System.Web.HttpContext.Current.Request.Form["Id"];
                    attach.LooId = System.Web.HttpContext.Current.Request.Form["Id"];
                    attach.LooIssue = System.Web.HttpContext.Current.Request.Form["Issue"];
                    attach.Article = System.Web.HttpContext.Current.Request.Form["FileName"];
                    attach.Description = pic.FileName; // include file path
                    attach.BinaryData = content;
                    if (System.Web.HttpContext.Current.Request.Form["Header"] == "Add CTR")
                        Services.NewAttach(attach);
                    else
                        Services.EditAttach(attach);

                }
            }          
            return null;
        }


        [HttpPost]      
        public JsonResult SaveCtr(Models.ta_CTR data, string header)
        {                    
            if (header == "Add CTR")
            {
                Services.NewCTR(false, data);
                LetterOfOffer.Services.UpdateLooStatus(Application.AppSettingsValue("HoldingCompany"), data.LooIssue, data.LooId, "Final");
                var Loo = LetterOfOffer.Services.Getloo(Application.AppSettingsValue("HoldingCompany"), data.LooIssue, data.LooId);
                if (Loo != null)
                   _800_BDM.ClientFactSheet.Services.UpdateCIDDateAgreement(Application.AppSettingsValue("HoldingCompany"), Loo.ClientIssue, Loo.ClientId, data.CtrDate.Value.Month);
            }            
            else if (header == "INV-SEC")
            {
                Services.UpdateCtr(data);
            }
            else
                Services.EditCTR(false, data);
            
            return null;
        }


        // GET: ManageCtrPartialView
        [HttpGet]
        public ActionResult ManageCtrPartialView()
        {
            return PartialView("~/Modules/900-CEO/LeaseAgreement/Views/Manage.cshtml");
        }


        // GET: ManagePreInvSecPartialView
        [HttpGet]
        public ActionResult ManagePreInvSecPartialView()
        {
            return PartialView("~/Modules/900-CEO/LeaseAgreement/Views/ManagePreInv.cshtml");
        }

        [HttpGet]
        public Task<JsonResult> GetLoo(string looId)
        {
            return Task.Factory.StartNew(() =>
            {
                var returnVal = new Libs.KmoAjaxResult()
                {
                    success = false
                };

                try
                {
                    LetterOfOffer.Models.vi_LOO loo = new LetterOfOffer.Models.vi_LOO();
                    var data = LetterOfOffer.Services.Getloo(Application.AppSettingsValue("HoldingCompany"), looId.Substring(0,2), looId.Substring(2,8));
                    if (data != null)
                    {                       
                        loo = data;
                    }
                    else
                    {
                        // new
                        loo.CtrId = "XXXXXXXX";
                        loo.LooId = string.Empty; 
                        loo.LooIssue = string.Empty;
                        loo.Cid = string.Empty;
                        loo.CreatedDate = DateTime.Now.Date;
                        loo.CtrDate = DateTime.Now.Date;
                        loo.Status = "WIP";                        
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
        public JsonResult GetCtr(string ctrId)
        {          
            var data = Services.GetCtr(ctrId);
            if (string.IsNullOrEmpty(data.PreInvId))
            {
                // new
                data.PreInvId = Services.NewPreInvID(Application.GlobalUseApi); ;
                data.PreInvMemoNo = 1;
                data.PreInvOther = string.Empty;
                data.PreInvDate = DateTime.Now.Date;
                data.PreInvDueDate = DateTime.Now.Date;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCtrSpecialConditions(string ctrId)
        {
            var data = Services.ListOfSpecialConditions(ctrId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCtrAmend(string ctrId)
        {
            var data = Services.ListOfAmandements(ctrId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCtrAttach(string ctrId)
        {
            var data = Services.ListOfAttachment(ctrId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        


        [HttpGet]
        public JsonResult GetAvailableLoo()
        {
            var data = LetterOfOffer.Services.ListOflooWithoutConstraint(Application.AppSettingsValue("HoldingCompany"), "Submitted");
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAvailableCtr()
        {
            var data = Services.ListOfCtrWithoutConstraint("Submitted");
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGlobalOption()
        {
            var data = Services.GetGlobalOption();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
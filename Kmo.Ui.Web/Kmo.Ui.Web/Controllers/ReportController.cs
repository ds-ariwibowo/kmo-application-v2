using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kmo.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id) || !TempData.ContainsKey("rptReq_" + id))
            {
                ViewBag.SessionID = "";
                ViewBag.InvalidReport = true;
                ViewBag.InvalidMessage = "Invalid Report Session";

            }
            else
            {
                ViewBag.InvalidReport = false;
                ViewBag.SessionID = id;

                var rptReq = TempData["rptReq_" + id] as Libs.ReportRequest;

                TempData.Remove("rptReq_" + id);
                TempData.Add("rptReq_" + id, rptReq);

                ViewBag.Title = rptReq.Title;

            }
            return View();
        }

        [HttpPost]
        public ActionResult PrepareReport(Libs.ReportRequest rptReq)
        {
            Libs.KmoAjaxResult result = new Libs.KmoAjaxResult()
            {
                success = false
            };

            try
            {
                rptReq.SessionID = Guid.NewGuid().ToString().Replace("-", "");
                TempData.Add("rptReq_" + rptReq.SessionID, rptReq);
                result.data = Url.RouteUrl("Default", new { Controller = "Report", Action = "Index", id = rptReq.SessionID });
                result.success = true;
            }
            catch (Exception x)
            {
                result.message = x.Message;
            }
            return Json(result);
        }

        [HttpGet]
        public JsonResult ReadReport(string sessionID)
        {
            var result = new Libs.KmoAjaxResult() { success = false };

            var rptReq = TempData["rptReq_" + sessionID] as Libs.ReportRequest;

            TempData.Remove("rptReq_" + sessionID);
            TempData.Add("rptReq_" + sessionID, rptReq);

            try
            {
                if (!TempData.ContainsKey("pdfbin_" + rptReq.SessionID))
                {
                    Libs.ReportParameterCollection rptPars;
                    if (!string.IsNullOrEmpty(rptReq.RecordSelectionFormula))
                    {
                        rptPars = Application.Report_Crpt_BuildParameters(rptReq.ReportName, rptReq.RecordSelectionFormula);
                    }
                    else
                    {
                        rptPars = Application.Report_Crpt_BuildParameters(rptReq.ReportName);
                    }

                    rptPars.AddRange(rptReq.Parameters.ToArray());

                    var pdfBin = Application.Report_Crpt_BuildRptPdf(rptPars).Result;

                    var base64string = GhostScript.RasterizePdf(pdfBin).ToBase64Strings();

                    var rptView = new Libs.ReportViewModel();
                    rptView.CurrentPage = 1;
                    rptView.TotalPage = base64string.Count();
                    rptView.PageData = base64string[0];
                    rptView.SessionID = sessionID;
                    result.data = rptView;

                    if (TempData.ContainsKey("pageNumber_" + rptReq.SessionID))
                    {
                        TempData.Remove("pageNumber_" + rptReq.SessionID);
                    }
                    TempData.Add("pdfbin_" + rptReq.SessionID, pdfBin);
                    TempData.Add("pageNumber_" + rptReq.SessionID, (Int32)1);
                    result.success = true;
                }
                else
                {
                    var pdfBin = (byte[])TempData["pdfbin_" + rptReq.SessionID];
                    var base64string = GhostScript.RasterizePdf(pdfBin).ToBase64Strings();

                    var page = base64string[0];
                    var rptView = new Libs.ReportViewModel();
                    rptView.CurrentPage = 1;
                    rptView.TotalPage = base64string.Count();
                    rptView.PageData = base64string[0];
                    rptView.SessionID = sessionID;
                    result.data = rptView;

                    if (TempData.ContainsKey("pageNumber_" + rptReq.SessionID))
                    {
                        TempData.Remove("pageNumber_" + rptReq.SessionID);
                    }
                    TempData.Add("pageNumber_" + rptReq.SessionID, (Int32)1);
                    TempData.Remove("pdfbin_" + rptReq.SessionID);
                    TempData.Add("pdfbin_" + rptReq.SessionID, pdfBin);
                    result.success = true;
                }
            }
            catch (Exception x)
            {
                result.message = x.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string NextPage(string sessionID)
        {
            var bytes = (byte[])TempData["pdfBin_" + sessionID];
            int currentPage = (int)TempData["pageNumber_" + sessionID];

            TempData.Remove("pdfBin_" + sessionID);
            TempData.Remove("pageNumber_" + sessionID);
            TempData.Add("pdfBin_" + sessionID, bytes);

            if (bytes == null)
            {
                return "";
            }

            var base64string = GhostScript.RasterizePdf(bytes).ToBase64Strings();
            int totalPage = base64string.Count();

            if (currentPage < totalPage)
            {
                TempData.Add("pageNumber_" + sessionID, currentPage + 1);
                return base64string[currentPage];
            }
            else
            {
                TempData.Add("pageNumber_" + sessionID, currentPage);
                return base64string[currentPage - 1];
            }
        }

        [HttpGet]
        public FileResult savePdf(string sessionID)
        {
            if (TempData.ContainsKey("rptReq_" + sessionID))
            {
                var rptReq = TempData["rptReq_" + sessionID] as Libs.ReportRequest;

                TempData.Remove("rptReq_" + sessionID);
                TempData.Add("rptReq_" + sessionID, rptReq);

                var bytes = (byte[])TempData["pdfBin_" + sessionID];

                TempData.Remove("pdfBin_" + sessionID);
                TempData.Add("pdfBin_" + sessionID, bytes);

                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, rptReq.Title + ".pdf");
            }

            return null;
        }
    }
}
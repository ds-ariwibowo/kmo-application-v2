using Kmo.Modules._650_SYS.DocumentAndReport;
using Kmo.Modules._650_SYS.DocumentAndReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kmo.WebApi.Controllers._650_SYS
{
    [RoutePrefix("sysapi/documentandreport")]
    public class DocumentAndReportController : ApiController
    {
        [HttpPost]
        public Task<bool> PostNewDocument(ta_MasterOfDocument doc)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return false;
                }
                try
                {
                    Services.DbOperation(doc, Enums.EntityMode.Insert);
                    return true;
                }
                catch (Exception x)
                {
                    throw x;
                }
                
            });
        }

        [HttpPost]
        public Task<bool> PostUpdateDocument(ta_MasterOfDocument doc)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<bool> PostNewCrystalReportRevision(ta_CrystalReportRevision1 rpt)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    rpt.datestamp = Application.DateTime(false);
                    Services.DbOperation(rpt, Enums.EntityMode.Insert);
                    return true;
                }
                catch (Exception x)
                {

                    throw x;
                }
            });
        }

        [HttpPost]
        public Task<bool> PostUpdateCrystalReportRevision(ta_CrystalReportRevision1 rpt)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<IEnumerable<ta_MasterOfDocument>> GetAllDocuments()
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return null;
                }
                try
                {
                    return Services.GetAllDocuments(false);
                }
                catch (Exception x)
                {

                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<ta_MasterOfDocument> GetDocument(string par1)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return null;
                }
                try
                {
                    var data = Services.GetAllDocuments(false).SingleOrDefault(r => r.DocumentName == par1);
                    return data;
                }
                catch (Exception x)
                {

                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<int> GetLastReportRevision(string par1)
        {
            try
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                return Services.GetLastReportRevision(false, par1);
            }
            catch (Exception x)
            {
                throw x;
            }
        }

    }
}

using Kmo.Libs;
using Kmo.Modules._650_SYS.DataStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kmo.WebApi.Controllers._650_SYS
{
    [RoutePrefix("sysapi/datastorage")]
    public class DataStorageController : ApiController
    {
        [HttpPost]
        public Task<bool> UploadBinary(ta_BinaryData bin)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return false;
                }
                try
                {
                    Application.BinToStorage(false, bin);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        [HttpPost]
        public Task<ta_BinaryData> DownloadBinary([FromBody]Guid par1)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return null;
                }
                using (var dc = new sysDataContext(Application._DefaultCn))
                {
                    var data = dc.ta_BinaryDatas.SingleOrDefault(r => r.BinaryDataId == par1);
                    return data;
                }
            });
        }

        [HttpPost]
        public Task<Guid?> PostLog(ta_Log par1)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return (Guid?)null;
                }
                try
                {
                    using (var dc = new sysDataContext(Application._DefaultCn))
                    {
                        par1.Date = Application.DateTime(false);
                        dc.ta_Logs.InsertOnSubmit(par1);
                        dc.SubmitChanges();
                        return par1.ID;
                    }
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpPost]
        public Task<byte[]> BuildReport(ReportParameterCollection pars)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    return Application.Report_Crpt_BuildData(pars).ContinueWith(r =>
                    {
                        var data = r.Result.GZipCompress();

                        return data;
                    }).Result;
                }
                catch (Exception x)
                {

                    throw x;
                }
            });
        }
    }
}

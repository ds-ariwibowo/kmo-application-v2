using Kmo.Modules._810_BDC.SuiteAvailabilityStatus;
using Kmo.Modules._810_BDC.SuiteAvailabilityStatus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kmo.WebApi.Controllers._810_BDC
{
    [RoutePrefix("bdcapi/suiteavailabilitystatus")]
    public class SuiteAvailabilityStatusController : ApiController
    {
        [HttpGet]
        public Task<HttpResponseMessage> GetAllSuites(string par1)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    var data = Services.GetAllSuites(false, par1).ToList().AsEnumerable();
                    
                    var dataString = JsonConvert.SerializeObject(data);
                    var resultData = new Libs.KmoJsonResult<IEnumerable<vi_SAS>>(dataString);
                    resultData.Success = true;

                    return new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(resultData)) };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<IEnumerable<vi_SAS>>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    var xx = JsonConvert.SerializeObject(resultData);
                    return new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(resultData)) };
                }
            });
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetAllFloor(string par1)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    var data = Services.GetAllFloor(false, par1).ToList().AsEnumerable();

                    var dataString = JsonConvert.SerializeObject(data);
                    var resultData = new Libs.KmoJsonResult<IEnumerable<vi_SAS>>(dataString);
                    resultData.Success = true;
                    return new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(resultData)) };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<IEnumerable<vi_SAS>>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    return new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(resultData)) };
                }
            });
        }

        [HttpPost]
        public Task<bool> PostSuite(ta_SUIT data)
        {
            return Task.Factory.StartNew(() => 
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }

                    data.SuitsId = Services.GetAllSuites(false).Where(r => r.FloorsId == data.FloorId).Count() + 1;
                    data.SuiteOrder = data.SuitsId;
                    Application.DbOperation<ta_SUIT>(data, Enums.EntityMode.Insert, Services.GetDataContext());
                    return true;
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpPost]
        public Task<bool> PostFloor(ta_FLOOR data)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }
                    if (Services.GetAllFloor(false, data.HoldingCompanyId).Where(r => r.HoldingCompanyId == data.HoldingCompanyId && r.FloorsId == data.FloorsId).Count() > 0)
                    {
                        return false;
                    }

                    Application.DbOperation(data, Enums.EntityMode.Insert, Services.GetDataContext());
                    return true;
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<ta_SUIT> GetSuite(string par1, int par2, string par3)
        {
            return Task.Factory.StartNew(() => 
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }
                    var data = Services.GetDataContext().ta_SUITs.SingleOrDefault(r => r.FloorId == par1 && r.SuitsId == par2 && r.HoldingCompanyId == par3).CloneObject();
                    return data;
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpPost]
        public Task<bool> UpdateSuite(ta_SUIT data)
        {
            return Task.Factory.StartNew(() => 
            {
                try
                {
                    Application.DbOperation(data, Enums.EntityMode.Update, Services.GetDataContext());
                    return true;
                }
                catch (Exception x)
                {

                    throw x;
                }
            });
        }

        [HttpPost]
        public Task<bool> VoidSuite(vi_SAS data)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var dc = Services.GetDataContext())
                    {
                        var upd = dc.ta_SUITs.Single(r => r.SuitsId == data.SuitsId && r.FloorId == data.FloorsId && r.HoldingCompanyId == data.HoldingCompanyId);
                        upd.Void = !data.Void;
                        dc.SubmitChanges();
                        return true;
                    }
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

    }
}

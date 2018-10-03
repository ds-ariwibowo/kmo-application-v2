using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Kmo.Modules._300_PMM.UtlEcd;
using Kmo.Libs;
using Kmo.Modules._300_PMM.UtlEcd.Models;

namespace Kmo.WebApi.Controllers._300_PMM
{
    [RoutePrefix("pmmapi/utlecd")]
    public class UtlEcdController : ApiController
    {
        [HttpGet]
        public Task<HttpResponseMessage> GetDataPartition(string par1)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    var data = Services.GetDataPartition(false, par1);
                    data.Success = true;
                    var dataString = JsonConvert.SerializeObject(data);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(dataString)
                    };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<DataPartition[]>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    var xx = JsonConvert.SerializeObject(resultData);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(resultData))
                    };
                }
            });
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetCompanyReading(string par1, string par2)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    var data = Services.GetCompanyReading(false, par1, par2);
                    data.Success = true;
                    var dataString = JsonConvert.SerializeObject(data);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(dataString)
                    };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<DataPartition[]>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    var xx = JsonConvert.SerializeObject(resultData);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(resultData))
                    };
                }
            });
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetCompanyFloorReading(string par1, string par2)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    var data = Services.GetCompanyFloorReading(false, par1, par2);
                    data.Success = true;
                    var dataString = JsonConvert.SerializeObject(data);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(dataString)
                    };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<ta_FLOORS_ECD_AcOutdoor[]>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    var xx = JsonConvert.SerializeObject(resultData);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(resultData))
                    };
                }
            });
        }

        [HttpPost]
        public Task<HttpResponseMessage> PostMonthlyReading()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }
                    var jsonString = Request.Content.ReadAsStringAsync().Result;
                    var jObj = JsonConvert.DeserializeObject<ta_SUITS_ECD>(jsonString);
                    var data = Services.PostMonthlyReading(false, jObj);
                    data.Success = true;
                    var dataString = JsonConvert.SerializeObject(data, Formatting.None);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(dataString)
                    };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<vi_SUITS_ECD>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    var xx = JsonConvert.SerializeObject(resultData);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(resultData))
                    };
                }
            });
        }

        [HttpGet]
        public Task<bool> ReadingExist(string par1, int par2, string par3, long par4)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    using (var dc = Services.GetDefaultDataContext())
                    {
                        var date = new DateTime(par4);
                        var count = dc.ta_SUITS_ECDs.Where(r => r.FloorId == par1 && r.SuitsId == par2 && r.HoldingCompanyId == par3 && r.Date == date.Date).Count();
                        return count > 0;
                    }
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<bool> DeleteReading(string par1, int par2, string par3)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    using (var dc = Services.GetDefaultDataContext())
                    {
                        var data = dc.ta_SUITS_ECDs.Single(r => r.FloorId == par1 && r.SuitsId == par2 && r.HoldingCompanyId == par3);
                        dc.ta_SUITS_ECDs.DeleteOnSubmit(data);
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

        [HttpGet]
        public Task<bool> ReadingFloorExist(string par1, string par2, long par3)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    using (var dc = Services.GetDefaultDataContext())
                    {
                        var date = new DateTime(par3);
                        var count = dc.ta_FLOORS_ECD_AcOutdoors.Where(r => r.FloorsId == par1 && r.HoldingCompanyId == par2 && r.Date == date.Date).Count();
                        return count > 0;
                    }
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpPost]
        public Task<HttpResponseMessage> PostMonthlyFloorOutdoorReading()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }

                    var jsonString = Request.Content.ReadAsStringAsync().Result;
                    var jObj = JsonConvert.DeserializeObject<ta_FLOORS_ECD_AcOutdoor>(jsonString);
                    jObj.datestamp = Application.DateTime(false);
                    var data = Services.PostMonthlyFloorOutdoorReading(false, jObj);
                    data.Success = true;
                    var dataString = JsonConvert.SerializeObject(data, Formatting.None);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(dataString)
                    };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<ta_FLOORS_ECD_AcOutdoor>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    var xx = JsonConvert.SerializeObject(resultData);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(resultData))
                    };
                }
            });
        }

        [HttpGet]
        public Task<bool> DeleteFloorReading(string par1, string par2)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    using (var dc = Services.GetDefaultDataContext())
                    {
                        var data = dc.ta_FLOORS_ECD_AcOutdoors.Single(r => r.FloorsId == par1 && r.HoldingCompanyId == par2);
                        dc.ta_FLOORS_ECD_AcOutdoors.DeleteOnSubmit(data);
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

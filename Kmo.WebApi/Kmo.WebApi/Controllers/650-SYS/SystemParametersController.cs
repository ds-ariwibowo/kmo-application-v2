using Kmo.Modules._650_SYS.SystemParameters;
using Kmo.Modules._650_SYS.SystemParameters.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kmo.WebApi.Controllers._650_SYS
{
    [RoutePrefix("sysapi/systemparameters")]
    public class SystemParametersController : ApiController
    {
        [HttpGet]
        public Task<Modules._650_SYS.SystemParameters.Models.ta_GlobalOption> GlobalOption(string par1)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return default(Modules._650_SYS.SystemParameters.Models.ta_GlobalOption);
                }
                using (var dc = new Modules._650_SYS.SystemParameters.Models.sysDataContext(Application._DefaultCn))
                {
                    var output = dc.ta_GlobalOptions.SingleOrDefault(r => r.Name == par1);

                    return output;
                }
            });
        }

        [HttpGet]
        public Task<bool> GlobalOption_Update(string par1, string par2)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return false;
                }
                try
                {
                    using (var dc = new Modules._650_SYS.SystemParameters.Models.sysDataContext(Application._DefaultCn))
                    {
                        var data = dc.ta_GlobalOptions.SingleOrDefault(r => r.Name == par1);
                        data.Value = par2;
                        dc.SubmitChanges();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            });
        }

        [HttpPost]
        public Task GlobalOption_New(Modules._650_SYS.SystemParameters.Models.ta_GlobalOption par1)
        {
            
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return;
                }
                try
                {
                    using (var dc = new Modules._650_SYS.SystemParameters.Models.sysDataContext(Application._DefaultCn))
                    {
                        var temp = dc.ta_GlobalOptions.Select(r => r.GlobalOptionId).ToList();
                        var id = 1;
                        if (temp.Count > 0)
                        {
                            id = temp.Max() + 1;
                        }

                        par1.GlobalOptionId = id;

                        dc.ta_GlobalOptions.InsertOnSubmit(par1);

                        dc.SubmitChanges();
                    }
                }
                catch 
                {
                    return;
                }
            });
        }

        [HttpGet]
        public Task<DateTime?> GetServerTime()
        {
            if (!Request.ValidateKmoAuthorizeToken())
            {
                return null;
            }
            return Task.Factory.StartNew(() => 
            {
                try
                {
                    return (DateTime?)Application.DateTime(false);
                }
                catch (Exception x)
                {
                    return null;
                }
            });
        }

        [HttpGet]
        public Task<IEnumerable<ta_GlobalOption>> GetAllGlobalOption()
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return null;
                }
                var data = Services.GetAllGlobalOption(false);
                return data.AsEnumerable();
            });
        }

        [HttpGet]
        public Task<IEnumerable<ta_HoldingCompany>> GetAllCompanyData()
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return null;
                }
                var data = Services.GetAllCompanyData(false);
                return data.AsEnumerable();
            });
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetAllPostalCode()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }

                    var data = Services.GetAllPostalCode(false).Result;
                    data.Success = true;
                    var dataString = JsonConvert.SerializeObject(data, Formatting.None);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(dataString)
                    };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<IEnumerable<ta_PostalCode>>();
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
        public Task<bool> PostNewPostalCode(ta_PostalCode data)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }
                    var result = Services.PostNewPostalCode(false, data);
                    return result;
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetPostalCode(string par1)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }

                    var data = Services.GetPostalCode(false, par1).Result;
                    data.Success = true;
                    var dataString = JsonConvert.SerializeObject(data, Formatting.None);
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(dataString)
                    };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<IEnumerable<ta_PostalCode>>();
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
    }
}

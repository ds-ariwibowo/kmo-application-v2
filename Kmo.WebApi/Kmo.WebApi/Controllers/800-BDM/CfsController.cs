using Kmo.Modules._800_BDM.ClientFactSheet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Kmo.Modules._800_BDM.ClientFactSheet;
using System.Windows.Forms;

namespace Kmo.WebApi.Controllers._800_BDM
{
    [RoutePrefix("bdmapi/cfs")]
    public class CfsController : ApiController
    {
        [HttpPost]
        public Task<HttpResponseMessage> PostNewCfs()
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
                    var jObj = JsonConvert.DeserializeObject<ta_CLIENT>(jsonString);
                    var result = Services.NewClient(false, jObj);
                    var jResult = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return new HttpResponseMessage { Content = new StringContent(jResult) };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<ta_CLIENT>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(resultData))
                    };
                }
            });
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetAllClients(string par1)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }

                    var result = Services.GetAllClients(false, par1);

                    var jResult = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return new HttpResponseMessage { Content = new StringContent(jResult) };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<ta_CLIENT>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(resultData))
                    };
                }
            });
        }

        [HttpPost]
        public Task<HttpResponseMessage> EditClient()
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
                    var jObj = JsonConvert.DeserializeObject<ta_CLIENT>(jsonString);
                    var result = Services.EditClient(false, jObj).Result;
                    var jResult = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    return new HttpResponseMessage { Content = new StringContent(jResult) };
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<ta_CLIENT>();
                    resultData.Success = false;
                    resultData.SetException(x);
                    resultData.ServerMessages = x.Message;
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(resultData))
                    };
                }
            });
        }

    }
}

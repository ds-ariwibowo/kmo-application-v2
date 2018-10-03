using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Kmo.Modules._900_CEO.LetterOfOffer;
using Kmo.Libs;
using Newtonsoft.Json;
using Kmo.Modules._900_CEO.LetterOfOffer.Models;

namespace Kmo.WebApi.Controllers._900_CEO
{
    [RoutePrefix("ceoapi/letterofoffer")]
    public class LetterOfOfferController : ApiController
    {
        [HttpGet]
        public Task<HttpResponseMessage> GetDataPartition(string par1)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }

                    using (var dc = Services.GetDefaultDataContext())
                    {
                        var data = dc.vi_LOOs.Where(r => r.HoldingCompanyId == par1).Select(r => r.LooId.Substring(3)).Distinct().ToArray().Select(r => new Libs.DataPartition { Month = int.Parse(r.Substring(0, 2)), Year = 2000 + int.Parse(r.Substring(2)), MoYe = r }).ToList();
                        var jString = JsonConvert.SerializeObject(data);
                        var result = new KmoJsonResult<DataPartition>(jString);
                        result.Success = true;
                        var jResult = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                        return new HttpResponseMessage { Content = new StringContent(jResult) };
                    }
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<DataPartition>();
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
        public Task<HttpResponseMessage> GetLoo(string par1, string par2)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }

                    using (var dc = Services.GetDefaultDataContext())
                    {
                        var data = dc.vi_LOOs.Where(r => r.HoldingCompanyId == par1 && r.LooId.EndsWith(par2)).ToList();
                        var jString = JsonConvert.SerializeObject(data);
                        var result = new KmoJsonResult<vi_LOO>(jString);
                        result.Success = true;
                        var jResult = JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                        return new HttpResponseMessage { Content = new StringContent(jResult) };
                    }
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<DataPartition>();
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
        public Task<HttpResponseMessage> PostNewLoo()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }

                    using (var dc = Services.GetDefaultDataContext())
                    {
                        var jString = Request.Content.ReadAsStringAsync().Result;
                        var data = JsonConvert.DeserializeObject<ta_LOO>(jString);
                        var result = Services.NewLoo(false, data).Result;
                        var jResult = Application.JsonSerialize(result);
                        return new HttpResponseMessage { Content = new StringContent(jResult) };
                    }
                }
                catch (Exception x)
                {
                    var resultData = new Libs.KmoJsonResult<DataPartition>();
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

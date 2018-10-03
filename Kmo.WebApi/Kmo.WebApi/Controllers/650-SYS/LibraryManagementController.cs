using Kmo.Modules._650_SYS.LibraryManagement;
using Kmo.Modules._650_SYS.LibraryManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Serialization;

namespace Kmo.WebApi.Controllers._650_SYS
{
    [RoutePrefix("sysapi/LibraryManagement")]
    public class LibraryManagementController : ApiController
    {
        [HttpPost]
        public Task<Guid> PostLibrary(ta_DesktopModulesLibrary data)
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
                        var dbData = dc.ta_DesktopModulesLibraries.SingleOrDefault(r => r.DesktopLibraryName == data.DesktopLibraryName);
                        data.DesktopLibraryId = dbData == null ? Guid.NewGuid() : dbData.DesktopLibraryId;
                        Application.DbOperation(data, dbData == null ? Enums.EntityMode.Insert : Enums.EntityMode.Update, dc);
                        return data.DesktopLibraryId;
                    }
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpPost]
        public Task<bool> PostLibraryData()
        {
            return Task.Factory.StartNew(() => 
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }
                    var validToken = Request.ValidateKmoAuthorizeToken();
                    MemoryStream ms = new MemoryStream();
                    Request.Content.ReadAsStreamAsync().Result.CopyTo(ms);
                    ms.Position = 0;
                    BinaryFormatter bf = new BinaryFormatter();
                    var libs = (List<LibraryStub>)bf.Deserialize(ms);
                    Task.Factory.StartNew(() => 
                    {
                        foreach (var item in libs)
                        {
                            using (var dc = Services.GetDefaultDataContext())
                            {
                                var bin = item.BinaryData;
                                var binDataId = Application.BinToStorage(false, item.AssemblyName, item.FileExtension, bin, null).Result;

                                var data = new ta_DesktopModulesLibrariesData
                                {
                                    Active = true,
                                    AssemblyName = item.AssemblyName,
                                    DesktopLibraryId = item.DesktopLibraryId,
                                    DesktopLibraryRevisionId = item.DesktopLibraryRevisionId,
                                    FileExtension = item.FileExtension,
                                    Md5Hash = item.Md5Hash,
                                    RevisionNote = item.RevisionNote,
                                    UsersId = item.UsersId,
                                    BinaryDataId = binDataId,
                                };
                                Application.DbOperation(data, Enums.EntityMode.Insert, dc);
                            }
                        }
                    }).Wait();
                    return true;
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<IEnumerable<ta_DesktopModulesLibrary>> GetLibs()
        {
            return Task.Factory.StartNew(() => 
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        throw new Exception("Invalid Token");
                    }
                    return Services.GetLibs(false).ToList().AsEnumerable();
                }
                catch (Exception x)
                {

                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<int> GetLastLibraryRevision(Guid par1)
        {
            return Task.Factory.StartNew(() => 
            {
                var result = Services.GetLastLibraryRevision(false, par1);
                return result;
            });
        }

        [HttpGet]
        public Task<IEnumerable<vi_DesktopLibrary>> GetCurrentLibraries()
        {
            return Task.Factory.StartNew(() => 
            {
                var data = Services.GetDefaultDataContext().vi_DesktopLibraries.ToArray();
                return data.AsEnumerable();
            });
        }

        [HttpGet]
        public Task<byte[]> GetLibraryBinary(Guid par1)
        {
            return Task.Factory.StartNew(() => 
            {
                var data = Application.BinFromStorage(false, par1).Result;
                return data;
            });
        }

        

    }
}

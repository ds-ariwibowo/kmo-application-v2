using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Kmo.Modules._650_SYS.ModuleManagement;
using Kmo.Modules._650_SYS.ModuleManagement.Models;
using System.Reflection;
using Newtonsoft.Json;

namespace Kmo.WebApi.Controllers._650_SYS
{
    [RoutePrefix("sysapi/modules")]
    public class ModulesController : ApiController
    {
        [HttpGet]
        public Task<ta_DesktopModule> GetModule(string par1, string par2, string par3)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return default(ta_DesktopModule);
                }

                var module = Services.GetDbData<ta_DesktopModule>().SingleOrDefault(r => r.Name == par1);
                if (module == null)
                {
                    module = new ta_DesktopModule();
                    module.Name = par1;
                    module.DesktopModulesId = Services.GetDbData<ta_DesktopModule>().Count() + 1;
                    module.DisplayName = par2;
                    module.ModulePath = string.IsNullOrEmpty(par3) ? "/@module" : par3.Replace('~', '/') ;
                    Services.DbOperation(module, Enums.EntityMode.Insert);
                }

                return module;
            });
        }

        [HttpPut]
        public Task PostModuleRevision(ta_DesktopModulesRevision1 revision)
        {
            return Task.Factory.StartNew(() => 
            {

                Services.DbOperation(revision, Enums.EntityMode.Insert);
            });
        }

        [HttpGet]
        public Task<int> GetLastModuleRevision(string par1)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return -1;
                }
                using (var dc = new sysModulesDataContext(Application._DefaultCn))
                {
                    var data = dc.ta_DesktopModules.SingleOrDefault(r => r.Name == par1);
                    if (data != null)
                    {
                        if (data.ta_DesktopModulesRevisions.Count > 0)
                        {
                            return data.ta_DesktopModulesRevisions.Max(r => r.ModulesRevisionId);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
            });
        }

        [HttpGet]
        public Task<IEnumerable<ta_DesktopModule>> GetModules()
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return default(IEnumerable<ta_DesktopModule>);
                }
                var data = Services.GetDbData<ta_DesktopModule>().ToList();
                return data.AsEnumerable();
            });
        }

        [HttpGet]
        public Task<bool> DeleteModule(int par1, int par2)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return false;
                }
                try
                {
                    using (var dc = Services.GetDefaultContext())
                    {
                        Application.DbOperation<ta_DesktopModulesRole>(Application.GetDbData<ta_DesktopModulesRole>(dc).Single(r => r.DesktopModulesId == par1 && r.RolesId == par2), Enums.EntityMode.Delete, dc);

                        return true;
                    }
                }
                catch (Exception x)
                {
                    return false;
                }
                
                
            });
        }

        [HttpGet]
        public Task<ta_DesktopModulesRole1> GetModuleRole(int par1, string par2)
        {
            return Task.Factory.StartNew(() => 
            {
                ta_DesktopModulesRole1 role = default(ta_DesktopModulesRole1);

                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return role;
                }
                
                using (var dc = new sysModulesDataContext(Application._DefaultCn))
                {
                    var module = dc.ta_DesktopModules.Single(r => r.DesktopModulesId == par1);
                    role = dc.ta_DesktopModulesRole1s.SingleOrDefault(r => r.DesktopModulesId == par1 && r.RoleName == par2);
                    if (role == null)
                    {
                        role = new ta_DesktopModulesRole1();
                        role.DesktopModulesId = par1;
                        if (module.ta_DesktopModulesRoles.Count > 0)
                        {
                            role.RolesId = (dc.ta_DesktopModulesRoles.Max(r => r.RolesId)) + 1;
                        }
                        else
                        {
                            role.RolesId = 1;
                        }
                        role.RoleName = par2;
                        dc.ta_DesktopModulesRole1s.InsertOnSubmit(role);
                        dc.SubmitChanges();
                    }
                    return role;
                }
            });
        }

        [HttpGet]
        public Task<bool> UpdateModule(int par1, string par2, string par3)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    using (var dc = Services.GetDefaultContext())
                    {
                        var data = dc.ta_DesktopModules.Single(r => r.DesktopModulesId == par1);
                        data.DisplayName = par2;
                        data.ModulePath = par3.Replace('~', '/');
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

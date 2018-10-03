using Aan.CustomExpressions;
using Kmo.Libs;
using Kmo.Modules._650_SYS.UserManagement;
using Kmo.Modules._650_SYS.UserManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kmo.WebApi.Controllers._650_SYS
{
    [RoutePrefix("sysapi/Users")]
    public class UsersController : ApiController
    {
        [HttpPost]
        public Libs.ServerPingResult Ping(Libs.ServerPing pingData)
        {            
            var user = Application.GetUser(pingData.UserID);
            Libs.ServerPingResult result = new Libs.ServerPingResult();
            result.Authenticated = false;

            if (user == null)
            {
                result.Messages = "Invalid User";
                result.Token = Guid.Empty;
                result.TokenExpired = default(DateTime);
            }
            else
            {
                if (user.UserID == "Admin")
                {
                    if (user.CheckPassword(false, pingData.Password.HashSha512().HexaString()))
                    {
                        result.Authenticated = true;
                        result.Messages = "OK";

                        var token = Application.NewAuthenticationToken();

                        result.Token = token.AuthenticationTokenId;
                        result.TokenExpired = token.Expired;
                    }
                    else
                    {
                        result.Messages = "Invalid User Or Password";
                        result.Token = Guid.Empty;
                        result.TokenExpired = default(DateTime);
                    }

                }
                else if (string.IsNullOrEmpty(pingData.Password))
                {
                    result.Messages = "Invalid Password";
                    result.Token = Guid.Empty;
                    result.TokenExpired = default(DateTime);
                }
                else
                {
                    var ta_usr = Services.GetDefaultContext().ta_Users.Single(r => r.UsersId == user.UserID);
                    
                    var equalPass = ta_usr.PasswordHash.ToArray().HexaString().HashComparer(pingData.Password.HashSha512().HexaString());
                    if (equalPass)
                    {
                        result.Authenticated = true;
                        result.Messages = "OK";

                        var token = Application.NewAuthenticationToken();

                        result.Token = token.AuthenticationTokenId;
                        result.TokenExpired = token.Expired;
                    }
                    else
                    {
                        result.Messages = "Invalid User Or Password";
                        result.Token = Guid.Empty;
                        result.TokenExpired = default(DateTime);
                    }
                }
            }
            return result;
        }

        [HttpGet]
        public Task<Dictionary<string, string>> RequestConnStrDictionary()
        {
            return Task.Factory.StartNew(() =>
            {
                if (Request.ValidateKmoAuthorizeToken())
                {
                    return Application.GetServerConnectionStrings();
                }
                else
                {
                    return null;
                }
            });
        }

        [HttpGet]
        public IEnumerable<Modules._650_SYS.UserManagement.Models.ta_User> ListCompany()
        {

            var dc = new Kmo.Modules._650_SYS.UserManagement.Models.sysModulesDataContext(Application._DefaultCn);

            var data = dc.ta_Users.ToList();
            return data;
        }

        [HttpPost]
        public Task<Guid> RequestPermanentToken([FromBody]string UsersId)
        {
            return Task.Factory.StartNew(() => 
            {
                return Modules._650_SYS.UserManagement.Services.GetPermanentToken(UsersId);
            });
        }

        [HttpPost]
        public Task<KmoWebApiResult> ListOfUsers(KmoWebApiRequest request)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = new KmoWebApiResult();
                result.Success = false;
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    result.ServerMessages.Add("Invalid Token", "Authorization Token is Invalid");
                    return result;
                }
                try
                {
                    if (request.Expression != null)
                    {
                        var exp = (Expression<Func<Modules._650_SYS.UserManagement.Models.ta_User, bool>>)request.Expression.BuildExpression();
                        var list = new Modules._650_SYS.UserManagement.Models.sysModulesDataContext(Application._DefaultCn).ta_Users.Where(exp).ToList();
                        result.JsonObject = JsonConvert.SerializeObject(list);
                        result.Success = true;
                        return result;
                    }
                    else
                    {
                        
                        var list = new Modules._650_SYS.UserManagement.Models.sysModulesDataContext(Application._DefaultCn).ta_Users.ToList();
                        result.JsonObject = JsonConvert.SerializeObject(list);
                        result.Success = true;
                        return result;
                    }
                }
                catch (Exception x)
                {
                    result.ServerMessages.Add("Exception", x.Message);
                    return result;
                }
            });
        }

        [HttpPost]
        public Task<bool> ValidateTokenLife([FromBody]Guid token)
        {
            return Task.Factory.StartNew(() => 
            {
                var dbData = Application.GetDbData<Modules._650_SYS.UserManagement.Models.vi_ListOfToken>(new Modules._650_SYS.UserManagement.Models.sysModulesDataContext(Application._DefaultCn))
                .SingleOrDefault(r => r.AuthenticationTokenId == token);

                return dbData != null;
            });
        }

        [HttpGet]
        public Task<UserInfo> GetUser(string par1)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return null;
                }
                var uInfo = Application.GetUser(par1);
                return uInfo;
            });
        }

        [HttpGet]
        public Task<ta_User> GetUserDb(string par1)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return null;
                }
                ta_User data = default(ta_User);

                data = new sysModulesDataContext(Application._DefaultCn).ta_Users.Single(r => r.UsersId == par1);

                return data;
            });
        }

        [HttpGet]
        public Task<IEnumerable<vi_User>> GetUsersDbView()
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return null;
                }
                IEnumerable<vi_User> data = default(IEnumerable<vi_User>);

                data = Services.GetUsers(false).ToList().AsEnumerable();

                return data;
            });
        }

        [HttpGet]
        public Task<vi_User> GetUserDbView(string par1)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return null;
                }
                vi_User data = default(vi_User);

                data = Services.GetUser(false, par1);

                return data;
            });
        }

        [HttpPost]
        public Task<bool> PostNewUser(ta_User user)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return false;
                }
                try
                {
                    Services.DbOperation(user, Enums.EntityMode.Insert);
                    return true;
                }
                catch (Exception x)
                {

                    throw x;
                }
            });
        }

        [HttpPost]
        public Task<bool> UpdateUser(ta_User user)
        {
            return Task.Factory.StartNew(() => 
            {
                try
                {
                    Services.DbOperation(user, Enums.EntityMode.Update);
                    return true;
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<bool> UpdateUserModuleAccess(int par1, string par2, bool par3)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return false;
                }
                using (var dc = new Modules._650_SYS.UserManagement.Models.sysModulesDataContext(Application._DefaultCn))
                {
                    if (par3)
                    {
                        var row = new Modules._650_SYS.UserManagement.Models.ta_UsersDesktopModule();
                        row.UsersId = par2;
                        row.DesktopModulesId = par1;
                        dc.ta_UsersDesktopModules.InsertOnSubmit(row);
                    }
                    else
                    {
                        var row = dc.ta_UsersDesktopModules.Single(r => r.DesktopModulesId == par1 && r.UsersId == par2);
                        dc.ta_UsersDesktopModules.DeleteOnSubmit(row);
                    }
                    dc.SubmitChanges();
                    return true;
                }
            });
        }

        [HttpGet]
        public Task<bool> UpdateUserRole(int par1, string par2, int par3, bool par4)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Request.ValidateKmoAuthorizeToken())
                    {
                        return false;
                    }
                    using (var dc = new Modules._650_SYS.UserManagement.Models.sysModulesDataContext(Application._DefaultCn))
                    {
                        if (par4)
                        {
                            var row = new Modules._650_SYS.UserManagement.Models.ta_UsersDesktopModulesAccess();
                            row.UsersId = par2;
                            row.DesktopModulesId = par1;
                            row.RolesId = par3;
                            dc.ta_UsersDesktopModulesAccesses.InsertOnSubmit(row);
                        }
                        else
                        {
                            var row = dc.ta_UsersDesktopModulesAccesses.Single(r => r.DesktopModulesId == par1 && r.UsersId == par2 && r.RolesId == par3);
                            dc.ta_UsersDesktopModulesAccesses.DeleteOnSubmit(row);
                        }
                        dc.SubmitChanges();
                    }
                    return true;
                }
                catch (Exception x)
                {
                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<bool> DeleteUser(string par1)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    return false;
                }
                
                using (var dc = Services.GetDefaultContext())
                {
                    var data = Application.GetDbData<ta_User>(dc).Single(r => r.UsersId == par1);
                    Application.DbOperation(data, Enums.EntityMode.Delete, dc);
                }
                return true;
            });
            
        }

        [HttpGet]
        public Task<bool> UpdateUserCompanyRole(string par1, string par2, bool par3)
        {
            return Task.Factory.StartNew(() => 
            {
                //ta_UsersDesktopModulesAccess
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    if (par3)
                    {
                        var data = new ta_UsersCompany()
                        {
                            UsersId = par1,
                            HoldingCompanyId = par2,
                        };
                        Application.DbOperation(data, Enums.EntityMode.Insert, Services.GetDefaultContext());
                        return true;
                    }
                    else
                    {
                        using (var dc = Services.GetDefaultContext())
                        {
                            dc.ta_UsersCompanies.DeleteOnSubmit(dc.ta_UsersCompanies.Single(r => r.UsersId == par1 && r.HoldingCompanyId == par2));
                            dc.SubmitChanges();
                        }
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
        public Task<IEnumerable<vi_UsersCompany>> GetUserCompany(string par1)
        {
            return Task.Factory.StartNew(() => 
            {
                if (!Request.ValidateKmoAuthorizeToken())
                {
                    throw new Exception("Invalid Token");
                }
                try
                {
                    return Services.GetUserCompany(false, par1).ToList().AsEnumerable();
                }
                catch (Exception x)
                {

                    throw x;
                }
            });
        }

        [HttpGet]
        public Task<IEnumerable<vi_UsersModule1>> GetUserModules(string par1)
        {
            return Task.Factory.StartNew(() => 
            {
                var data = Services.GetDefaultContext().vi_UsersModule1s.Where(r => r.UsersId == par1 && r.ModuleAllowed).ToList();

                return data.AsEnumerable();
            });
            throw new NotImplementedException();
        }

    }
}

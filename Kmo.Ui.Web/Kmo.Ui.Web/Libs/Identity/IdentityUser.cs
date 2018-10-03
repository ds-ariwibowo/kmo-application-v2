using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Kmo.Libs.Identity
{
    public class IdentityUser : IUser<string>
    {
        UserInfo _uInfo;

        public IdentityUser(string userID)
        {
            _uInfo = Application.GetUser(userID);
        }

        public string Id
        {
            get
            {
                return _uInfo.UserID;
            }
        }

        public string UserName
        {
            get
            {
                return _uInfo.FullName;
            }

            set
            {
                throw new NotSupportedException("This Action is Not Supported");
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<IdentityUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public Libs.UserInfo UserInfo
        {
            get
            {
                return _uInfo;
            }
        }
    }
}
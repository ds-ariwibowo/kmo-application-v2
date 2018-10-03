using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Kmo.Libs.Identity
{
    public class UserStorage : 
        IUserStore<IdentityUser>
        , IUserPasswordStore<IdentityUser>
        , IUserLockoutStore<IdentityUser, string>
        , IUserTwoFactorStore<IdentityUser, string>

    {
        #region Not Used

        public Task CreateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }
       
        public Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            System.Threading.Tasks.Task<bool> logonTask = System.Threading.Tasks.Task<bool>.Factory.StartNew(() =>
            {
                return false;
            });
            return logonTask;
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            System.Threading.Tasks.Task<bool> task = System.Threading.Tasks.Task<bool>.Factory.StartNew(() =>
            {
                return false;
            });
            return task;
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Used

        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var val = new IdentityUser(userId);
                    return val.UserInfo == null ? null : val;
                }
                catch
                {
                    return null;
                }
            });
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew(() => 
            {
                try
                {
                    var val = new IdentityUser(userName);
                    return val.UserInfo == null ? null : val;
                }
                catch 
                {
                    return null;
                }
            });
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            return Task.Factory.StartNew(() => { return 0; });
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            return Task.Factory.StartNew(() => { return user.UserInfo.GetHash(false); });

        }

        public void Dispose()
        {
        }

        #endregion


    }
}
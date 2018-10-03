using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kmo.Libs.Identity
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var equal = hashedPassword == providedPassword.HashSha512().HexaString();
            var output = equal ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
            return output;
        }
    }
}
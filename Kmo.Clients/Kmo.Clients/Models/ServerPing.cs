using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kmo.Clients.Models
{
    public class ServerPing
    {
        public ServerPing()
        {

        }

        public ServerPing(ClientType ClientType, string UserID, string Password)
        {
            this.ClientType = ClientType;
            this.UserID = UserID;
            this.Password = Password;
        }

        public ClientType ClientType;
        public string UserID;
        public string Password;
    }
}

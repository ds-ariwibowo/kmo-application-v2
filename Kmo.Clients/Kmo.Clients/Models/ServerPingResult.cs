using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kmo.Clients.Models
{
    [DataContract(Name = "ServerPingResult", Namespace = "Kmo.Libs")]
    public class ServerPingResult
    {
        [DataMember()]
        public Guid Token;

        [DataMember()]
        public bool Authenticated;

        [DataMember()]
        public string Messages;

        [DataMember()]
        public DateTime TokenExpired;
    }
}

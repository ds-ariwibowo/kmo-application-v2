using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kmo.Modules._800_BDM.ClientFactSheet.Models
{
    public class ClientPics
    {
        public IEnumerable<Models.ta_CLIENT1> Clients { get; set; }

        public IEnumerable<Models.ta_CLIENTS_PIC1> ClientsPic { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kmo.Libs
{
    public class ReportViewModel
    {
        public int CurrentPage { get; set; }

        public int TotalPage { get; set; }

        public string PageData { get; set; }

        public string SessionID { get; set; }
    }
}
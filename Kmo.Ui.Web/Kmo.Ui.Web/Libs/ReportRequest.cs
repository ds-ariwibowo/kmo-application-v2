using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kmo.Libs
{
    public class ReportRequest
    {
        public string SessionID { get; set; }

        public IEnumerable<Libs.ReportParameter> Parameters { get; set; }

        public string ReportName { get; set; }

        public string RecordSelectionFormula { get; set; }

        public string Title { get; set; }

    }
}
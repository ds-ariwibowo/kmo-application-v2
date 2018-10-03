using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kmo.Ui.Desktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var form = new Forms.DeveloperForm();

            form.RegisterModuleType(typeof(Modules._300_PMM.UtlEcd.ctl_MasterForm), "Electricity Consumption Distribution", "300-PMM");
            form.RegisterModuleType(typeof(Modules._800_BDM.ClientFactSheet.ctl_MasterForm), "Client Fact Sheet", "800-BDM");
            form.RegisterModuleType(typeof(Modules._810_BDC.SuiteAvailabilityStatus.ctl_MasterForm), "Suite Availability Status", "810-SAS");
            form.RegisterModuleType(typeof(Modules._900_CEO.LetterOfOffer.ctl_MasterForm), "Letter of Offer", "900-CEO");

            form.RegisterModuleType(typeof(Modules._650_SYS.ModuleManagement.ctl_MasterForm), "Module Management", "650-SYS");
            form.RegisterModuleType(typeof(Modules._650_SYS.UserManagement.ctl_MasterForm), "User Management", "650-SYS");
            form.RegisterModuleType(typeof(Modules._650_SYS.LibraryManagement.ctl_MasterForm), "Library Management", "650-SYS");
            form.RegisterModuleType(typeof(Modules._650_SYS.SystemParameters.ctl_MasterForm), "System Parameters", "650-SYS");
            form.RegisterModuleType(typeof(Modules._650_SYS.DocumentAndReport.ctl_MasterForm), "Documents And Reports", "650-SYS");

            System.Windows.Forms.Application.Run(form);
        }
    }
}

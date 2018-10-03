using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kmo.Clients
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{7B1FE636-4E48-4038-9E6A-70D51E97E80C}");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ValidateLocalLibraries().Wait();

            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                //foreach (var item in Directory.EnumerateFiles(@"D:\TEMP\ASM"))
                //{
                //    File.WriteAllBytes(@"D:\TEMP\ASM\comp\" + new FileInfo(item).Name, Application.DeflateCompress(File.ReadAllBytes(item)));
                //}
                

                Application.Pipes_Start();

                Application.ActiveForm = new logonForm();

                System.Windows.Forms.Application.Run(Application.ActiveForm);

                Application.GlobalCancelationToken?.Cancel();

                Application.MemMappedFile?.Dispose();
                mutex.ReleaseMutex();
            }
           
        }
    }
}

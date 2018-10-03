using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using System.Windows.Forms;
using Kmo.Clients.Models;

namespace Kmo
{
    public static partial class Application
    {
        
        public static NamedPipeServerStream Pipes_ConnectedServer { get; internal set; }

        public static Task Pipes_DesktopFrameworkRead { get; internal set; }

        public static Func<XElement, XElement> Pipes_MessagesHandler { get; internal set; }



        public static Guid? Pipes_ActiveID { get; set; }


        public static void Pipes_Start()
        {
            if (Pipes_DesktopFrameworkRead != null && Pipes_DesktopFrameworkRead.Status == TaskStatus.Running)
            {
                return;
            }

            Pipes_DesktopFrameworkRead = Task.Factory.StartNew(() => 
            {
                do
                {
                    using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("KMO Desktop Framework", PipeDirection.InOut))
                    {
                        pipeServer.WaitForConnection();
                        XElement resultEl = null;
                        Pipes_ConnectedServer = pipeServer;
                        try
                        {
                            byte[] bufferData = new byte[1024 * 63];
                            int readedBytes = 0;
                            var ms = new MemoryStream();
                            while ((readedBytes = pipeServer.Read(bufferData, 0, bufferData.Length)) > 0)
                            {
                                if (bufferData[readedBytes - 1] == 0x04)
                                {
                                    ms.Write(bufferData, 0, readedBytes - 1);
                                    resultEl = XElement.Load(new MemoryStream(Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(ms.ToArray()))));
                                    resultEl = Pipes_MessagesHandler?.Invoke(resultEl);
                                    bufferData = Encoding.UTF8.GetBytes(resultEl.ToString());
                                    pipeServer.Write(bufferData, 0, bufferData.Length);
                                    pipeServer.WriteByte(0x04);
                                    pipeServer.Flush();
                                    bufferData = new byte[1024 * 64];
                                    ms = new MemoryStream();
                                }
                                else
                                {
                                    ms.Write(bufferData, 0, readedBytes);
                                }
                            }

                            ms.Dispose();
                        }
                        catch (Exception x)
                        {
                            throw x;
                        }

                    }
                } while (!GlobalCancelationToken.IsCancellationRequested);
            }, GlobalCancelationToken.Token);

        }

        public static void Pipes_Send(XElement data)
        {
            if (Pipes_ActiveID == null)
            {
                return;
            }

            if (Pipes_ConnectedServer != null)
            {

            }

            using (var pipe = new NamedPipeClientStream(".", "client_" + Pipes_ActiveID.ToString()))
            {
                pipe.Connect();
                XElement el = new XElement("server");
                el.Add(data);
                var buffer = Encoding.UTF8.GetBytes(el.ToString());
                pipe.Write(buffer, 0, buffer.Length);
                pipe.Flush();
            }
        }

        public static XElement BuildMenuItem(List<vi_UsersModule> modules)
        {
            XElement outEl = new XElement("ToolStripMenuItems");
            foreach (var item in modules)
            {
                var el = new XElement("ToolStripMenuItem");
                el.Add(new XElement("Text", item.DisplayName));
                el.Add(new XElement("Name", item.ModuleName));
                el.Add(new XElement("FileName", item.FileName));
                el.Add(new XElement("FileExtension", item.FileExtension));
                el.Add(new XElement("ModulePath", item.ModuleStructure));
                el.Add(new XElement("DisplayName", item.DisplayName));
                outEl.Add(el);
            }
            return outEl;
        }
    }
}

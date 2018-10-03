using Kmo.Clients.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Kmo
{
    public static partial class Application
    {
        public static string LibPath
        {
            get
            {
                if (!Directory.Exists(Path.GetTempPath() + "KMO Desktop Framework\\"))
                {
                    Directory.CreateDirectory(Path.GetTempPath() + "KMO Desktop Framework\\");
                }
                return Path.GetTempPath() + "KMO Desktop Framework\\";
            }
        }
        private static Guid _ActiveToken;
        private static MemoryMappedFile _MemMappedFile;

        public static Form ActiveForm { get; set; }

        public static MemoryMappedFile MemMappedFile
        {
            get
            {
                return _MemMappedFile;
            }
            set
            {
                _MemMappedFile = value;
            }
        }

        public static Task ValidateLocalLibraries()
        {
            return Task.Factory.StartNew(() => 
            {
                try
                {
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Newtonsoft.Json.dll"))
                    {
                        File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "Newtonsoft.Json.dll", Kmo.Clients.Properties.Resources.Newtonsoft_Json.DeflateDecompress());
                    }
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "System.Net.Http.Formatting.dll"))
                    {
                        File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "System.Net.Http.Formatting.dll", Kmo.Clients.Properties.Resources.System_Net_Http_Formatting.DeflateDecompress());
                    }
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "System.Net.Http.dll"))
                    {
                        File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "System.Net.Http.dll", Kmo.Clients.Properties.Resources.System_Web_Http.DeflateDecompress());
                    }
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "System.Net.Http.WebRequest.dll"))
                    {
                        File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "System.Net.Http.WebRequest.dll", Kmo.Clients.Properties.Resources.System_Net_Http_WebRequest.DeflateDecompress());
                    }
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "System.Web.Http.dll"))
                    {
                        File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "System.Web.Http.dll", Kmo.Clients.Properties.Resources.System_Web_Http.DeflateDecompress());
                    }
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Kmo.Clients.exe.config"))
                    {
                        File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Kmo.Clients.exe.config", Kmo.Clients.Properties.Resources.Kmo_Clients_exe);
                        ProcessStartInfo proc = new ProcessStartInfo();
                        proc.UseShellExecute = true;
                        proc.WorkingDirectory = Environment.CurrentDirectory;
                        proc.FileName = System.Windows.Forms.Application.ExecutablePath;
                        Process.Start(proc);
                        Environment.Exit(0);
                    }
                }
                catch (Exception x)
                {
                    if (x is System.UnauthorizedAccessException)
                    {
                        ProcessStartInfo proc = new ProcessStartInfo();
                        proc.UseShellExecute = true;
                        proc.WorkingDirectory = Environment.CurrentDirectory;
                        proc.FileName = System.Windows.Forms.Application.ExecutablePath;
                        proc.Verb = "runas";

                        try
                        {
                            Process.Start(proc);
                        }
                        catch
                        {
                            // The user refused to allow privileges elevation.
                            // Do nothing and return directly ...
                            return;
                        }
                        
                        System.Windows.Forms.Application.Exit();  // Quit itself
                    }
                }
            });
        }

        public static Guid ActiveToken
        {
            get
            {
                return _ActiveToken;
            }
            set
            {
                _ActiveToken = value;
            }
        }

        public static RegistryKey RegKey
        {
            get
            {
                return Registry.CurrentUser.OpenSubKey("KMO", true) ?? Registry.CurrentUser.CreateSubKey("KMO", RegistryKeyPermissionCheck.ReadWriteSubTree);
            }
        }

        public static string WebApiLocation()
        {
            var key = RegKey.OpenSubKey("Desktop Client", RegistryKeyPermissionCheck.ReadWriteSubTree) ??
                    RegKey.CreateSubKey("Desktop Client", RegistryKeyPermissionCheck.ReadWriteSubTree);
            try
            {
                var uri = new Uri((string)key.GetValue("WebApi Location"));
                return uri.ToString().EndsWith("/") ? uri.ToString() : uri.ToString() + "/";
            }
            catch (Exception x)
            {
                return null;
            }
        }

        public static ServerPingResult PingWebApiServer(ServerPing client, string apiLoc)
        {
            HttpClient hClient = new HttpClient();
            var result = hClient.PostAsJsonAsync<ServerPing>(apiLoc + Constant.SysWebApiUrl.Ping, client).Result.Content.ReadAsAsync<ServerPingResult>().Result;
            return result;
        }

        public static vi_DesktopLibrary[] GetCurrentLibraries()
        {
            try
            {
                var res = ApiGet<IEnumerable<vi_DesktopLibrary>>(Constant.SysWebApiUrl.GetCurrentLibraries).Result;
                return res.ToArray();
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public static void FilterExistingLibraries(List<vi_DesktopLibrary> serverVersion)
        {
            if (File.Exists(LibPath + "LibInfo.xml"))
            {
                var root = XElement.Load(LibPath + "LibInfo.xml");
                
                
                foreach (var el in root.Elements("Library").ToArray())
                {
                    
                    var BinaryDataId = Guid.Parse(el.Element("BinaryDataId").Value);
                    var AssemblyName = el.Element("AssemblyName").Value;
                    var DesktopLibraryId = Guid.Parse(el.Element("DesktopLibraryId").Value);
                    var DesktopLibraryRevisionId = Convert.ToInt32(el.Element("DesktopLibraryRevisionId").Value);
                    var FileExtension = el.Element("FileExtension").Value;
                    var StartingPoint = Convert.ToBoolean(el.Element("StartingPoint").Value);

                    if (BinaryDataId.ToString() == "5c223abb-751c-46d8-9783-e5bcf1fc705d")
                    {

                    }

                    var versionData = serverVersion.SingleOrDefault(r => r.AssemblyName == AssemblyName);
                    if (versionData == null)
                    {
                        root.Elements().Where(r => r.Element("AssemblyName").Value == AssemblyName).Single().Remove();
                        //el.Remove();
                        if (File.Exists(LibPath + "Bin\\" + AssemblyName + FileExtension))
                        {
                            File.Delete(LibPath + "Bin\\" + AssemblyName + FileExtension);
                        }
                    }
                    else
                    {
                        if (DesktopLibraryRevisionId == versionData.DesktopLibraryRevisionId
                            && File.Exists(LibPath + "Bin\\" + versionData.AssemblyName + versionData.FileExtension))
                        {
                            serverVersion.Remove(versionData);
                        }
                        else
                        {
                            if (File.Exists(LibPath + "Bin\\" + versionData.AssemblyName + versionData.FileExtension))
                            {
                                File.Delete(LibPath + "Bin\\" + versionData.AssemblyName + versionData.FileExtension);
                            }
                            root.Elements().Where(r => r.Element("AssemblyName").Value == AssemblyName).Single().Remove();
                            //el.Remove();
                        }
                    }
                }

                foreach (var item in serverVersion)
                {
                    XElement AssemblyName = new XElement("AssemblyName", item.AssemblyName);
                    XElement BinaryDataId = new XElement("BinaryDataId", item.BinaryDataId);
                    XElement DesktopLibraryId = new XElement("DesktopLibraryId", item.DesktopLibraryId);
                    XElement DesktopLibraryRevisionId = new XElement("DesktopLibraryRevisionId", item.DesktopLibraryRevisionId);
                    XElement FileExtension = new XElement("FileExtension", item.FileExtension);
                    XElement StartingPoint = new XElement("StartingPoint", item.StartingPoint);

                    XElement libEl = new XElement("Library", AssemblyName, BinaryDataId, DesktopLibraryId, DesktopLibraryRevisionId, FileExtension, StartingPoint);
                    root.Add(libEl);
                }

                XDocument doc = new XDocument();
                
                doc.Add(root);
                doc.Save(LibPath + "LibInfo.xml");
            }
            else
            {
                if (Directory.Exists(LibPath + "Bin\\"))
                {
                    Directory.Delete(LibPath + "Bin\\", true);
                }

                XDocument doc = new XDocument();
                XElement root = new XElement("Libraries");
                
                foreach (var item in serverVersion)
                {
                    
                    XElement AssemblyName = new XElement("AssemblyName", item.AssemblyName);
                    XElement BinaryDataId = new XElement("BinaryDataId", item.BinaryDataId);
                    XElement DesktopLibraryId = new XElement("DesktopLibraryId", item.DesktopLibraryId);
                    XElement DesktopLibraryRevisionId = new XElement("DesktopLibraryRevisionId", item.DesktopLibraryRevisionId);
                    XElement FileExtension = new XElement("FileExtension", item.FileExtension);
                    XElement StartingPoint = new XElement("StartingPoint", item.StartingPoint);

                    XElement libEl = new XElement("Library", AssemblyName, BinaryDataId, DesktopLibraryId, DesktopLibraryRevisionId, FileExtension, StartingPoint);
                    root.Add(libEl);
                }
                doc.Add(root);
                doc.Save(LibPath + "LibInfo.xml");
            }
        }

        public static void FilterExistingModules(List<vi_UsersModule> serverVersion)
        {
            if (File.Exists(LibPath + "ModuleInfo.xml"))
            {
                var root = XElement.Load(LibPath + "ModuleInfo.xml");
                var newRoot = new XElement("Modules");
                foreach (var el in root.Elements())
                {
                    var DesktopModulesId = int.Parse(el.Element("DesktopModulesId").Value);
                    var ModuleName = el.Element("ModuleName").Value;
                    var ModuleRevisionId = int.Parse(el.Element("ModuleRevisionId").Value);
                    var FileName = el.Element("FileName").Value;
                    var BinaryDataId = Guid.Parse(el.Element("BinaryDataId").Value);
                    var ModuleStructure = el.Element("ModuleStructure").Value;

                    var versionData = serverVersion.SingleOrDefault(r => r.DesktopModulesId == DesktopModulesId);
                    if (versionData == null)
                    {
                        el.Remove();
                        if (File.Exists(LibPath + "Bin\\" + FileName))
                        {
                            File.Delete(LibPath + "Bin\\" + FileName);
                        }
                    }
                    else
                    {
                        if (ModuleRevisionId == versionData.ModuleRevisionId
                            && File.Exists(LibPath + "Bin\\" + FileName))
                        {
                            serverVersion.Remove(versionData);
                        }
                        else
                        {
                            if (File.Exists(LibPath + "Bin\\" + FileName))
                            {
                                File.Delete(LibPath + "Bin\\" + FileName);
                            }
                            el.Remove();
                        }
                    }
                }

            }
            else
            {
                XDocument doc = new XDocument();
                XElement root = new XElement("Modules");

                foreach (var item in serverVersion)
                {
                    XElement DesktopModulesId = new XElement("DesktopModulesId", item.DesktopModulesId);
                    XElement ModuleName = new XElement("ModuleName", item.ModuleName);
                    XElement ModuleRevisionId = new XElement("ModuleRevisionId", item.ModuleRevisionId);
                    XElement FileName = new XElement("FileName", item.FileName);
                    XElement FileExtension = new XElement("FileExtension", item.FileExtension);
                    XElement BinaryDataId = new XElement("BinaryDataId", item.BinaryDataId);
                    XElement ModuleStructure= new XElement("ModuleStructure", item.ModuleStructure);

                    XElement libEl = new XElement("Module", DesktopModulesId, ModuleName, ModuleRevisionId, FileName, FileExtension, BinaryDataId, ModuleStructure);
                    root.Add(libEl);
                }

                doc.Add(root);
                doc.Save(LibPath + "ModuleInfo.xml");
            }
        }

        private static CancellationTokenSource _GlobalCancelationToken;

        public static CancellationTokenSource GlobalCancelationToken
        {
            get
            {
                return _GlobalCancelationToken = _GlobalCancelationToken ?? new CancellationTokenSource();
            }

        }

        public static byte[] DownloadLib(Guid BinID)
        {
            var uri = Constant.SysWebApiUrl.GetLibraryBinary + "/" + BinID.ToString() + "/";
            var res = ApiGet<byte[]>(uri).Result;
            return res;
        }

        #region Compression

        public static byte[] DeflateCompress(this byte[] bin)
        {
            try
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (DeflateStream zip = new DeflateStream(ms, CompressionMode.Compress))
                    {
                        zip.Write(bin, 0, bin.Length);
                        zip.Flush();
                        zip.Close();
                        return ms.ToArray();
                    }

                }

            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public static byte[] DeflateDecompress(this byte[] bin)
        {
            try
            {
                using (System.IO.MemoryStream msCommpressed = new System.IO.MemoryStream())
                {
                    msCommpressed.Write(bin, 0, bin.Length);
                    msCommpressed.Flush();
                    msCommpressed.Position = 0;
                    //msCommpressed.Position = 0;
                    using (DeflateStream zip = new DeflateStream(msCommpressed, CompressionMode.Decompress))
                    {
                        using (System.IO.MemoryStream msDecompressed = new MemoryStream())
                        {
                            zip.CopyTo(msDecompressed);
                            zip.Flush();
                            zip.Close();
                            //msDecompressed.Position = 0;
                            msDecompressed.Flush();
                            msCommpressed.Close();
                            return msDecompressed.ToArray();
                        }
                    }
                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public static byte[] GZipCompress(this byte[] bin)
        {
            try
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (var zip = new GZipStream(ms, CompressionMode.Compress))
                    {
                        zip.Write(bin, 0, bin.Length);
                        zip.Flush();
                        zip.Close();
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        public static byte[] GZipDecompress(this byte[] bin)
        {
            try
            {
                using (System.IO.MemoryStream msCommpressed = new System.IO.MemoryStream())
                {
                    msCommpressed.Write(bin, 0, bin.Length);
                    msCommpressed.Flush();
                    msCommpressed.Position = 0;
                    //msCommpressed.Position = 0;
                    using (DeflateStream zip = new DeflateStream(msCommpressed, CompressionMode.Decompress))
                    {
                        using (System.IO.MemoryStream msDecompressed = new MemoryStream())
                        {
                            zip.CopyTo(msDecompressed);
                            zip.Flush();
                            zip.Close();
                            //msDecompressed.Position = 0;
                            msDecompressed.Flush();
                            msCommpressed.Close();
                            return msDecompressed.ToArray();
                        }
                    }
                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        #endregion

        #region WebApi Communication

        #region ApiGet

        public static Task<TResult> ApiGet<TResult>(string ApiUri)
        {
            return Task.Factory.StartNew<TResult>(() =>
            {
                var throwOnError = false;
                return ApiGet<TResult>(ApiUri, throwOnError).Result;
            });
        }

        public static Task<TResult> ApiGet<TResult>(string ApiUri, bool throwOnError)
        {
            return Task.Factory.StartNew<TResult>(() =>
            {
                Exception x = default(Exception);
                var output = ApiGet<TResult>(ApiUri, x);

                if (output.Equals(default(TResult)))
                {
                    if (throwOnError)
                    {
                        throw x;
                    }
                    else
                    {
                        return default(TResult);
                    }
                }
                return output.Result;
            });
        }

        public static Task<TResult> ApiGet<TResult>(string ApiUri, Exception x)
        {
            x = default(Exception);

            return Task.Factory.StartNew<TResult>(() =>
            {
                var contentString = "";
                try
                {
                    using (var http = new HttpClient().AddActiveToken())
                    {
                        Uri uri = new Uri(WebApiLocation() + ApiUri);
                        var result = http.GetAsync(uri).Result;
                        contentString = result.Content.ReadAsStringAsync().Result;
                        var ok = result.IsSuccessStatusCode;
                        
                        if (ok)
                        {
                            try
                            {
                                var resultData = JsonConvert.DeserializeObject<TResult>(contentString);
                                return resultData;
                            }
                            catch
                            {
                                
                            }

                            try
                            {
                                var resultData = result.Content.ReadAsAsync<TResult>().Result;
                                return resultData;
                            }
                            catch 
                            {
                                throw x;
                            }

                            
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch (Exception ex)
                {
                    x = ex;
                    x.Data.Add("HttpResponseMessages", contentString);
                }
                return default(TResult);
            });
        }

        #endregion

        #region ApiPostJson

        public static Task<TResult> ApiPostJson<TData, TResult>(string ApiUri, TData PostData)
        {
            return Task.Factory.StartNew(() =>
            {
                var throwOnError = false;
                return ApiPostJson<TData, TResult>(ApiUri, PostData, throwOnError).Result;
            });
        }

        public static Task<TResult> ApiPostJson<TData, TResult>(string ApiUri, TData postData, bool throwOnError)
        {
            return Task.Factory.StartNew<TResult>(() =>
            {
                Exception x = default(Exception);
                var output = ApiPostJson<TData, TResult>(ApiUri, postData, x);

                if (x != null || output.Equals(default(TResult)))
                {
                    if (throwOnError)
                    {
                        throw x;
                    }
                    else
                    {
                        return default(TResult);
                    }
                }
                return output.Result;
            });
        }

        public static Task<TResult> ApiPostJson<TData, TResult>(string ApiUri, TData postData, Exception x)
        {
            x = default(Exception);

            return Task.Factory.StartNew<TResult>(() =>
            {
                var contentString = "";

                try
                {
                    using (var http = new HttpClient().AddActiveToken())
                    {
                        
                        string uri = WebApiLocation() + ApiUri;
                        //var result = http.PostAsync<TData>(uri, postData, new System.Net.Http.Formatting.XmlMediaTypeFormatter()).Result;
                        var result = http.PostAsJsonAsync(uri, postData).Result;
                        contentString = result.Content.ReadAsStringAsync().Result;
                        var ok = result.IsSuccessStatusCode;
                        if (ok)
                        {
                            return result.Content.ReadAsAsync<TResult>().Result;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch (Exception ex)
                {
                    x = ex;
                    x.Data.Add("HttpResponseMessages", contentString);
                }
                return default(TResult);
            });
        }

        #endregion

        #region ApiPostXml

        public static Task<TResult> ApiPostXml<TData, TResult>(string ApiUri, TData PostData)
        {
            return Task.Factory.StartNew(() =>
            {
                var throwOnError = false;
                return ApiPostXml<TData, TResult>(ApiUri, PostData, throwOnError).Result;
            });
        }

        public static Task<TResult> ApiPostXml<TData, TResult>(string ApiUri, TData postData, bool throwOnError)
        {
            return Task.Factory.StartNew<TResult>(() =>
            {
                Exception x = default(Exception);
                var output = ApiPostXml<TData, TResult>(ApiUri, postData, x);

                if (x != null || output.Equals(default(TResult)))
                {
                    if (throwOnError)
                    {
                        throw x;
                    }
                    else
                    {
                        return default(TResult);
                    }
                }
                return output.Result;
            });
        }

        public static Task<TResult> ApiPostXml<TData, TResult>(string ApiUri, TData postData, Exception x)
        {
            x = default(Exception);

            return Task.Factory.StartNew(() =>
            {
                var contentString = "";

                try
                {
                    using (var http = new HttpClient().AddActiveToken())
                    {
                        string uri = WebApiLocation() + ApiUri;
                        var result = http.PostAsXmlAsync<TData>(uri, postData).Result;
                        contentString = result.Content.ReadAsStringAsync().Result;
                        var ok = result.IsSuccessStatusCode;
                        if (ok)
                        {
                            return result.Content.ReadAsAsync<TResult>().Result;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch (Exception ex)
                {
                    x = ex;
                    x.Data.Add("HttpResponseMessages", contentString);
                }
                return default(TResult);
            });
        }

        #endregion

        public static System.Net.Http.HttpClient AddActiveToken(this System.Net.Http.HttpClient client)
        {
            client.DefaultRequestHeaders.Add("AuthorizeToken", ActiveToken.ToString());
            return client;
        }


        #endregion

    }

    public enum LogType
    {
        InsertData = 0,
        UpdateData = 1,
        DeleteData = 2,
        SystemLog = 3,
        ExceptionLog = 4
    }

    public enum ClientType
    {
        DesktopClient,
        DesktopDeveloperClient,
        WebClient
    }
}

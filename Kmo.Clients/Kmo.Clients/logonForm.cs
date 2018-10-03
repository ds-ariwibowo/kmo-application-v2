using Kmo.Clients.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Kmo.Clients
{
    public partial class logonForm : Form
    {
        public logonForm()
        {
            InitializeComponent();
            Application.Pipes_MessagesHandler = new Func<XElement, XElement>(IncomingMessages);
            button1.Enabled = false;
            textBox2.Enabled = false;

        }

        private void LogonForm_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = Application.WebApiLocation();
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                ValidateLibraries();
            }

        }

        public bool libsValidated;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Visible = !Visible;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            windowsStateTollstripMenuItems.Text = !Visible ? "Restore Window" : "Hide Window";
        }

        private void windowsStateTollstripMenuItems_Click(object sender, EventArgs e)
        {
            Visible = !Visible;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Visible = false;
            }
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                notifyIcon1.ShowBalloonTip(1000, "Client Minimize", "KMO Client is Minimized, Not Closed. Right Click KMO Icon to Show Available Menu.", ToolTipIcon.Info);
            }
        }

        private Task ValidateLibraries()
        {
            var serverClient = new ServerPing();
            serverClient.ClientType = ClientType.DesktopClient;

            var apiLoc = textBox1.Text;
            libsValidated = false;
            progressBar1.Style = ProgressBarStyle.Marquee;
            msgLbl.Text = "Ping " + apiLoc;
            _setButton.Enabled = false;
            button1.Enabled = false;
            button1.Visible = false;

            return Task.Factory.StartNew(() => 
            {
                var result = Application.PingWebApiServer(serverClient, apiLoc);
                return result != null;

            }).ContinueWith(r => 
            {
                List<vi_DesktopLibrary> libsDef = new List<vi_DesktopLibrary>();
                if (r.Result)
                {
                    this.Invoke(new Action(() =>
                    {
                        msgLbl.Text = "Get Libraries Information.";
                    }));

                    libsDef = Application.GetCurrentLibraries().ToList();
                    Application.FilterExistingLibraries(libsDef);

                    this.Invoke(new Action(() =>
                    {
                        progressBar1.Value = 0;
                        progressBar1.Step = 1;
                        progressBar1.Maximum = libsDef.Count;
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        _setButton.Enabled = true;
                        button1.Enabled = true;
                        textBox2.Enabled = true;
                        msgLbl.Text = "Finished.";
                        textBox2.Focus();
                    }));

                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        _setButton.Enabled = true;

                        msgLbl.Text = "Cannot Ping Server.";
                    }));
                }

                return libsDef;                
            }).ContinueWith(r => 
            {
                if (r.Result.Count > 0)
                {
                    foreach (var item in r.Result)
                    {
                        this.Invoke(new Action(() =>
                        {
                            msgLbl.Text = "Downloading " + item.AssemblyName + item.FileExtension;                            
                        }));

                        var bin = Application.DownloadLib(item.BinaryDataId);
                        if (!Directory.Exists(Application.LibPath + "bin"))
                        {
                            Directory.CreateDirectory(Application.LibPath + "bin");
                        }
                        this.Invoke(new Action(() =>
                        {
                            msgLbl.Text = "Saving " + item.AssemblyName + item.FileExtension;
                        }));
                        File.WriteAllBytes(Application.LibPath + "bin\\" + item.AssemblyName + item.FileExtension, bin.DeflateDecompress());

                        Invoke(new Action((progressBar1.PerformStep)));
                    }
                }
            }).ContinueWith(r => 
            {
                this.Invoke(new Action(() =>
                {
                    progressBar1.Value = 0;
                    msgLbl.Text = "Libraries Downloaded";
                    libsValidated = true;
                }));
            });

        }

        private void _setButton_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.EndsWith("/"))
            {
                textBox1.Text += "/";
            }
            var key = Application.RegKey.OpenSubKey("Desktop Client", RegistryKeyPermissionCheck.ReadWriteSubTree) ??
                    Application.RegKey.CreateSubKey("Desktop Client", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.SetValue("WebApi Location", textBox1.Text);

            ValidateLibraries();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var result = Application.PingWebApiServer(new ServerPing { ClientType = ClientType.DesktopClient, UserID = textBox2.Text }, textBox1.Text);

                label4.Visible = result.Messages == "Invalid Password";
                textBox3.Visible = result.Messages == "Invalid Password";
                button1.Visible = result.Messages == "Invalid Password";

                msgLbl.Text = result.Messages == "Invalid Password" ? "Enter Password" : result.Messages;

                if (result.Messages == "Invalid Password")
                {
                    textBox3.Focus();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = Application.PingWebApiServer(new ServerPing { ClientType = ClientType.DesktopClient, UserID = textBox2.Text, Password = textBox3.Text }, textBox1.Text);

            if (result.Token == Guid.Empty)
            {
                msgLbl.Text = "Invalid Password";
            }
            else
            {
                _ActiveLogin = result;
                ValidateLibraries().ContinueWith(r =>
                {
                    Invoke(new Action(() =>
                    {
                        ValidateUserModules();
                        this.Hide();
                    }));
                });
                //this.Hide();
            }
        }

        private void ValidateUserModules()
        {
            var flag = false;
            textBox1.Enabled = flag;
            textBox2.Enabled = flag;
            textBox3.Enabled = flag;
            _setButton.Enabled = flag;
            button1.Enabled = flag;
            msgLbl.Text = "Checking Module for " + textBox2.Text;
            Task.Factory.StartNew(() => 
            {
                var result = Application.ApiGet<List<vi_UsersModule>>(Constant.SysWebApiUrl.GetUserModules + "/" + textBox2.Text + "/").Result;
                _LoadedUserModule = result.ToList();
                var output = result.Select(r => r.ModuleStructure).ToArray();
                Application.FilterExistingModules(result);
                foreach (var item in result)
                {
                    this.Invoke(new Action(() =>
                    {
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Value = 0;
                        progressBar1.Maximum = result.Count;
                        msgLbl.Text = "Downloading " + item.FileName;
                    }));
                    var bin = Application.DownloadLib(item.BinaryDataId);
                    if (!Directory.Exists(Application.LibPath + "bin"))
                    {
                        Directory.CreateDirectory(Application.LibPath + "bin");
                    }
                    this.Invoke(new Action(() =>
                    {
                        msgLbl.Text = "Saving " + item.FileName;
                    }));
                    File.WriteAllBytes(Application.LibPath + "bin\\" + item.FileName, bin);
                    Invoke(new Action((progressBar1.PerformStep)));
                    
                }
                this.Invoke(new Action(() =>
                {
                    progressBar1.Value = 0;
                    msgLbl.Text = "Libraries Downloaded";
                    libsValidated = true;
                    this.Hide();
                }));
                var el = XElement.Load(Application.LibPath + "LibInfo.xml");
                var startPoint = el.Elements("Library").Where(r => r.Element("StartingPoint").Value == "true");
                if (startPoint.Count() == 1)
                {
                    var startApp = Application.LibPath + "bin\\" + startPoint.Single().Element("AssemblyName").Value + startPoint.Single().Element("FileExtension").Value;
                    if (File.Exists(startApp))
                    {
                        _ClientProcess = new Process();
                        _ClientProcess.StartInfo = new ProcessStartInfo(startApp);
                        _ClientProcess.Start();
                        
                    }
                }
                return output;
            });
        }

        Process _ClientProcess;

        ServerPingResult _ActiveLogin;

        List<vi_UsersModule> _LoadedUserModule;
        

        private XElement IncomingMessages(XElement data)
        {
            if (data.Element("RequestUserInfo") != null)
            {
                if (_ActiveLogin == null)
                {
                    return new XElement("server", new XElement("UserNotLogin"));
                }
                else
                {
                    var el = new XElement("server");
                    var uInfo = new XElement("UserInfo");
                    uInfo.Add(new XElement("UserId", textBox2.Text));
                    uInfo.Add(new XElement("UserPassword", textBox3.Text));
                    uInfo.Add(new XElement("AuthenticationToken", _ActiveLogin.Token));
                    uInfo.Add(new XElement("TokenExpired", _ActiveLogin.TokenExpired));
                    el.Add(uInfo);
                    return el;
                }
            }
            else if (data.Element("RequestModules") != null)
            {
                var el = Application.BuildMenuItem(_LoadedUserModule);
                return el;
            }
            else if (data.Element("InfoClosing") != null)
            {
                BeginInvoke(new Action(() => 
                {
                    _ActiveLogin = null;
                    _LoadedUserModule = null;
                    var flag = true;
                    textBox1.Enabled = flag;
                    textBox2.Enabled = flag;
                    textBox3.Enabled = flag;
                    _setButton.Enabled = flag;
                    button1.Enabled = flag;
                    button1.Focus();
                    this.Show();
                }));
                return new XElement("server", new XElement("Bye"));
            }

            return null;
        }

        private void logonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Pipes_ConnectedServer?.Dispose();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _setButton.PerformClick();
            }
        }
    }
}

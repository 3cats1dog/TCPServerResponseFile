using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SuperSimpleTcp;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        SimpleTcpServer server;
        string ClientIpPort = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void AddLog(string txt)
        {
            this.UIThread(() =>
            {
                txtLogs.Text = txt + Environment.NewLine + txtLogs.Text;
            });

        }
        void ClientConnected(object sender, ConnectionEventArgs e)
        {
            ClientIpPort = e.IpPort;
            AddLog(string.Format("{0} client connected", e.IpPort));
            // once a client has connected...
            if (txtHelloMsg.Text != "")
            {
                SendData2Client(txtHelloMsg.Text);
            }
        }

        void ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            ClientIpPort = "";
            AddLog(string.Format("{0} client disconnected : {1}", e.IpPort, e.Reason));
        }

        void DataReceived(object sender, DataReceivedEventArgs e)
        {
            string recieveData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            CheckFileCommand(recieveData);
            AddLog(string.Format("{0}: {1}", e.IpPort,recieveData ));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                return;
            }
            server = new SimpleTcpServer(txtIP.Text, Convert.ToInt32(txtPort.Text));   // "127.0.0.1:9000"

            // set events
            server.Events.ClientConnected += ClientConnected;
            server.Events.ClientDisconnected += ClientDisconnected;
            server.Events.DataReceived += DataReceived;

            // let's go!
            server.Start();
            AddLog("Server start listening...");
            btnStop.Enabled = true;
            btnStart.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                return;
            }

            server.Stop();
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            server.Dispose();
            server = null;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK) return;

            txtFilePath.Text = ofd.FileName;
            FileInfo fi = new FileInfo(ofd.FileName);
            lblFileInfo.Text = fi.Length.ToString()  + " byte";
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            SendData2Client(txtMessage.Text);
        }

        void CheckFileCommand(string recieveCmd)
        {
            this.UIThread(() => {
                if (string.IsNullOrEmpty(txtFileTriggerCommand.Text)) return;

                if (recieveCmd.Contains(txtFileTriggerCommand.Text.Trim()))
                {
                    AddLog("File request triggered");
                    //Check file any file set;
                    if (string.IsNullOrEmpty(txtFilePath.Text))
                    {
                        AddLog("No file selected!!!");
                        return;
                    }
                    if (!File.Exists(txtFilePath.Text))
                    {
                        AddLog("selected file not exists!!!");
                        return;
                    }
                    //send file
                    SendFileBinary(txtFilePath.Text);
                }
            });
        }
        void SendData2Client(string txt)
        {

            if (server == null) return;
            if (ClientIpPort == "") return;
            if (chkAddLF.Checked) txt = txt + "\r\n";
            server.Send(ClientIpPort, txt);
        }

        void SendFileBinary(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            if (server == null) return;
            if (ClientIpPort == "") return;
            server.Send(ClientIpPort, data);
            AddLog("File sended");
        }
    }
}

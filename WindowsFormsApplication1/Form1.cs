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


namespace TCPServerResponseFile
{
    public partial class Form1 : Form
    {

        SimpleTcpServer server;
        string ClientIpPort = "";
        FileSend sending;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sending = new FileSend();
            sending.isSending = false;
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
            sending.isSending = false;
            AddLog(string.Format("{0} client disconnected : {1}", e.IpPort, e.Reason));
        }

        void DataReceived(object sender, DataReceivedEventArgs e)
        {
            byte firstChar = e.Data.Array[0];
            if (sending.isSending)
            {
                switch (firstChar)
                {
                    case Commands.NAK:
                        AddLog("Recieve NAK, resend frame");
                        SendFilePartBinary();
                        break;
                    case Commands.ACK:
                        AddLog("Recieve ACK, send next frame");
                        sending.packageNo++;
                        SendFilePartBinary();
                        break;
                }

                return;
            }
            string recieveData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            CheckFileCommand(recieveData);
            AddLog(string.Format("{0}: {1}", e.IpPort, recieveData));
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
            lblFileInfo.Text = string.Format("{0} byte/{1} pacakge\t\t Last:{2}", fi.Length, (int)Math.Ceiling((double)fi.Length / Commands.PackageSize), fi.LastWriteTime);
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            SendData2Client(txtMessage.Text);
        }

        void CheckFileCommand(string recieveCmd)
        {
            this.UIThread(() => {

                if (sending.isSending)
                {
                    SendFilePartBinary();
                    return;
                }

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
                    StartSendFile(txtFilePath.Text);
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

        void StartSendFile(string path)
        {
            sending.raw= File.ReadAllBytes(path);
            sending.maxPackage = (int)Math.Ceiling((double)sending.raw.Length / Commands.PackageSize);
            sending.packageNo = 0;
            sending.isSending = true;
            SendFilePartBinary();
        }
        void SendFilePartBinary()
        {
            if (server == null) return;
            if (ClientIpPort == "")
            {
                sending.isSending = false;
                return;
            }
            if(sending.packageNo < sending.maxPackage)
            {
                byte[] cmd = Commands.CreateCommand(ref sending.raw, sending.packageNo);
                server.Send(ClientIpPort, cmd);
                AddLog(string.Format("Frame {0}/{1} sended", sending.packageNo+1, sending.maxPackage));
            }
            else
            {
                byte[] endcmd = Commands.CreateEndCommand(ref sending.raw);
                server.Send(ClientIpPort, endcmd);
                AddLog("File's all frame sended");
                sending.isSending = false;
            }
        }
    }
}

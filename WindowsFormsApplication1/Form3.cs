using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperSimpleTcp;

namespace TCPServerResponseFile
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        static string responseSTR = @"~x#0043404{0}
0.0.0(EM72000656621)
0.9.1(172751)
0.9.2(100209)
1.8.0(343642.9*kWh)
2.8.0(1958.9*kWh)
3.8.0(120.7*kvarh)
4.8.0(7068.5*kvarh)
1.6.0(18014*kW)(10-02-01 00:15)
2.6.0(0*kW)(10-02-01 00:15)
3.6.0(186*kvar)(10-02-01 00:15)
4.6.0(366*kvar)(10-02-01 00:15)
1.2.0(275663*kW)
2.2.0(2158*kW)
3.2.0(54*kvar)
4.2.0(5683*kvar)
1.8.1(150967.3*kWh)
1.8.2(104673.3*kWh)
1.8.3(88002.3*kWh)
2.8.1(1958.9*kWh)
$";

       volatile int ConnectionCount = 0;
        volatile int DisConnCount = 0;
        volatile int RequestCount = 0;
        volatile int ResponseCount = 0;
        volatile int FailCount = 0;
        Timer timer;
        private void Form3_Load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 10;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.UIThread(() =>
            {
                label4.Text = string.Format("ConnectionCount: {0}\r\nRequestCount: {1}\r\nResponseCount: {2}\r\nFailCount: {3}- DisConnCount: {4}\r\n", 
                        ConnectionCount,
                        RequestCount,
                        ResponseCount,
                        FailCount, 
                        DisConnCount
                        );
            });
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            int startport = Convert.ToInt32(txtPortStart.Text);
            int endport = Convert.ToInt32(txtPortEnd.Text);

            for (int i = startport; i <= endport; i++)
            {
                SimpleTcpServer server;
                server = new SimpleTcpServer(txtIP.Text, i);   // "127.0.0.1:9000"

                // set events
                server.Events.ClientConnected += Events_ClientConnected;
                server.Events.ClientDisconnected += Events_ClientDisconnected;
                server.Events.DataReceived += Events_DataReceived;

                // let's go!
                server.Start();
            }
            AddLog("Server start listening...");
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            //#02 000000000100123654051$
            //#02 000000000100123654051$
            String recieveData = Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count);
            //AddLog(string.Format("{0}{1}: {2}", e.IpPort, ((SimpleTcpServer)sender).Port, recieveData));
            if (recieveData.EndsWith("$"))
            {
                string reqesutType = recieveData.Substring(1, 2);
                string responsePart = recieveData.Substring(3, recieveData.Length - 4);
                string responseData = string.Format(responseSTR, responsePart);
                if (reqesutType == "17") responseData = responseData;   //obis read;
                if (reqesutType == "08") responseData += responseData + responseData;
                RequestCount++;
                //AddLog(string.Format("{0}:{1} responsed", e.IpPort, ((SimpleTcpServer)sender).Port));
                SendData2Client((SimpleTcpServer)sender, e.IpPort, responseData, reqesutType);
            }
        }

        private void Events_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            DisConnCount++;
            //AddLog(string.Format("{0}:{2} client disconnected : {1}", e.IpPort, e.Reason, ((SimpleTcpServer)sender).Port));
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e)
        {
            ConnectionCount++;
            //ClientIpPort = e.IpPort;
            //AddLog(string.Format("{0}:{1} client connected", e.IpPort, ((SimpleTcpServer)sender).Port));
        }
        void SendData2Client(SimpleTcpServer server, string ClientIpPort, string txt, string reqType)
        {
            try
            {
                //Add random lag;
                Random rnd = new Random();
                if (reqType=="08")
                {
                    System.Threading.Thread.Sleep(rnd.Next(10000, 15000));
                }
                else
                {
                    System.Threading.Thread.Sleep(rnd.Next(2000, 4000));
                }
                if (server == null) return;
                if (ClientIpPort == "") return;
                server.Send(ClientIpPort, txt);
                ResponseCount++;
            }
            catch (Exception e)
            {
                AddLog(e.Message);
                FailCount++;
            }
        }
        void AddLog(string txt)
        {
            this.UIThread(() =>
            {
                txtLogs.Text = txt + Environment.NewLine + txtLogs.Text;
                if (txtLogs.Text.Length > 10000) txtLogs.Text = "";
            });

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtLogs.Text = "";
            ConnectionCount = 0;
            ResponseCount = 0;
            FailCount = 0;
            DisConnCount = 0;
            RequestCount = 0;
        }
    }
}

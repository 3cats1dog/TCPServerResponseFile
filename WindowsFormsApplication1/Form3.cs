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
            AddLog(string.Format("{0}: {1}", e.IpPort, recieveData));
            if (recieveData.EndsWith("$"))
            {
                string responsePart = recieveData.Substring(3, recieveData.Length - 4);
                string responseData = string.Format(responseSTR, responsePart);
                AddLog(string.Format("{0} responsed", e.IpPort));
                SendData2Client((SimpleTcpServer)sender, e.IpPort, responseData);
            }
        }

        private void Events_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            AddLog(string.Format("{0} client disconnected : {1}", e.IpPort, e.Reason));
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e)
        {
            //ClientIpPort = e.IpPort;
            AddLog(string.Format("{0} client connected", e.IpPort));
        }
        void SendData2Client(SimpleTcpServer server, string ClientIpPort, string txt)
        {
            //Add random lag;
            Random rnd = new Random();
            System.Threading.Thread.Sleep(rnd.Next(100, 200));
            if (server == null) return;
            if (ClientIpPort == "") return;
            server.Send(ClientIpPort, txt);

        }
        void AddLog(string txt)
        {
            this.UIThread(() =>
            {
                txtLogs.Text = txt + Environment.NewLine + txtLogs.Text;
                if (txtLogs.Text.Length > 10000) txtLogs.Text = "";
            });

        }
    }
}

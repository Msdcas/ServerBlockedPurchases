using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerBlockedPurchases.Classes;

namespace ServerBlockedPurchases
{
    public partial class Form1 : Form
    {
        private Form _MyForm;
        private Thread ServerThread = null;
        private SocketServer _socketServer = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TableHandler.SetDataGridView(ref mainGrid);
            Log.SetLogsRichTb(richTextBoxLog);
            _MyForm = this;

            try
            {
                comboBoxIpList.Items.AddRange(GetAllIpAddresses().ToArray());
                if (comboBoxIpList.Items.Count > 0)
                    comboBoxIpList.SelectedIndex = 0;
            }
            catch (Exception ex){ Log.Msg(ex.Message); }
        }

        private List<string> GetAllIpAddresses()
        {
            List<string> ipAddresses = new List<string>();
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties ipProps = netInterface.GetIPProperties();
                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                {
                    ipAddresses.Add(netInterface.Name + " → " + addr.Address.ToString());
                }
            }
            return ipAddresses;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            mainGrid.Height = _MyForm.Height - 184;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_socketServer != null)
                _socketServer.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ServerThread == null)
            {
                _socketServer = new SocketServer();

                if (!_socketServer.SetIpEndPoint(comboBoxIpList.Text.Split('→')[1].TrimStart(' ')))
                {
                    Log.Msg("Недопустимый IP адрес");
                    return;
                }
                ServerThread = new Thread(new ThreadStart(_socketServer.Listen));
                ServerThread.Start();
                if (ServerThread.ThreadState == ThreadState.Running)
                    Log.Msg("Сервер запущен");
                else
                    Log.Msg(ServerThread.ThreadState.ToString());
                button1.Text = "S T O P";
            }
            else
            {
                _socketServer.Stop();
                ServerThread = null;
                button1.Text = "S T A R T";
                Log.Msg("Сервер остановлен");
            }
        }




    }
}

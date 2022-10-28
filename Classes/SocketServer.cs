using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ServerBlockedPurchases.Classes.SocketClient;
using System.Runtime.InteropServices;

namespace ServerBlockedPurchases.Classes
{
    public class SocketServer
    {
        private const int _Port = 9690;
        private bool _IsWork = false;
        private bool _IsContinue = true;
        private Socket _SListener;
        private IPAddress _IpAddress;
        private List<SocketClient> _Clients = new List<SocketClient>();

        public void Listen()
        {
            _IsWork = true;
            _SListener = null;
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(_IpAddress, _Port);
                _SListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  //new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _SListener.Bind(ipPoint);
                _SListener.Listen(10);

                while (true)
                {
                    Socket handler = _SListener.Accept();

                    if (!_IsContinue) break;

                    SocketClient socketClient = new SocketClient(handler, this);
                    lock (_Clients)
                        _Clients.Add(socketClient);

                    Thread clientThread = new Thread(new ThreadStart(socketClient.Listen));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Log.Msg("Сервер остановлен из за ошибки: " + ex.Message);
            }
            finally
            {
                //_SListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                DisconnectAll();
                _SListener.Close();
                _IsWork = false;
            }
        }
        private void DisconnectAll()
        {
            for (int i = 0; i < _Clients.Count; i++)
            {
                _Clients[i].Stop();
            }
        }

        protected internal void BroadcastMessage(string message, SocketClient clientObj)
        {
            for (int i = 0; i < _Clients.Count; i++)
            {
                if (_Clients[i].IdDepartment == clientObj.IdDepartment && _Clients[i] != clientObj)
                {
                    _Clients[i].SendMessage(message);
                }
            }
        }

        protected internal void ClientLogout(SocketClient clientObj)
        {
            lock (_Clients)
                _Clients.Remove(clientObj);
        }

        public void Stop()
        {
            if (_IsWork)
            {
                _IsContinue = false;
                EmulateNewConnection();
                while (_IsWork) { };
            }
        }

        public bool SetIpEndPoint(string IpAddress)
        {
            if (IPAddress.TryParse(IpAddress, out _IpAddress))
                return true;
            return false;
        }



        /// <summary>
        /// Emulate a new connection to exit the connection wait procedure
        /// </summary>
        private void EmulateNewConnection()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try { socket.ConnectAsync(_IpAddress, _Port); }
            catch (Exception ex) { Log.Msg(ex.Message); }
        }



    }
}

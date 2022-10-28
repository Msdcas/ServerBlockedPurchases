using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerBlockedPurchases.Classes;

namespace ServerBlockedPurchases
{
    
    public class TcpServer
    {
        const int _Port = 9690;
        private static TcpListener _TcpListener;
        public TcpClientObj ClientObject;
        protected internal List<TcpClientObj> Clients { get; private set; } = new List<TcpClientObj>();


        public async void Listen()
        {
            try
            {
                _TcpListener = new TcpListener(IPAddress.Any, _Port);
                _TcpListener.Start(20);
                
                while (true)
                {
                    //TcpClient tcpClient = _TcpListener.AcceptTcpClient();
                    TcpClient tcpClient = await _TcpListener.AcceptTcpClientAsync().ConfigureAwait(false);//non blocking waiting 

                    TcpClientObj clientObject = new TcpClientObj(tcpClient, this);
                    lock(Clients)
                        Clients.Add(clientObject);

                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
                

            }
            catch (SocketException ex) when (ex.ErrorCode == 10004)
            {
                Log.Msg("Работа завершена");
                return;
            }
            catch (Exception ex)
            {
                Log.Msg("Сервер остановлен из за ошибки: " + ex.Message);
                DisconnectAll();
                return;
            }
        }

        protected internal void BroadcastMessage(string message, TcpClientObj clientObj)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].IdDepartment == clientObj.IdDepartment && Clients[i] != clientObj)
                {
                    Clients[i].SendData(message);
                }
            }
        }

        protected internal void ClientDisconnection(TcpClientObj clientObj)
        {
            //TcpClientObj client = _Clients.FirstOrDefault(c => c == clientObj);
            lock(Clients)
                Clients.Remove(clientObj);
        }

        public void DisconnectAll()
        {
            _TcpListener.Stop();
            for (int i = 0; i < Clients.Count; i++)
            {
                Clients[i].Close();
                ClientDisconnection(Clients[i]);
            }
        }

    }


}

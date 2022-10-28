using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerBlockedPurchases.Classes
{
    public class TcpClientObj
    {
        protected internal NetworkStream _Stream { get; private set; }
        private TcpClient _TcpClient;
        private TcpServer _TcpServer;
        private bool _ContinueListen = true;
        private User User;

        public string ErrMsg { get; private set; }
        public string IdDepartment { get; private set; }

        public TcpClientObj(TcpClient tcpClient, TcpServer serverObject)
        {
            _TcpClient = tcpClient;
            _TcpServer = serverObject;
            _Stream = tcpClient.GetStream();
        }

        public void Process()
        {
            string message;
            try
            {
                while (_ContinueListen)
                {
                    message = ReadData();
                    if (message != null)
                    {
                        RecognizeInstructionInMessageAndCallMethod(message);
                        //_TcpClient.Client.Shutdown(SocketShutdown.Send);
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception e)
            {
                Log.Msg(e.Message);
            }
            finally
            {
                Close();
                _TcpServer.ClientDisconnection(this);
            }
        }

        private void RecognizeInstructionInMessageAndCallMethod(String message)
        {
            List<string> Instruction = new List<string>();
            try
            {
                Instruction = message.Split(',').ToList();
            }
            catch (Exception ex){ Log.Msg(ex.Message); return; }

            if (Instruction.Count <1) { return; }

            switch (Instruction[0]) //type instruction
            {
                case "0": //table double click || add || del || change purchases
                    User.SetIndexEditedSQLRow(Instruction[1]);
                    _TcpServer.BroadcastMessage(String.Format("0,{0},{1}", Instruction[1], User.FullName), this);
                    break;
                case "1"://user log in/out
                    if (string.IsNullOrEmpty(Instruction[1]) && string.IsNullOrEmpty(Instruction[2]))
                        _ContinueListen = false;
                    else
                    {
                        User = new User(Instruction[1], Instruction[2], ref _TcpClient);
                        IdDepartment = Instruction[2];
                    }
                    break;
                case "2": //form closed
                    _ContinueListen = false;
                    TableHandler.DelRowByUserName(User);
                    break;
            }
        }
        public void Close()
        {
            _ContinueListen = false;
            if (_Stream != null)
                _Stream.Close();
            if (_TcpClient != null)
                _TcpClient.Close();
        }
        public void SendData(object data)
        {
            if (!IsConnected())
            {
                _ContinueListen = false;
                return;
            }

            byte[] bytedata;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                try { bf.Serialize(ms, data); }
                catch (IOException e) { ErrMsg = e.Message; }
                bytedata = ms.ToArray();
            }

            try
            {
                if (_Stream.CanWrite)
                    lock (_Stream)
                        _Stream.Write(bytedata, 0, bytedata.Length);
            }
            catch (IOException e)
            {
                ErrMsg = e.Message;
            }
            return;
        }

        private bool IsConnected()
        {
            try
            {
                if (_TcpClient != null && _TcpClient.Client != null && _TcpClient.Client.Connected)
                {
                    // Detect if client disconnected
                    if (_TcpClient.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (_TcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            // _TcpClient disconnected
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        private string ReadData()
        {
            if (!IsConnected())
            {
                //_ContinueListen = false;
                return null;
            }

            byte[] stdata = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            if (_Stream.CanRead)
                lock(_Stream)
                    while (_Stream.DataAvailable)
                    {
                        bytes = _Stream.Read(stdata, 0, stdata.Length);
                        builder.Append(Encoding.Unicode.GetString(stdata, 0, bytes));
                    }

            if (bytes == 0) return null;

            object data = null;
            using (MemoryStream ms = new MemoryStream(stdata, 0, stdata.Length))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try { data = bf.Deserialize(ms); }
                catch { }
            }
            return data.ToString();
        }
        
    }
  
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.Drawing;

namespace ServerBlockedPurchases.Classes
{
    public class SocketClient
    {
        private readonly Socket _Socket;
        private bool _ContinueListen = true;
        private bool _IsBreak = false;
        private SocketServer socketServer;
        private User User = null;
        public string IdDepartment { get; private set; }
        public string ErrMsg { get; private set; }
        

        public SocketClient(Socket socket, SocketServer socketServer)
        {
            _Socket = socket;
            this.socketServer = socketServer;
        }

        public void Listen()
        {
            string message = null;
            try
            {
                while (_ContinueListen)
                {
                    //lock(_Socket)
                        message = Deserialize(ReadBytes(128));

                    if (message != null)
                    {
                        Log.Msg(message);
                        RecognizeInstructionInMessageAndCallMethod(message);
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
                _Socket.Close();
                socketServer.ClientLogout(this);
                TableHandler.DelRowByUserName(User);
                _IsBreak = true;
            }
        }

        private void RecognizeInstructionInMessageAndCallMethod(String message)
        {
            List<string> Instruction = new List<string>();
            try
            {
                Instruction = message.Split(',').ToList();


                if (Instruction.Count < 2) { return; }

                switch (Instruction[0]) //type instruction
                {
                    case "0": //table double click || add || del || change purchases
                        User.SetIndexEditedSQLRow(Instruction[1]);
                        socketServer.BroadcastMessage(String.Format("0,{0},{1}", Instruction[1], User.FullName), this);
                        break;

                    case "1": //user log in/out
                        if (string.IsNullOrEmpty(Instruction[1]) && string.IsNullOrEmpty(Instruction[2]))
                            _ContinueListen = false;
                        else
                        {
                            if (User != null)
                                TableHandler.DelRowByUserName(User);
                            User = new User(Instruction[1], Instruction[2], _Socket);
                            IdDepartment = Instruction[2];
                        }
                        break;

                    case "2": //form closed
                        _ContinueListen = false;
                        break;
                }
            }
            catch (Exception ex) 
            { 
                Log.Msg(ex.Message);
                return; 
            }
        }
        
        public void Stop()
        {
            _ContinueListen = false;
            //_Socket.Disconnect(true);
            _Socket.Close();
            while (! _IsBreak) { };
        }
        public void SendMessage(string msg)
        {
            try
            {
                Log.Msg("To " + User.FullName + "msg = " + msg);
                _Socket.Send(Serialize(msg));
            }
            catch { }
        }

        private byte[] ReadBytes(int size)
        {
            if (_Socket.Available == 1)
                return null;

            var bytes = new byte[size];
            var total = 0;
            do
            {
                var read = _Socket.Receive(bytes, total, size - total, SocketFlags.None);
                Debug.WriteLine("Client recieved {0} bytes", total);
                if (read == 0)
                {
                    //If it gets here and you received 0 bytes it means that the Socket has
                    //Disconnected gracefully (without throwing exception) so you will need to handle that here
                    return bytes;
                }
                total += read;
            } while (total == 0);  //total != size
            return bytes;
        }

        private bool IsConnected()
        {
            try
            {
                bool part1 = _Socket.Poll(1000, SelectMode.SelectRead);
                bool part2 = (_Socket.Available == 0);

                if ((part1 && part2) || !_Socket.Connected)
                    return false;
                else
                    return true;
            
            }
            catch { return false; }
        }

        private byte[] Serialize(string msg)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                try { bf.Serialize(ms, msg); }
                catch (IOException e) { ErrMsg = e.Message; }
                return ms.ToArray();
            }
        }

        private string Deserialize(byte[] bytes)
        {
            object result = null;
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try { result = bf.Deserialize(ms); }
                catch { }
            }
            return result?.ToString();
        }



        /**
        public byte[] Receive(int offset, int size, int timeout)
        {
            byte[] buffer = null;
            int startTickCount = Environment.TickCount;
            int received = 0;
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                    return null;//throw new Exception("Timeout.");
                try
                {
                    received += _Socket.Receive(buffer, offset + received, size - received, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably empty, wait and try again
                        Thread.Sleep(30);
                    }
                    else
                        throw ex;  // any serious error occurr
                }
            } while (received < size);
            return buffer;
        }



        public static void Send(Socket socket, byte[] buffer, int offset, int size, int timeout)
        {
            int startTickCount = Environment.TickCount;
            int sent = 0;  // how many bytes is already sent
            do
            {
                if (Environment.TickCount > startTickCount + timeout)
                    throw new Exception("Timeout.");
                try
                {
                    sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably full, wait and try again
                        Thread.Sleep(30);
                    }
                    else
                        throw ex;  // any serious error occurr
                }
            } while (sent < size);
        }

        **/
    }

}

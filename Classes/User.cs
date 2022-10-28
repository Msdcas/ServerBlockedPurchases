using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerBlockedPurchases.Classes
{
    internal class User
    {

        public string FullName;
        public string IdDepart;
        public string IndexEditedSqlRow;
        public string IpAddress;
        public DateTime dateTime;

        public User(string fullName, string idDepart, Socket socket)
        {
            FullName = fullName;
            IdDepart = idDepart;

            IPEndPoint ipep = (IPEndPoint)socket.RemoteEndPoint;
            IpAddress = ipep.Address.ToString();
            IndexEditedSqlRow = null;
            dateTime = DateTime.Now; //time of login

            TableHandler.SetIndexEditedRowByUserName(this);
        }

        public User(string fullName, string idDepart, ref TcpClient tcpClient)
        {
            FullName = fullName;
            IdDepart = idDepart;

            IPEndPoint ipep = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            IpAddress = ipep.Address.ToString();

            dateTime = DateTime.Now; //time of login
            IndexEditedSqlRow = null;

            TableHandler.SetIndexEditedRowByUserName(this);
        }

        public void SetIndexEditedSQLRow(string SQLRowId)
        {
            IndexEditedSqlRow = SQLRowId;
            dateTime = DateTime.Now;
            TableHandler.SetIndexEditedRowByUserName(this);
        }






    }
}

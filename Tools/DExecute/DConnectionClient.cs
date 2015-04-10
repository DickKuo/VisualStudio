using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace DExecute
{
    public class DConnectionClient
    {
        private string _ip;
        private int _port;
        public string IP { get { return _ip; } }
        public int  Port { get { return _port; } }

        Dictionary<string, string> Paramenters = new Dictionary<string, string>();
        public DConnectionClient(Dictionary<string, string>pParameter)
        {
            Paramenters = pParameter;
            if (Paramenters.ContainsKey("IP"))
            {
             _ip= Paramenters["IP"];
            }
            if (Paramenters.ContainsKey("Port"))
            {
                _port =Convert.ToInt32( Paramenters["Port"]);
            }
        }

        NetworkStream myNetworkStream;
        TcpClient myTcpClient = new TcpClient();

        public string ConnectioinTest()
        {
            try
            {
                myTcpClient.Connect(IP, Port);
                return "連線成功";
            }
            catch(Exception  ex)
            {
                return ex.Message;
            }
        }

        public void Command(string CommandString)
        {           
            //將字串轉 byte 陣列，使用 ASCII 編碼
            Byte[] myBytes = Encoding.ASCII.GetBytes(CommandString);
            Console.WriteLine("建立網路資料流 !!");
            //建立網路資料流
            myNetworkStream = myTcpClient.GetStream();
            Console.WriteLine("將字串寫入資料流　!!");
            //將字串寫入資料流
            string s = Dns.GetHostName();
            IPAddress[] IPS = Dns.GetHostEntry(s).AddressList;
            if (IPS.Length > 3)
            {
                //string serr = "CommandString";
                myBytes = System.Text.Encoding.Default.GetBytes(CommandString);
                myNetworkStream.Write(myBytes, 0, myBytes.Length);
            }
            else
            {
                myBytes = System.Text.Encoding.Default.GetBytes("無法擷取IP");
                myNetworkStream.Write(myBytes, 0, myBytes.Length);
            }
        }

    }
}

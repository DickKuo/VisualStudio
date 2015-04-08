using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

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
    }
}

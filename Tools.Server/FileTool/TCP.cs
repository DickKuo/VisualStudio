using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CommTool
{
    public class TCP
    { 
        //宣告網路資料流變數
        NetworkStream myNetworkStream;

        //宣告 Tcp 用戶端物件
        TcpClient myTcpClient = new TcpClient();

        public static string CurrentDbConnection { set; get; }

        private string _ip;

        public string IP { get { return _ip; } }

        public void CommandToService(string pIP, string pPort, string pCommand) {
            try {
                TCP myNetworkClient = new TCP();
                _ip = pIP;
                myNetworkClient.myTcpClient.Connect(pIP, Convert.ToInt32(pPort));
                myNetworkClient.WriteData(pCommand);
                myNetworkClient.ReadData();
            }
            catch (Exception ex) {
            }
        }

        /// <summary>寫入資料</summary>
        /// <param name="pText"></param>
        private void WriteData(string pText) {
            Byte[] myBytes = System.Text.Encoding.Default.GetBytes(pText);        
            myNetworkStream = myTcpClient.GetStream();           
            myNetworkStream.Write(myBytes, 0, myBytes.Length);        
        }

        /// <summary>讀取資料</summary>
        private void ReadData() {
            //Console.WriteLine("從網路資料流讀取資料 !!");
            //從網路資料流讀取資料
            int bufferSize = myTcpClient.ReceiveBufferSize;
            byte[] myBufferBytes = new byte[bufferSize];
            myNetworkStream.Read(myBufferBytes, 0, bufferSize);
            //取得資料並且解碼文字
            CurrentDbConnection = Encoding.ASCII.GetString(myBufferBytes, 0, bufferSize).Replace("\0", string.Empty);
        }

        /// <summary>取得伺服器IP</summary>
        /// <returns></returns>
        public static string GetServerIP() {
            string ReturnIP = BaseConst.LocalHostIP;
            string HostName = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry IPHostInf = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (System.Net.IPAddress IPAddress in IPHostInf.AddressList) {
                if(IPAddress.AddressFamily ==System.Net.Sockets.AddressFamily.InterNetwork){
                    ReturnIP = IPAddress.ToString();
                    break;
                }
            }
            return ReturnIP;
        }        
    }
}

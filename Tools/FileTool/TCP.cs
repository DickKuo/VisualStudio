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

        public void CommandToService(string pIP ,string pPort,string pCommand)
        {
            try
            {
                TCP myNetworkClient = new TCP();
                //myNetworkClient.myTcpClient ;

                myNetworkClient.myTcpClient.Connect(pIP, Convert.ToInt32(pPort));
                myNetworkClient.WriteData(pCommand);
                myNetworkClient.ReadData();
                //CurrentDbConnection = myNetworkClient.ReadData();

                //MessageBox.Show("連線成功 !!");
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }
        //寫入資料
        void WriteData(string pText)
        {
            //String strTest = "this is a test string !!";
            //將字串轉 byte 陣列，使用 ASCII 編碼
            //Byte[] myBytes = Encoding.ASCII.GetBytes(pText);
            Byte[] myBytes = System.Text.Encoding.Default.GetBytes(pText);
            //Console.WriteLine("建立網路資料流 !!");
            //建立網路資料流
            myNetworkStream = myTcpClient.GetStream();
            //Console.WriteLine("將字串寫入資料流　!!");
            //將字串寫入資料流
            string s = Dns.GetHostName();
            IPAddress[] IPS = Dns.GetHostEntry(s).AddressList;
            if (IPS.Length > 3)
            {
                myBytes = System.Text.Encoding.Default.GetBytes(pText);
                myNetworkStream.Write(myBytes, 0, myBytes.Length);
            }
            else
            {
                myBytes = System.Text.Encoding.Default.GetBytes("無法擷取IP");
                myNetworkStream.Write(myBytes, 0, myBytes.Length);
            }
        }

        //讀取資料
        void ReadData()
        {
            //Console.WriteLine("從網路資料流讀取資料 !!");
            //從網路資料流讀取資料
            int bufferSize = myTcpClient.ReceiveBufferSize;
            byte[] myBufferBytes = new byte[bufferSize];
            myNetworkStream.Read(myBufferBytes, 0, bufferSize);
            //取得資料並且解碼文字
            CurrentDbConnection = Encoding.ASCII.GetString(myBufferBytes, 0, bufferSize).Replace("\0", string.Empty);

        }
    }
}

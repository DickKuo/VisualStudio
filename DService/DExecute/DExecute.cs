using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace DExecute
{
    public class DExecute
    {
        private string _ip;
        private string _port;
        public string IP { get { return _ip; } }
        public string Port { get { return _port; } }
        Dictionary<string, string> DicParameters = new Dictionary<string, string>();

        public DExecute(Dictionary<string, string> Parameters)
        {
            InitParamter(Parameters);
            DicParameters = Parameters;
        }

        public virtual void Start()
        {
            Lessner();
        }

        private void InitParamter(Dictionary<string,string>Parameters)
        {
            if (Parameters.ContainsKey("LogPath"))
            {
                CommTool.ToolLog.ToolPath = Parameters["LogPath"];
            }
            else
            {
                CommTool.ToolLog.ToolPath = @"C:\SLog";
            }
            if (Parameters.ContainsKey("IP"))
            {
                _ip = Parameters["IP"];
            }
            if (Parameters.ContainsKey("Port"))
            {
                _port = Parameters["Port"];
            }
        }

        private  void Lessner()
        {
            System.Net.IPAddress theIPAddress;
            //建立 IPAddress 物件(本機)                      
            theIPAddress = System.Net.IPAddress.Parse(IP);
            //Console.WriteLine(ConfigHelper.GetConfigValueByKey("IP").ToString());
            //Console.Read();
            //建立監聽物件
            TcpListener myTcpListener = new TcpListener(theIPAddress, Convert.ToInt32(Port));
            //啟動監聽
            //string CurrentDbSource = ConfigHelper.GetConfigValueByKey("CurrentDbSource").ToString();

            myTcpListener.Start(255);
            CommTool.ToolLog.Log("通訊埠 等待用戶端連線...... !!");
            do
            {
                Socket mySocket = myTcpListener.AcceptSocket();
                try
                {
                    //偵測是否有來自用戶端的連線要求，若是
                    //用戶端請求連線成功，就會秀出訊息。
                    if (mySocket.Connected)
                    {
                        //Task task = new Task(new Action());
                        string NetString=string.Empty;
                        int dataLength;
                        // Console.WriteLine("連線成功 !!");
                        byte[] myBufferBytes = new byte[1024];
                        //取得用戶端寫入的資料
                        dataLength = mySocket.Receive(myBufferBytes);                     
                        //CommTool.ToolLog.Log(string.Format("接收到的資料長度 {0} \n ", dataLength.ToString()));
                        CommTool.ToolLog.Log("取出用戶端寫入網路資料流的資料內容 :");
                        NetString =Encoding.ASCII.GetString(myBufferBytes, 0, dataLength);
                        CommTool.ToolLog.Log(NetString);
                        CommTool.ToolLog.Log("進行資料解析....");
                        DicParameters.Add("NetString", NetString);
                      


                        //Console.WriteLine("按下 [任意鍵] 將資料回傳至用戶端 !!");
                        //string str = Console.ReadLine();
                        // ConfigHelper.GetConfigValueByKey("TimeLimte").ToString();
                        //myBufferBytes = System.Text.Encoding.Default.GetBytes(CurrentDbSource + "," + ti);

                        //將接收到的資料回傳給用戶端
                       // mySocket.Send(myBufferBytes, myBufferBytes.Length, 0);

                    }
                }
                catch (Exception e)
                {
                    CommTool.ToolLog.Log(e.Message);
                    //Log(e);
                }
                finally
                {
                    mySocket.Close();
                }
            } while (true);
        }


       
    }
}

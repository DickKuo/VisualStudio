using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool;
using System.Net.Sockets;
using DExecute;
using System.Text.RegularExpressions;

namespace ServerApplication
{
    public class DExecuteX
    {
        private string _ip;
        private string _port;
        private string _logpath;
        private string _postaddress;
        public string IP { get { return _ip; } }
        public string Port { get { return _port; } }

        Dictionary<string, string> DicParameters = new Dictionary<string, string>();

        public DExecuteX(Dictionary<string, string> Parameters)
        {
            InitParamter(Parameters);
            DicParameters = Parameters;
        }

        public virtual void Start()
        {
            try
            {
                Lessner();
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }

        private void InitParamter(Dictionary<string, string> Parameters)
        {
            try
            {
                if (Parameters.ContainsKey("LogPath"))
                {
                    ToolLog.ToolPath = Parameters["LogPath"];
                }
                else
                {
                    ToolLog.ToolPath = @"C:\SLog";
                }
                _logpath = ToolLog.ToolPath;
                if (Parameters.ContainsKey("AppIP"))
                {
                    _ip = Parameters["AppIP"];
                }
                if (Parameters.ContainsKey("AppPort"))
                {
                    _port = Parameters["AppPort"];
                }
                if (Parameters.ContainsKey("PostAddress"))
                {
                    _postaddress = Parameters["PostAddress"];
                }
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
            }
        }

        private void Lessner()
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
            ToolLog.Log("通訊埠 等待用戶端連線...... !!");
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
                        string NetString = string.Empty;
                        int dataLength;
                        // Console.WriteLine("連線成功 !!");
                        byte[] myBufferBytes = new byte[1024];
                        //取得用戶端寫入的資料
                        dataLength = mySocket.Receive(myBufferBytes);
                        //CommTool.ToolLog.Log(string.Format("接收到的資料長度 {0} \n ", dataLength.ToString()));
                        ToolLog.Log("取出用戶端寫入網路資料流的資料內容 :");
                        //myBufferBytes = System.Text.Encoding.Default.GetBytes("解析語法中請稍後....");
                        mySocket.Send(myBufferBytes, myBufferBytes.Length, 0);
                        //NetString = Encoding.ASCII.GetString(myBufferBytes, 0, dataLength);
                        NetString = Encoding.ASCII.GetString(myBufferBytes, 0, dataLength);
                        DoAnalysis(NetString);
                        myBufferBytes = System.Text.Encoding.Default.GetBytes("解析完成。");
                        //Console.WriteLine("按下 [任意鍵] 將資料回傳至用戶端 !!");
                        //string str = Console.ReadLine();
                        // ConfigHelper.GetConfigValueByKey("TimeLimte").ToString();
                        myBufferBytes = System.Text.Encoding.Default.GetBytes("服務運行中，S 停止服務，R 重啟服務。");

                        //將接收到的資料回傳給用戶端
                        mySocket.Send(myBufferBytes, myBufferBytes.Length, 0);

                    }
                }
                catch (Exception e)
                {
                    ToolLog.Log(e.Message);
                }
                finally
                {
                    mySocket.Close();
                }
            } while (true);
        }

        private void DoAnalysis(string NetString)
        {
            WebInfo.WebInfo webinfo = new WebInfo.WebInfo(_logpath);
            ToolLog.Log(NetString);
            ToolLog.Log("進行資料解析....");
            DAnalysis analysis = new DAnalysis();
            DAnalysis.StructAnalysisResult result = analysis.Start(NetString);
            if (result.Type == DAnalysis.AnalysisType.E)
            {
                ToolLog.Log(result.Result);
                return;
            }
            if (result.Type == DAnalysis.AnalysisType.W)
            {
                ToolLog.Log("抓取網頁資料");
                webinfo.GetBueatyDirtory(result.Result, 0, _postaddress);
            }
            if (result.Type == DAnalysis.AnalysisType.R)
            {  //重啟服務
                ToolLog.Log("重啟服務");
                System.Diagnostics.Process.Start("net", "stop DService");
                System.Diagnostics.Process.Start("net", "start DService");
            }
            if (result.Type == DAnalysis.AnalysisType.S)
            {

            }
        }

    }


}

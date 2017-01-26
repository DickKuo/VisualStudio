using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using CommTool;
using WebInfo;
using WebInfo.Business.DataEntities;
using DService.Business.Entities;
using DService.Business.Services;
using DStandardServer;
using System.Runtime.Remoting.Channels;
using System.Reflection;

namespace ServerApplication
{
    class Program
    {
        private static  TriggerService server = null;
        private static  ConfigManager configmanage;
        static void Main(string[] args)
        {
            string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DService.exe.config");
            configmanage = new ConfigManager(configiPath, "DService");
            //Console.WriteLine("服務啟動");
            //Thread.Sleep(50000);
            //AssemblyLoader load = new AssemblyLoader();
            //CallService.CurrentServiceProvider = load;
            //load.Load();
            //Assembly ii =      Assembly.LoadFile(@"C:\Users\Dick\Desktop\Clinet\bin\Debug\WithoutRemote.dll");
            //ITestService Itest = CallService.GetService<ITestService>();
            //Itest.HelloWord();
            //System.Type[] types = ii.GetTypes();
            
            #region 20150609加入顯示字樣 #63
            Console.WriteLine(string.Format("ServerIP:{0}",configmanage.GetValue("AppIP")));
            Console.WriteLine(string.Format("ServerPort:{0}",configmanage.GetValue("AppPort")));
            #endregion

            #region 設定自動觸發
            //server = GetTriggerServices();
            //System.Timers.Timer t = new System.Timers.Timer(1000);
            //t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);   //到達時間的時候執行事件； 
            //t.AutoReset = true;//設置是執行一次（false）還是一直執行(true)； 
            //t.Enabled = true;//是否執行System.Timers.Timer.Elapsed事件； 
            #endregion
            
            #region  等候客戶端連線
            //Thread t1 = new Thread(Lessner);
            //t1.Start();
            #endregion               
    
            
            System.Timers.Timer StockTimer = new System.Timers.Timer(20*1000);
            StockTimer.Elapsed += StockTimer_Elapsed;
         
            StockTimer.Start();

            Console.Read();
        }

        static void StockTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {           
            Stock.StockData _StockData = new Stock.StockData();
            string StockUrl = "https://tw.screener.finance.yahoo.net/future/aa03?fumr=futurepart&opmr=optionpart&opcm=WTXO&opym=";
            string Result   = _StockData.GetOptionEveryDay(StockUrl);           
        }
 
   

        static void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            server.Run(DateTime.Now.ToString(configmanage.GetValue("ShortTimeFormat")));
        }


        private static void Lessner()
        {
            DExecute.DExecute execute = new DExecute.DExecute(configmanage.GetParamters());
            execute.Start();
        }


        private static List<string> _timelist = new List<string>();


        private static TriggerService GetTriggerServices()
        {
            ServerImplement service = new ServerImplement();
            return service.GetAutoTriggerService();
        }
    }
}

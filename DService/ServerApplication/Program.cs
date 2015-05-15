﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using CommTool;
using WebInfo;
using WebInfo.Business.DataEntities;
using DStandardServer;

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
            server = GetTriggerServices();
            System.Timers.Timer t = new System.Timers.Timer(1000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);   //到達時間的時候執行事件； 
            t.AutoReset = true;//設置是執行一次（false）還是一直執行(true)； 
            t.Enabled = true;//是否執行System.Timers.Timer.Elapsed事件； 

            //Lessner();
            //Thread t1 = new Thread(Lessner);
            //t1.Start();
       
            Console.Read();
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

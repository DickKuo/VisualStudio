using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WebInfo;
using System.Xml;
using System.IO;
using CommTool;
using WebInfo.Business.DataEntities;
using AutoTriggerService;

namespace ServerApplication
{
    class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("服務啟動");
            //Thread thread = new Thread(startthread);
            //thread.Start();
            //iInit();

            //DExecute.DAnalysis analysis = new DExecute.DAnalysis();
            // analysis.Start("aaaaaa");
            //GetPTTBueaty bueaty = new GetPTTBueaty();
            //bueaty.Execute("12:00");
            //
            TriggerService server = GetTriggerServices();
            server.Run(DateTime.Now.ToString());

            Console.Read();
        }
        private static List<string> _timelist = new List<string>();


        private static TriggerService GetTriggerServices()
        {
            CaseTriggerStandard service = new CaseTriggerStandard();
            return service.GetAutoTriggerService();
        }

        
        private static void iInit()
        {
            ToolLog.ToolPath = @"C:\SLog\";
            DateTime BaseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);           
            DateTime FlagTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, 23,59, 0);
         
            int interval = Convert.ToInt32(15);
            while (BaseTime <= FlagTime)
            {
                _timelist.Add(BaseTime.ToString("HH:mm:ss"));
                Console.WriteLine(BaseTime.ToString("HH:mm:ss"));
                BaseTime = BaseTime.AddSeconds(GetTime() * interval);

            }
        }
       static void startthread()
        {
            Dictionary<string, string> Parameter = new Dictionary<string, string>();
            Parameter.Add("AppIP", "10.40.30.104");
            Parameter.Add("AppPort", "9001");
            Parameter.Add("PostAddress", "http://dickguo.net63.net/chat/test/");
            DExecuteX exe = new DExecuteX(Parameter);
            exe.Start();
        }
       public static void DServerLog(string Message)
       {
           CommTool.ToolLog.Log(Message);
       }



       /// <summary>
       /// 20141219 add by Dick for 取得時間單位轉換
       /// </summary>
       /// <returns></returns>
       private static int GetTime()
       {
           int Result = 0;
           switch ("m")
           {
               case "s":
                   Result = 1;
                   break;
               case "m":
                   Result = 60;
                   break;
               case "h":
                   Result = 3600;
                   break;
               default:

                   break;
           }
           return Result;
       }

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("服務啟動");
            Thread thread = new Thread(startthread);
            thread.Start();
           

            DExecute.DAnalysis analysis = new DExecute.DAnalysis();
             analysis.Start("aaaaaa");
            
            Console.Read();
        }

       static void startthread()
        {
            Dictionary<string, string> Parameter = new Dictionary<string, string>();
            Parameter.Add("AppIP", "10.40.30.104");
            Parameter.Add("AppPort", "8051");
            Parameter.Add("PostAddress", "http://dickguo.net63.net/chat/test/");
            DExecuteX exe = new DExecuteX(Parameter);
            exe.Start();
        }
    }
}

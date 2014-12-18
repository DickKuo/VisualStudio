using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace DService
{
    public partial class Service1 : ServiceBase
    {
        private List<string> _timelist = new List<string>();
        public Service1()
        {
            InitializeComponent();
            FileTool.ToolLog.ToolPath = Settings1.Default.LogPath;
            DateTime BaseTime =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
            DateTime FlagTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(1).Day,0,0,0);
            int interval =Convert.ToInt32(Settings1.Default.Interval);
            while (BaseTime <= FlagTime)
            {
                _timelist.Add(BaseTime.ToString("HH:mm:ss"));
                BaseTime = BaseTime.AddSeconds(GetTime() * interval);
            }
        }
                

        protected override void OnStart(string[] args)
        {
            DServerLog("服務啟動");
            System.Timers.Timer time = new System.Timers.Timer();
            time.Elapsed += new System.Timers.ElapsedEventHandler(time_Elapsed);
            time.Interval = 1000;
            time.Start();
        }

        void time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            if (DateTime.Now.ToString("HH:mm") == Settings1.Default.UpDateTime)
            {
                UpDateDll();
            }

            if (_timelist.Contains(time))
            {
                DExecute.DExecute execute = new DExecute.DExecute();
                execute.Start();
            }
        }

        protected override void OnStop()
        {
            DServerLog("服務停止");            
        }

        /// <summary>
        /// 20141219 add by Dick for 取得時間單位轉換
        /// </summary>
        /// <returns></returns>
        private int GetTime()
        { 
            int Result =0;
            switch(Settings1.Default.IntervalUnit.ToLower())
            {
                case "s":
                    Result = 1;
                    break;
                case"m":
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

        /// <summary>
        /// 20141219 add by Dick 自動更新
        /// </summary>
        private void UpDateDll()
        {
            DServerLog("更新開始");

            DServerLog("更新結束");
        }

        public static void DServerLog(string Message)
        {
            FileTool.ToolLog.Log(string.Format("{0} {1} ", Message , DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
        }
    }
}

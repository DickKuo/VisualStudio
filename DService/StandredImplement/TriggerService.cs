using WebInfo.Business.DataEntities;
using DService.Business.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.IO;
using CommTool;
using WebInfo;
using System;
using Stock;

namespace StandredImplement
{
    #region 此為標準trigger 範例
    ///// <summary>
    ///// 此為標準trigger 範例
    ///// </summary>
    //public class SampleService : AutoTrigger
    //{
    //    BackgroundWorker _work;

    //    public SampleService()
    //    {
    //        _work = new BackgroundWorker();
    //        _work.DoWork += new DoWorkEventHandler(work_DoWork);
    //        _work.RunWorkerCompleted += new RunWorkerCompletedEventHandler(work_RunWorkerCompleted);
    //    }

    //    void work_DoWork(object sender, DoWorkEventArgs e)
    //    { 

    //    }

    //    void work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    //    {

    //    }

    //    /// <summary>
    //    /// 執行trigger 
    //    /// </summary>
    //    /// <param name="pCurrentTime"></param>
    //    public override void Execute(string pCurrentTime)
    //    {
    //try {
    //    if (!_work.IsBusy) {
    //        _work.RunWorkerAsync();
    //    }   
    //}
    //catch (Exception ex) {
    //    CommTool.ToolLog.Log(ex);
    //}
    //    }
    //}
    #endregion

    public class GetBueaty : AutoTrigger
    {
        private List<string> _timelist = new List<string>();
        private string _shortFormat;
        private object locker = new Object();

        private class Default
        {
            public const string LogPath = "LogPath";
            public const string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
            public const string DService = "DService";
            public const string Index = "/index";
            public const string DServiceConfig = "DService.exe.config";
            public const string StartPoint = "StartPoint";
            public const string StartTag = "StartTag";
            public const string FileName = "Record.xml";
            public const string ShortTimeFormat = "ShortTimeFormat";
            public const string Interval = "Interval";
            public const string IntervalUnit = "IntervalUnit";
            public const string TestPostAddress = "TestPostAddress";
            public const string PostAddress = "PostAddress";
            public const string html = ".html";
            public const string IsTest = "IsTest";
            public const string PushCount = "PushCount";
            public const string IsWorkBueaty = "IsWorkBueaty";
        }

        public GetBueaty()
        {
            DateTime BaseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime FlagTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0);
            string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Default.DServiceConfig);
            ConfigManager configmanage = new ConfigManager(configiPath, Default.DService);
            int interval = Convert.ToInt32(configmanage.GetValue(Default.Interval));
            ToolLog.ToolPath = configmanage.GetValue(Default.LogPath);
            _shortFormat = configmanage.GetValue(Default.ShortTimeFormat);
            while (BaseTime <= FlagTime)
            {
                _timelist.Add(BaseTime.ToString(_shortFormat));
                BaseTime = BaseTime.AddSeconds(GetTime(configmanage.GetValue(Default.IntervalUnit)) * interval);
            }
        }

        private void work_DoWork(string Time)
        {
            lock (locker)
            {
                if (_timelist.Contains(Time))
                {
                    string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Default.DServiceConfig);
                    ConfigManager configmanage = new ConfigManager(configiPath, Default.DService);
                    int currentTag = Convert.ToInt32(configmanage.GetValue(Default.StartTag));
                    if (currentTag <= Convert.ToInt32(configmanage.GetValue(Default.StartPoint)))
                    {
                        currentTag = Convert.ToInt32(configmanage.GetValue(Default.StartPoint));
                    }
                    try
                    {
                        ToolLog.Log("執行" + DateTime.Now.ToString(_shortFormat));
                        string LogPath = configmanage.GetValue(Default.LogPath);
                        GetSite site = new GetSite(LogPath);
                        string RecordXml = Path.Combine(LogPath, Default.FileName);
                        if (!File.Exists(RecordXml))
                        {
                            XmlFile xml = new XmlFile();
                            xml.CreateBaseXml(RecordXml, string.Empty, true);
                        }
                        XmlDocument doc = XmlFile.LoadXml(RecordXml);
                        XmlNode root = doc.SelectSingleNode(AutoTrigger.Default.Root);
                        site.PostAddress = Convert.ToBoolean(configmanage.GetValue(Default.IsTest)) == true ? configmanage.GetValue(Default.TestPostAddress) : site.PostAddress = configmanage.GetValue(Default.PostAddress);
                        site.PushCount = Convert.ToInt32(configmanage.GetValue(Default.PushCount));
                        site.Tag = currentTag;
                        string Site = configmanage.GetValue("Theme");
                        string ListAddress = "https://www.ptt.cc/bbs/" + Site + Default.Index + site.Tag + Default.html;
                        ToolLog.Log("取得表特列表" + DateTime.Now.ToString(Default.DateTimeFormat));
                        ToolLog.Log("列表網址:" + ListAddress);
                        SitePlus siteplus = site.GetUrlList(ListAddress);
                        List<SiteInfo> SiteInfoList = new List<SiteInfo>();
                        site.Recursive(ref currentTag, siteplus, SiteInfoList, Site, "/bbs/" + Site + Default.Index, configmanage.GetValue("Condition"), doc, root);
                    }
                    catch (Exception ex)
                    {
                        ToolLog.Log(ex.Message);
                    }
                    finally
                    {
                        configmanage.SetValue(Default.StartTag, (currentTag - 2).ToString());
                    }
                }
            }
        }

        /// <summary>執行trigger</summary>
        /// <param name="pCurrentTime"></param>
        public override void Execute(string pCurrentTime)
        {
            string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Default.DServiceConfig);
            ConfigManager configmanage = new ConfigManager(configiPath, Default.DService);
            bool IsWork = Convert.ToBoolean(configmanage.GetValue(Default.IsWorkBueaty));
            if (IsWork)
            {
                work_DoWork(pCurrentTime);
            }
        }

        /// <summary>20141219 add by Dick for 取得時間單位轉換</summary>
        /// <returns></returns>
        private static int GetTime(string ptype)
        {
            int Result = 0;
            switch (ptype.ToLower())
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
            }
            return Result;
        }
    }

    #region 20150610 抓取台灣銀行黃金存摺價位

    ///// <summary>
    ///// 
    ///// </summary>
    public class GetGoldTrigger : AutoTrigger
    {
        BackgroundWorker _work;

        public GetGoldTrigger()
        {
            _work = new BackgroundWorker();
            _work.DoWork += new DoWorkEventHandler(work_DoWork);
            _work.RunWorkerCompleted += new RunWorkerCompletedEventHandler(work_RunWorkerCompleted);
        }

        void work_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Gold gold = GetGold();
                if (gold != null)
                {
                    string record = Path.Combine(@"C:\GoldLog", "GoldRecord.xml");
                    if (!File.Exists(record))
                    {
                        XmlFile.CreateBaseXml(record, true);
                    }
                    else
                    {
                        XmlDocument doc = XmlFile.LoadXml(record);
                        XmlNode root = doc.SelectSingleNode(AutoTrigger.Default.Root);
                        XmlElement Xmlrecord = doc.CreateElement(AutoTrigger.Default.Record);
                        Xmlrecord.SetAttribute("Time", DateTime.Now.ToString(AutoTrigger.Default.DateTimeFormat));
                        Xmlrecord.SetAttribute("BankSell", gold.BankSell.ToString());
                        Xmlrecord.SetAttribute("BankBuy", gold.BankBuy.ToString());
                        root.AppendChild(Xmlrecord);
                        doc.Save(record);
                    }
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex.Message);
            }
        }

        /// <summary></summary>
        /// <returns></returns>
        private Gold GetGold()
        {
            WebInfo.WebInfo web = new WebInfo.WebInfo(@"C:\GoldLog");
            StreamReader sq = web.GetResponse("http://rate.bot.com.tw/Pages/Static/UIP005.zh-TW.htm");
            string line = string.Empty;
            line = sq.ReadToEnd();
            string[] array = line.Split(new string[] { "<table" }, StringSplitOptions.RemoveEmptyEntries);
            string[] trs = array[8].Split(new string[] { "<tr" }, StringSplitOptions.RemoveEmptyEntries);
            Gold gold = new Gold();
            string[] selltd = trs[3].Split(new string[] { "<td" }, StringSplitOptions.RemoveEmptyEntries);
            gold.BankSell = Convert.ToDecimal(selltd[selltd.Length - 1].Replace("</td></tr>", string.Empty).Replace("class=\"decimal\">", string.Empty).Trim());
            string[] Buytd = trs[4].Split(new string[] { "<td" }, StringSplitOptions.RemoveEmptyEntries);
            string[] temp = Buytd[Buytd.Length - 1].Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            gold.BankBuy = Convert.ToDecimal(temp[0].Replace("</td></tr>", string.Empty).Replace("class=\"decimal\">", string.Empty).Trim());
            return gold;
        }

        /// <summary></summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        /// <summary>執行trigger</summary>
        /// <param name="pCurrentTime"></param>
        public override void Execute(string pCurrentTime)
        {
            if (pCurrentTime == "16:00:00")
            {
                if (!_work.IsBusy)
                {
                    _work.RunWorkerAsync();
                }
            }
        }
    }
    #endregion

    #region 20170126 抓取選擇權資料
    public class OptionTrigger : AutoTrigger
    {
        BackgroundWorker _work;
        private class Default
        {
            public const string DServiceConfig = "DService.exe.config";
            public const string DService = "DService";
            public const string YahooStock = "YahooStock";
            public const string Capitalfutures = "Capitalfutures";
            public const string Channel = "Channel";
            public const string WeigthedUrl = "WeigthedUrl";
        }

        public OptionTrigger()
        {
            _work = new BackgroundWorker();
            _work.DoWork += new DoWorkEventHandler(work_DoWork);
            _work.RunWorkerCompleted += new RunWorkerCompletedEventHandler(work_RunWorkerCompleted);
        }

        /// <summary>執行</summary>
        /// 20170208 修改成一個Thread來執行到底 modified by Dick
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void work_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                lock (IsBusy)
                {
                    DateTime Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 44, 50);
                    DateTime End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 45, 30);
                   
                    string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Default.DServiceConfig);
                    ConfigManager configmanage = new ConfigManager(configiPath, Default.DService);
                    StockDAO StockContext = new StockDAO();

                    while (DateTime.Now.Subtract(Start).TotalSeconds >= 0 && DateTime.Now.Subtract(End).TotalSeconds <= 0)
                    {         
                        string WeigthedUrl = configmanage.GetValue(Default.WeigthedUrl);
                        string CapitalfuturesUrl = string.Empty;
                        int Channel = Convert.ToInt32(configmanage.GetValue(Default.Channel));

                        if (Channel == 1)
                        {
                            CapitalfuturesUrl = configmanage.GetValue(Default.YahooStock);
                        }

                        if (Channel == 2)
                        {
                            CapitalfuturesUrl = configmanage.GetValue(Default.Capitalfutures);
                        }

                        StockContext.GetOptionEveryDay(CapitalfuturesUrl, WeigthedUrl, Channel);
                        Thread.Sleep(1000 * 20);
                    }

                    if (DateTime.Now.Day == 1)
                    {
                        try
                        {
                            bool IsDo = false;
                            DateTime CalendarStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 30, 10);
                            DateTime CalendarEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 30, 50);
                            while (DateTime.Now.Subtract(CalendarStart).TotalSeconds >= 0 && DateTime.Now.Subtract(CalendarEnd).TotalSeconds <= 0)
                            {
                                if (!IsDo)
                                {
                                    IsDo = true;
                                    CalendarDAO CalendarContext = new CalendarDAO();
                                    CalendarContext.CreateNextMonthCalendar(DateTime.Now);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ToolLog.Log(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex.Message);
            }
        }

        /// <summary></summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private static object IsBusy = new object();

        /// <summary>執行trigger</summary>
        /// 20170202 修正這邊單純執行，將時間判斷交給Object    modified by Dick 
        /// <param name="pCurrentTime"></param>
        public override void Execute(string pCurrentTime)
        {
            try
            {
                if (!_work.IsBusy)
                {
                    _work.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }
    }
    #endregion

    /// <summary> 20170208 抓取摩托車資訊 </summary>
    public class MotorTrigger : AutoTrigger
    {
        BackgroundWorker _work;
        private static object IsBusy = new object();
        public MotorTrigger()
        {
            _work = new BackgroundWorker();
            _work.DoWork += new DoWorkEventHandler(work_DoWork);
            _work.RunWorkerCompleted += new RunWorkerCompletedEventHandler(work_RunWorkerCompleted);
        }

        private void work_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                lock (IsBusy)
                {
                    WebInfo.MotorData _MotorDB = new MotorData();
                    _MotorDB.GetMotorData();
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex.Message);
            }
        }

        private void work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        /// <summary>
        /// 執行trigger 
        /// </summary>
        /// <param name="pCurrentTime"></param>
        public override void Execute(string pCurrentTime)
        {
            try
            {
                if (!_work.IsBusy)
                {
                    _work.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }
    }

    #region  監控價格
    /// <summary>
    /// 此為標準trigger 範例
    /// </summary>
    public class ControlPriceService : AutoTrigger
    {
        BackgroundWorker _work;

        public ControlPriceService()
        {
            _work = new BackgroundWorker();
            _work.DoWork += new DoWorkEventHandler(work_DoWork);
            _work.RunWorkerCompleted += new RunWorkerCompletedEventHandler(work_RunWorkerCompleted);
        }

        private static object IsBusy = new object();

        private void work_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                lock (IsBusy)
                { 
                    DateTime Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 44, 50);
                    DateTime End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 45, 20);
                    while (DateTime.Now.Subtract(Start).TotalSeconds >= 0 && DateTime.Now.Subtract(End).TotalSeconds <= 0)
                    {
                        StockDAO StockContext = new StockDAO();
                        StockContext.ControlPrice();
                        Thread.Sleep(1000 * 20);
                    }

                    DateTime ReportStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 50, 0);
                    DateTime ReportEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 55, 0);
                    while (DateTime.Now.Subtract(ReportStart).TotalSeconds >= 0 && DateTime.Now.Subtract(ReportEnd).TotalSeconds <= 0)
                    {
                        TradeRecordDAO RecordDAO = new TradeRecordDAO();
                        RecordDAO.CalculateResultReport();
                        Thread.Sleep(1000 * 20);
                    }
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex.Message);
            }
        }

        private void work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        /// <summary>
        /// 執行trigger 
        /// </summary>
        /// <param name="pCurrentTime"></param>
        public override void Execute(string pCurrentTime)
        {
            try
            {
                if (!_work.IsBusy)
                {
                    _work.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }
    }
    #endregion        
}
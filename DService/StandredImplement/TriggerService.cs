using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool;
using System.ComponentModel;
using System.Xml;
using WebInfo;
using System.IO;
using WebInfo.Business.DataEntities;

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
    //        //throw new NotImplementedException();
    //    }
    //}
    #endregion

    public class GetBueaty : AutoTrigger
    {
        private  List<string> _timelist = new List<string>();
        private string _shortFormat;
        private object locker = new Object(); 

        public GetBueaty()
        {           
            DateTime BaseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime FlagTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0);
            string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DService.exe.config"); 
            ConfigManager configmanage = new ConfigManager(configiPath);
            int interval = Convert.ToInt32(configmanage.GetValue("Interval"));
            ToolLog.ToolPath = configmanage.GetValue("LogPath");
            _shortFormat = configmanage.GetValue("ShortTimeFormat");
            while (BaseTime <= FlagTime)
            {
                _timelist.Add(BaseTime.ToString(_shortFormat));
                ToolLog.Log(BaseTime.ToString(_shortFormat));
                BaseTime = BaseTime.AddSeconds(GetTime(configmanage.GetValue("IntervalUnit")) * interval);
            }           
        }


        private void work_DoWork(string Time)
        {
            lock (locker)
            {
                if (_timelist.Contains(Time))
                {
                    string configiPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DService.exe.config");
                    ConfigManager configmanage = new ConfigManager(configiPath);
                    int currentTag = Convert.ToInt32(configmanage.GetValue("StartTag"));
                    if (currentTag <= Convert.ToInt32(configmanage.GetValue("StartPoint")))
                    {
                        currentTag = Convert.ToInt32(configmanage.GetValue("StartPoint"));
                    }
                    try
                    {
                        ToolLog.Log("執行" + DateTime.Now.ToString(_shortFormat));
                        string LogPath = configmanage.GetValue("LogPath");
                        GetSite site = new GetSite(LogPath);
                        string RecordXml = Path.Combine(LogPath, "Record.xml");
                        if (!File.Exists(RecordXml))
                        {
                            XmlFile xml = new XmlFile();
                            xml.CreateBaseXml(RecordXml, string.Empty, true);
                        }
                        XmlDocument doc = XmlFile.LoadXml(RecordXml);
                        XmlNode root = doc.SelectSingleNode("root");
                        site.PostAddress = Convert.ToBoolean(configmanage.GetValue("IsTest")) == true ? configmanage.GetValue("TestPostAddress") : site.PostAddress = configmanage.GetValue("PostAddress");
                        site.PushCount = Convert.ToInt32(configmanage.GetValue("PushCount"));
                        site.Tag = currentTag;
                        string Site = configmanage.GetValue("Theme");
                        string ListAddress = "https://www.ptt.cc/bbs/" + Site + "/index" + site.Tag + ".html";
                        ToolLog.Log("取得表特列表" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        ToolLog.Log("列表網址:" + ListAddress);
                        SitePlus siteplus = site.GetUrlList(ListAddress);
                        List<SiteInfo> SiteInfoList = new List<SiteInfo>();
                        site.Recursive(ref currentTag, siteplus, SiteInfoList, Site, "/bbs/" + Site + "/index", configmanage.GetValue("Condition"), doc, root);
                    }
                    catch (Exception ex)
                    {
                        ToolLog.Log(ex.Message);
                    }
                    finally
                    {
                        configmanage.SetValue("StartTag", (currentTag - 2).ToString());
                    }
                }                
            }
        }


        /// <summary>
        /// 執行trigger 
        /// </summary>
        /// <param name="pCurrentTime"></param>
        public override void Execute(string pCurrentTime)
        {
            work_DoWork(pCurrentTime);
        }

        /// <summary>
        /// 20141219 add by Dick for 取得時間單位轉換
        /// </summary>
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
}

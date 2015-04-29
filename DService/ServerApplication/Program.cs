using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WebInfo;
using System.Xml;
using CommTool;
using System.IO;
namespace ServerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("服務啟動");
            Thread thread = new Thread(startthread);
            thread.Start();

            DServerLog("執行" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            string LogPath = @"C:\SLog";
            GetSite site = new GetSite(LogPath);
            string RecordXml = Path.Combine(LogPath, "Record.xml");
            if (!File.Exists(RecordXml))
            {
                XmlFile xml = new XmlFile();
                xml.CreateBaseXml(RecordXml, string.Empty, true);
            }
            XmlDocument doc = XmlFile.LoadXml(RecordXml);
            XmlNode root = doc.SelectSingleNode("root");
            site.PostAddress = Convert.ToBoolean("true") == true ? "http://dickguo.net63.net/chat/test/" : "";
            site.PushCount = 30;
            site.Tag = 1150;
            string Site = "Beauty";
            SitePlus siteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + "/index" + site.Tag + ".html");
            DServerLog("取得表特列表" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            List<SiteInfo> SiteInfoList = new List<SiteInfo>();
            site.Recursive(1150, siteplus, SiteInfoList, Site, "/bbs/" + Site + "/index", "[正妹]", doc, root);


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
       public static void DServerLog(string Message)
       {
           CommTool.ToolLog.Log(Message);
       }
    }
}

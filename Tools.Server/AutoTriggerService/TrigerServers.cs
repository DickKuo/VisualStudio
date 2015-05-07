using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommTool;
using WebInfo;
using System.IO;
using System.Xml;
using WebInfo.Business.DataEntities;

namespace AutoTriggerService
{
    public class TrigerSample : AutoTrigger
    {

        public override void Execute(string pCurrentTime)
        {
            throw new NotImplementedException();
        }
        
    }
    


    public class GetPTTBueaty : AutoTrigger
    {
        private static List<string> _timelist = new List<string>();

        /// <summary>
        /// 建立結構
        /// </summary>
        public GetPTTBueaty()
        {
            DateTime BaseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime FlagTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, 23, 59, 0);
            int interval = Convert.ToInt32(15);
            while (BaseTime <= FlagTime)
            {
                _timelist.Add(BaseTime.ToString("HH:mm:ss"));
                Console.WriteLine(BaseTime.ToString("HH:mm:ss"));
                BaseTime = BaseTime.AddSeconds(GetTime() * interval);
            }
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
                    Result = 60;
                    break;
            }
            return Result;
        }

        /// <summary>
        ///  執行抓網頁111
        /// </summary>
        /// <param name="pCurrentTime"></param>
        public override void Execute(string pCurrentTime)
        {
            this.Log("執行" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
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
            int currentTag = site.Tag;
            string Site = "Beauty";
            SitePlus siteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + "/index" + site.Tag + ".html");
            this.Log("取得表特列表" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            List<SiteInfo> SiteInfoList = new List<SiteInfo>();
            site.Recursive(ref currentTag, siteplus, SiteInfoList, Site, "/bbs/" + Site + "/index", "[正妹]", doc, root);
        }

    }
}

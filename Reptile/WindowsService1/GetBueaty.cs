using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using GetSite;
using System.Threading;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Xml;

namespace WindowsService1
{
    public partial class GetBueaty : ServiceBase
    {
        public GetBueaty()
        {
            InitializeComponent();
        }
        public int Tag { set; get; }
        public int PushCount { set; get; }
        public string Address { set; get; }
        public DateTime RecordTime { set; get; }

        public static List<string> listold = new List<string>();
        //public static List<string> listnew = new List<string>();


        /// <summary>
        /// 服務開始
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            Log("服務啟動" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            System.Timers.Timer time = new System.Timers.Timer();
            time.Elapsed += new System.Timers.ElapsedEventHandler(time_Elapsed);
            time.Interval = Settings1.Default.Interval;
            time.Start();
            Init();
            //Start();
        }

        /// <summary>
        /// 抓取XML檔內現有的紀錄
        /// </summary>
        private void Init()
        {
            XmlDocument doc = Tools.XmlTool.LoadXml(Settings1.Default.RecordPath + "\\" + "Record.xml");
            XmlNodeList NodeList = doc.SelectNodes("root/Title");
            foreach (XmlNode node in NodeList)
            {
                listold.Add(node.InnerText);
            }
            Log(string.Format("已有記錄 {0} 筆", listold.Count));
        }


        private static void Log(string str)
        {
            FileTool.ToolLog log = new FileTool.ToolLog(Settings1.Default.LogPath);
            log.Log(str);
        }

        void time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            Log("執行" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            Start();
        }


        /// <summary>
        /// 開始爬PTT網站及POST資料
        /// </summary>
        private void Start()
        {
            GetSite.GetSite site = new GetSite.GetSite(Settings1.Default.LogPath);
            XmlDocument doc = Tools.XmlTool.LoadXml(Settings1.Default.RecordPath+"\\"+ "Record.xml");
            XmlNode root = doc.SelectSingleNode("root");
            RecordTime = Convert.ToDateTime("2014/04/08 00:00:00");
            PushCount = Convert.ToInt32(Settings1.Default.PushCount);
            string Condition = Settings1.Default.Condition;
            Address = Settings1.Default.PostAddress;
            //Log("Address=" + Address + "   " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            Tag = 1150;
            //Log("Tag=" + Tag + "   " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            string Site = Settings1.Default.Site;
            SitePlus siteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + "/index" + Tag + ".html");
            Log("取得表特列表" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            List<SiteInfo> SiteInfoList = new List<SiteInfo>();
            Recursive(Tag, siteplus, SiteInfoList, Site, "/bbs/" + Site + "/index", Condition, doc, root);
        }


        /// <summary>
        /// 記錄已經Post出去的資料
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pTitle"></param>
        private static void Record(XmlDocument doc, string pTitle)
        {
            XmlNode root = doc.SelectSingleNode("root");
            XmlElement element = doc.CreateElement("Title");
            element.InnerText = pTitle;
            root.AppendChild(element);
            doc.Save(Settings1.Default.RecordPath+"\\"+ "Record.xml");
        }


        /// <summary>
        /// Post功能
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="SiteInfoList"></param>
        /// <returns></returns>
        private long POST(string Address, List<SiteInfo> SiteInfoList)
        {
            string strJson = "SiteInfoList=" + JsonConvert.SerializeObject(SiteInfoList, Newtonsoft.Json.Formatting.Indented);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Address);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(strJson);
            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream receiveStream = response.GetResponseStream();
                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader readStream = new StreamReader(receiveStream, encode);
                    length = response.ContentLength;
                    string st = readStream.ReadToEnd();
                    if (st.IndexOf("Dis") != -1)
                    {
                        DateTime date = DateTime.Now;                       
                    }
                }
            }
            catch (WebException ex)
            {
                Log(ex.Message + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            }
            return length;
        }

        /// <summary>
        /// 遞回所有頁面
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pSiteplus"></param>
        /// <param name="li"></param>
        /// <param name="Formate"></param>       
        public void Recursive(int index, SitePlus pSiteplus, List<SiteInfo> li, string Site, string Formate, string pCondition, XmlDocument doc, XmlNode root)
        {
            GetSite.GetSite site = new GetSite.GetSite(Settings1.Default.LogPath);
            foreach (string str in pSiteplus.Context)
            {
                GetSite.SiteInfo info = site.GetInfo(@"https://www.ptt.cc" + str);
                if (info.Title != null)
                {
                    if (info.Title.IndexOf(pCondition) != -1 && info.PushList.Count > PushCount)
                    {
                        Thread.Sleep(1000);
                        info.PushList.Clear();
                        if (info.Title.IndexOf("Re: ") == -1)
                        {
                            li.Add(info);
                            if (!listold.Contains(info.Title))
                            {
                                long length = POST(Address, li);
                                RecordTime = info.PostDate;
                                listold.Add(info.Title);                             
                                Log(string.Format("寫入紀錄 {0} ", info.Title));
                                Record(doc, info.Title);
                                Log("記錄結束");
                            }
                            li.Clear();
                        }
                    }
                }
            }
            foreach (string str in pSiteplus.Index)
            {
                string temp = str.Replace(Formate, "").Replace(".html", "");
                int result = 0;
                if (int.TryParse(temp, out result))
                {
                    if (result > index)
                    {
                        Thread.Sleep(1000);
                        Tag = result;
                        pSiteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + "/index" + result + ".html");
                        Recursive(result, pSiteplus, li, Site, Formate, pCondition, doc, root);
                    }
                }
            }
        }

        protected override void OnStop()
        {
            Log("服務停止" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        }
    }
}

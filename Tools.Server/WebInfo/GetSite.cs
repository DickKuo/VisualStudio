using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using CommTool;
using System.Globalization;
using System.Xml;
using System.Threading;
using WebInfo.Business.DataEntities;
using WebInfo.Business;
using WebInfo.Business.Services;

namespace WebInfo
{
    public class GetSite : IGetSiteService
    {
        Dictionary<string, int> Month = new Dictionary<string, int>();
        const int _defaultCount = 30;
        private int _pushcount = 0;
        public int PushCount { get{
            if (_pushcount == 0)
            {
                return _defaultCount;
            }
            else
            {
                return _pushcount;
            }
        }
            set {
                _pushcount = value;
            }
        }
        public DateTime RecordTime { set; get; }
        public static List<string> listold = new List<string>();
        public string PostAddress { set; get; }
        public int Tag { set; get; }

        public GetSite(string LogPath)
        {
            Month.Add("Jan", 1);
            Month.Add("Feb", 2);
            Month.Add("Mar", 3);
            Month.Add("Apr", 4);
            Month.Add("May", 5);
            Month.Add("Jun", 6);
            Month.Add("Jul", 7);
            Month.Add("Aug", 8);
            Month.Add("Sep", 9);
            Month.Add("Oct", 10);
            Month.Add("Nov", 11);
            Month.Add("Dec", 12);
            ToolLog.ToolPath = LogPath;
            listold = GetRecord();        
        }

        /// <summary>
        /// 抓取文章內容
        /// </summary>
        /// <param name="Url">Url</param>
        /// <returns></returns>
        public StreamReader GetWebInfo(string Url)
        {
            StreamReader reader = null;
            WebRequest myWebRequest = WebRequest.Create(Url);
            myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
                Stream DataStream = response.GetResponseStream();
                reader = new StreamReader(DataStream, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                ToolLog.Log(CommTool.LogType.Error, "抓取文章" + ex.Message);
            }
            return reader;
        }

        /// <summary>
        /// 抓取PTT文章列表         
        /// </summary>
        /// <param name="BaseUrl">起始網址Url</param>
        /// <returns></returns>
        public SitePlus GetUrlList(string BaseUrl)
        {
            SitePlus siteplus = new SitePlus();
            StreamReader reader = GetWebInfo(BaseUrl);
            if (reader != null)
            {
                string str = string.Empty;
                while ((str = reader.ReadLine()) != null)
                {
                    if (str.IndexOf("href=") != -1 && str.IndexOf(".html") != -1 && str.IndexOf("bbs") != -1)
                    {
                        MatchCollection matches = Regex.Matches(str, "href=\"[^\"]+\"", RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            string temp = match.Value.Trim().Replace("href=", "");
                            temp = temp.Replace("\"", "");
                            if (temp.IndexOf("index") != -1)
                            {
                                siteplus.Index.Add(temp);
                            }
                            else
                            {
                                siteplus.Context.Add(temp);
                            }
                        }
                    }
                }
            }
            return siteplus;
        }


        /// <summary>
        /// 20150410 針對網址解析
        /// 解析表特版文章內容。
        /// </summary>
        /// <param name="Url">Url</param>
        /// <returns></returns>
        public SiteInfo GetInfo(string Url)
        {
            SiteInfo Info = new SiteInfo();
            Info.Address = Url;
            StreamReader reader = this.GetWebInfo(Url);
            string str = string.Empty;
            if (reader != null)
            {
                string temp = reader.ReadToEnd();
                MatchCollection matches = Regex.Matches(temp, "<title>[^\"]+</title>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    Info.Title = match.Value.Replace("\"", "'").Replace("</title>", "").Replace("<title>", "").Replace("--", "");
                }
                matches = Regex.Matches(temp, "</span></div>[^\"]+<span class=\"f2\">", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    Info.Context = match.Value.Replace("\"", "'").Replace("</span></div>", "").Replace("<span class=\"f2\">", "").Replace("--", "");
                }
                if (Info.Context == null)
                {
                    MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(temp));
                    StreamReader secoend = new StreamReader(memory);
                    StringBuilder sb = new StringBuilder();
                    string line = string.Empty;
                    while ((line = secoend.ReadLine()) != null)
                    {                       
                        if (line.IndexOf("<span class=\"article-meta-value\">") != -1)
                        {
                            while ((line = secoend.ReadLine()) != null)
                            {
                                if (line.IndexOf("<span class=\"f2\">") != -1)
                                {
                                    break;
                                }

                                #region 20140908 修改網址，轉換成圖片連結
                                if (line.IndexOf("http://ppt.cc/") != -1)
                                {
                                    matches = Regex.Matches(line, "\">[^\"]+</a>", RegexOptions.IgnoreCase);
                                    foreach (Match match in matches)
                                    {
                                        line = match.Value.Replace("\">", "<br/><img  src=\"").Replace("</a>", "@.jpg\"  /><br/><br/>");
                                    }
                                }
                                #endregion

                                #region 轉youtube 網址
                                if (line.ToLower().Replace(".", "").Trim().IndexOf("youtube") != -1)
                                {
                                    matches = Regex.Matches(line, "https://www.youtube.com[^\"]+", RegexOptions.IgnoreCase);
                                    if (matches.Count > 0)
                                    {
                                        foreach (Match match in matches)
                                        {
                                            line = "<a href =\"" + match.Value + "\">影片連結</a>";
                                        }
                                    }
                                    else
                                    {
                                        matches = Regex.Matches(line, "http://youtu.be[^\"]+", RegexOptions.IgnoreCase);
                                        foreach (Match match in matches)
                                        {
                                            line = "<a href =\"" + match.Value + "\">影片連結</a>";
                                        }
                                    }
                                }
                                #endregion

                                #region 加入data-original
                                matches = Regex.Matches(line, "src=\"[^\"]+", RegexOptions.IgnoreCase);
                                foreach (Match match in matches)
                                {
                                    string href = match.Value.Replace("src=\"", "");
                                    line = line.Replace(".jpg\"", ".jpg\" data-original='" + href + "\" class='lazy'");
                                }
                                #endregion

                                if (line.IndexOf(" <span class=\"f3 hl\">") != -1)
                                {
                                    break;
                                }
                                sb.Append(line).Replace("--", "");
                            }
                            break;
                        }
                    }
                    Info.Context = sb.ToString().Replace("\"", "'");
                }
                matches = Regex.Matches(temp, "時間</span><span class=\"article-meta-value\">[^\"]+</span></div>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    //Wed Jul 23 16:41:02 2014
                    string s = match.Value.Replace("時間</span><span class=\"article-meta-value\">", "").Replace("</span></div>", "");
                    string[] spl = s.Split(' ');
                    if (spl.Length > 4)
                    {
                        string stemp = spl[4] + spl[1] + spl[2] + spl[3];
                        IFormatProvider ifp = new CultureInfo("en-US").DateTimeFormat;
                        int month = Month[spl[1]];
                        string[] sptime = spl[3].Split(':');
                        if (sptime.Length > 2)
                        {
                            Info.PostDate = new DateTime(Convert.ToInt32(spl[4]), month, Convert.ToInt32(spl[2]), Convert.ToInt32(sptime[0]), Convert.ToInt32(sptime[1]), Convert.ToInt32(sptime[2]));
                        }
                        else
                        {
                            sptime = spl[4].Split(':');
                            if (sptime.Length > 2)
                            {
                                Info.PostDate = new DateTime(Convert.ToInt32(spl[5]), month, Convert.ToInt32(spl[3]), Convert.ToInt32(sptime[0]), Convert.ToInt32(sptime[1]), Convert.ToInt32(sptime[2]));
                            }
                        }
                    }
                }
                matches = Regex.Matches(temp, "作者</span><span class=\"article-meta-value\">[^\"]+</span></div>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    Info.Author = match.Value.Replace("\"", "'").Replace("作者</span><span class=\"article-meta-value\">", "").Replace("</span></div>", "").Replace("--", "");
                }

                matches = Regex.Matches(temp, "<span class=\"hl push-tag\">[^\"]+</span>", RegexOptions.IgnoreCase);
                MatchCollection matchesId = Regex.Matches(temp, "<span class=\"f3 hl push-userid\">[^\"]+</span>", RegexOptions.IgnoreCase);
                MatchCollection matchescontent = Regex.Matches(temp, "<span class=\"f3 push-content\">[^\"]+</span>", RegexOptions.IgnoreCase);
                int count = 0;
                foreach (Match match in matches)
                {
                    if (matches.Count > count && matchesId.Count > count && matchescontent.Count > count)
                    {
                        string matchstr = match.Value + matchesId[count] + matchescontent[count];
                        Info.PushList.Add(matchstr.Replace("\"", "'"));
                    }
                    count++;
                }
            }
            return Info;
        }


        /// <summary>
        /// 20150505 modified 加入其他屬性
        /// </summary>
        /// <param name="doc">Record_Doc</param>
        /// <param name="pTitle">文章開頭</param>
        /// <param name="Address">網址</param>
        /// <param name="PushCount">推文數</param>
        private static void Record(XmlDocument doc, string pTitle, string Address, string PushCount)
        {
            XmlNode root = doc.SelectSingleNode("root");
            XmlElement element = doc.CreateElement("Title");
            element.InnerText = pTitle;
            element.SetAttribute("Time", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            element.SetAttribute("Address", Address);
            element.SetAttribute("PushCount", PushCount);
            root.AppendChild(element);
            doc.Save(ToolLog.ToolPath + "\\" + "Record.xml");
        }


        /// <summary>
        /// 20150505 add by Dick for 初始化已存在的紀錄
        /// </summary>
        private static List<string> GetRecord()
        {
            List<string> result = new List<string>();
            string RecordFilePath = Path.Combine(ToolLog.ToolPath, "Record.xml");
            if (File.Exists(RecordFilePath))
            {
                XmlDocument doc = XmlFile.LoadXml(RecordFilePath);
                XmlNode root = doc.SelectSingleNode("root");
                foreach (XmlNode node in root.ChildNodes)
                {
                    string value = node.InnerText;
                    if (!result.Contains(value))
                    {
                        result.Add(value);
                    }
                }
            }
            return result;
        }



        /// <summary>
        /// 遞回所有頁面
        /// </summary>
        /// <param name="index">起始索引列</param>
        /// <param name="pSiteplus">輔助類</param>
        /// <param name="li">Post列</param>
        /// <param name="Site">Ptt版</param>
        /// <param name="Formate">網頁清單清除條件</param>
        /// <param name="pCondition">撈取資料條件</param>
        /// <param name="doc">存檔Xml Document</param>
        /// <param name="root">跟索引</param>     
        public void Recursive(ref int index, SitePlus pSiteplus, List<SiteInfo> li, string Site, string Formate, string pCondition, XmlDocument doc, XmlNode root)
        {
            GetSite site = new GetSite(ToolLog.ToolPath);
            foreach (string str in pSiteplus.Context)
            {
                string Url = @"https://www.ptt.cc" + str;
                SiteInfo info = site.GetInfo(Url);
                Thread.Sleep(1000);
                if (info.Title != null)
                {
                    if (info.Title.IndexOf(pCondition) != -1 && info.PushList.Count > PushCount)
                    {
                        info.PushList.Clear();
                        if (info.Title.IndexOf("Re: ") == -1)
                        {
                            li.Add(info);
                            if (!listold.Contains(info.Title.Trim()))
                            {
                                WebInfo webinfo = new WebInfo(ToolLog.ToolPath);
                                Console.WriteLine(info.Title);
                                long length = webinfo.POST(PostAddress, li);
                                Thread.Sleep(1000);
                                RecordTime = info.PostDate;
                                listold.Add(info.Title.Trim());
                                ToolLog.Log(string.Format("寫入紀錄 {0} ", info.Title));
                                ToolLog.Log(string.Format("Url ：  {0} ", Url));
                                Record(doc, info.Title.Trim(), Url, info.PushList.Count.ToString());
                                ToolLog.Log("記錄結束");
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
                        index = result;
                        pSiteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + "/index" + result + ".html");
                        Recursive(ref index, pSiteplus, li, Site, Formate, pCondition, doc, root);
                    }
                }
            }
        }


    } 
}

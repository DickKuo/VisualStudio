﻿using System.Text.RegularExpressions;
using WebInfo.Business.DataEntities;
using System.Security.Permissions;
using System.Collections.Generic;
using WebInfo.Business.Services;
using System.Globalization;
using System.Threading;
using System.Security;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;
using CommTool;
using System;

namespace WebInfo
{
    public class GetSite : IGetSiteService
    {
        private class Default
        {
            public const string GetContext = "抓取文章";
            public const string Attributes_href = "href";
            public const int Second = 1000;
            public const string HtmlExtend = ".html";
            public const string Href = "href=";
            public const string DoubleNegativeSymbol = "--";
            public const string index = "index";
            public const string Quote = "\"";
            public const string SingleQuote = "'";
            public const string Record = "Record.xml";
            public const string Root = "root";
            public const string bbs = "bbs";
        }

        private class HtmlStartTag
        {
            public const string Title = "<title>";
        }

        private class HtmlEndTag
        {
            public const string Title = "</title>";
        }

        Dictionary<string, int> Month = new Dictionary<string, int>();

        private const int _defaultCount = 30;

        private int _pushcount = 0;

        public int PushCount
        {
            get
            {
                if (_pushcount == 0)
                {
                    return _defaultCount;
                }
                else
                {
                    return _pushcount;
                }
            }
            set
            {
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

        /// <summary>抓取文章內容 </summary>
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
                ToolLog.Log(CommTool.LogType.Error, Default.GetContext + ex.Message);
                ToolLog.Log(Url);
            }
            return reader;
        }

        /// <summary> 抓取文章內容  20150616 擴充可以選擇是否避免相關指定網頁以外的網址 工具集 #68</summary>
        /// <param name="Url"></param>
        /// <param name="IsAvoid">true 如果不是指定的網站類型則跳過</param>
        /// <param name="PointUrl">指定網址類型</param>
        /// <returns></returns>
        public StreamReader GetWebInfo(string Url, bool IsAvoid, string PointUrl)
        {
            if (IsAvoid)
            {
                if (Url.IndexOf(PointUrl) == -1)
                {
                    return null;
                }
            }
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
                ToolLog.Log(CommTool.LogType.Error, Default.GetContext + ex.Message);
                ToolLog.Log(Url);
            }
            return reader;
        }

        /// <summary> 抓取PTT文章列表 </summary>
        /// <param name="BaseUrl">起始網址Url</param>
        /// <returns></returns>
        public SitePlus GetUrlList(string BaseUrl)
        {
            SitePlus siteplus = new SitePlus();
            Console.WriteLine(BaseUrl);
            StreamReader reader = GetWebInfo(BaseUrl);
            if (reader != null)
            {
                string str = string.Empty;
                while ((str = reader.ReadLine()) != null)
                {
                    if (str.IndexOf(Default.Href) != -1 && str.IndexOf(Default.HtmlExtend) != -1 && str.IndexOf(Default.bbs) != -1)
                    {
                        MatchCollection matches = Regex.Matches(str, "href=\"[^\"]+\"", RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            string temp = match.Value.Trim().Replace(Default.Href, string.Empty);
                            temp = temp.Replace(Default.Quote, string.Empty);
                            if (temp.IndexOf(Default.index) != -1)
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

        /// <summary> 20150410 針對網址解析 解析表特版文章內容。
        /// </summary>
        /// <param name="Url">Url</param>
        /// <returns></returns>
        public SiteInfo GetInfo(string Url)
        {
            SiteInfo Info = new SiteInfo();
            Info.Address = Url;
            Console.WriteLine(Url);
            ToolLog.Log(Url);
            StreamReader reader = this.GetWebInfo(Url);
            string str = string.Empty;
            if (reader != null)
            {
                string temp = reader.ReadToEnd();
                MatchCollection matches = Regex.Matches(temp, string.Format("{0}[^\"]+{1}", HtmlStartTag.Title, HtmlEndTag.Title), RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    Info.Title = match.Value.Replace(Default.Quote, Default.SingleQuote).Replace(HtmlEndTag.Title, string.Empty).Replace(HtmlStartTag.Title, string.Empty).Replace(Default.DoubleNegativeSymbol, string.Empty);
                }
                matches = Regex.Matches(temp, "</span></div>[^\"]+<span class=\"f2\">", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    Info.Context = match.Value.Replace(Default.Quote, Default.SingleQuote).Replace("</span></div>", string.Empty).Replace("<span class=\"f2\">", string.Empty).Replace(Default.DoubleNegativeSymbol, string.Empty);
                    //20150609 #64
                    Info.Context = GetAnalysis(temp, Info.Context);
                }
                if (Info.Context == null)
                {
                    MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(temp));
                    StreamReader secoend = new StreamReader(memory);
                    StringBuilder sb = new StringBuilder();
                    string line = string.Empty;
                    List<string> ExistImage = new List<string>(); //避免重複加入圖片
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
                                if (line.ToLower().Replace(".", string.Empty).Trim().IndexOf("youtube") != -1)
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
                                    string href = match.Value.Replace("src=\"", string.Empty);
                                    line = line.Replace(".jpg\"", ".jpg\" data-original='" + href + "\" class='lazy'");
                                }
                                #endregion

                                #region 簽名檔則跳開
                                if (line.IndexOf(" <span class=\"f3 hl\">") != -1)
                                {
                                    break;
                                }

                                if (line.IndexOf(Default.DoubleNegativeSymbol) != -1 && line.Length < 3)
                                {
                                    break;
                                }
                                #endregion

                                #region 20150603 可以轉換miupix 網站的照片功能，將真正的Img 位址解析出來 #60

                                if (line.IndexOf("http://miupix.cc/") != -1)
                                {
                                    //matches = Regex.Matches(temp, "<a href=\"[^\"]+", RegexOptions.IgnoreCase);
                                    matches = Regex.Matches(temp, @"<a href=\""http://[a-zA-Z0-9\./_-]+", RegexOptions.IgnoreCase);
                                    StringBuilder ImageUrls = new StringBuilder();
                                    foreach (Match match in matches)
                                    {
                                        ImageUrls.Append(this.GetMiupixImg(match.Value.Replace("<a href=\"", string.Empty)));
                                        ImageUrls.Append("\r\n");
                                    }
                                    if (!ExistImage.Contains(ImageUrls.ToString()))
                                    {
                                        line = ImageUrls.ToString();
                                        ExistImage.Add(ImageUrls.ToString());
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                #endregion
                                sb.Append(line).Replace(Default.DoubleNegativeSymbol, string.Empty);
                            }
                            break;
                        }
                    }
                    Info.Context = sb.ToString().Replace(Default.Quote, Default.SingleQuote);
                }
                matches = Regex.Matches(temp, "時間</span><span class=\"article-meta-value\">[^\"]+</span></div>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    //Wed Jul 23 16:41:02 2014
                    string s = match.Value.Replace("時間</span><span class=\"article-meta-value\">", string.Empty).Replace("</span></div>", string.Empty);
                    string[] spl = s.Split(' ');
                    if (spl.Length > 4)
                    {
                        string stemp = spl[4] + spl[1] + spl[2] + spl[3];
                        IFormatProvider ifp = new CultureInfo("en-US").DateTimeFormat;
                        int month = Month[spl[1]];
                        string[] sptime = spl[3].Split(':');
                        if (sptime.Length > 2)
                        {
                            try
                            {
                                Info.PostDate = new DateTime(Convert.ToInt32(spl[4]), month, Convert.ToInt32(spl[2]), Convert.ToInt32(sptime[0]), Convert.ToInt32(sptime[1]), Convert.ToInt32(sptime[2]));
                            }
                            catch (Exception ex)
                            {
                                Info.PostDate = DateTime.Now;
                                ToolLog.Log(ex.Message);
                            }
                        }
                        else
                        {
                            sptime = spl[4].Split(':');
                            if (sptime.Length > 2)
                            {
                                int year = 0;
                                if (spl.Length > 5)
                                {
                                    int.TryParse(spl[5], out year);
                                }
                                else
                                {
                                    year = DateTime.Now.Year;
                                }
                                try
                                {
                                    Info.PostDate = new DateTime(year, month, Convert.ToInt32(spl[3]), Convert.ToInt32(sptime[0]), Convert.ToInt32(sptime[1]), Convert.ToInt32(sptime[2]));
                                }
                                catch (Exception ex)
                                {
                                    Info.PostDate = DateTime.Now;
                                    ToolLog.Log(ex.Message);
                                }
                            }
                        }
                    }
                }
                matches = Regex.Matches(temp, "作者</span><span class=\"article-meta-value\">[^\"]+</span></div>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    Info.Author = match.Value.Replace(Default.Quote, Default.SingleQuote).Replace("作者</span><span class=\"article-meta-value\">", string.Empty).Replace("</span></div>", string.Empty).Replace(Default.DoubleNegativeSymbol, string.Empty);
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
                        Info.PushList.Add(matchstr.Replace(Default.Quote, Default.SingleQuote));
                    }
                    count++;
                }
            }
            return Info;
        }

        /// <summary> 20150609 猜解PTT文章內容 #64 </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private string GetAnalysis(string context, string result)
        {
            StringBuilder sb = new StringBuilder();
            if (result.IndexOf("jpg") == -1)
            {
                if (result.IndexOf("<a href") == -1)
                {
                    string[] array = context.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    bool Into = false;
                    foreach (string str in array)
                    {
                        if (str.IndexOf("class=\"article-meta-tag\">時間</span><span") != -1)
                        {
                            Into = true;
                            continue;
                        }
                        if (str.IndexOf("class=\"f2\">※") != -1)
                        {
                            Into = false;
                        }

                        if (Into)
                        {
                            sb.AppendFormat("{0}\r\n", str);
                        }
                    }
                    return sb.ToString();
                }
                else
                {
                    return result;
                }
            }
            else
            {
                return result;
            }
        }

        /// <summary>20150505 modified 加入其他屬性</summary>
        /// <param name="doc">Record_Doc</param>
        /// <param name="pTitle">文章開頭</param>
        /// <param name="Address">網址</param>
        /// <param name="PushCount">推文數</param>
        private static void Record(XmlDocument doc, string pTitle, string Address, string PushCount)
        {
            XmlNode root = doc.SelectSingleNode(Default.Root);
            XmlElement element = doc.CreateElement(BaseConst.Title);
            element.InnerText = pTitle;
            element.SetAttribute("Time", DateTime.Now.ToString(BaseConst.TimeFormatComplete));
            element.SetAttribute("Address", Address);
            element.SetAttribute("PushCount", PushCount);
            root.AppendChild(element);
            string recordpath = Path.Combine(ToolLog.ToolPath, Default.Record);
            #region 20150520 加入檔案存取權限
            FileIOPermission f2 = new FileIOPermission(PermissionState.None);
            f2.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, recordpath);
            try
            {
                f2.Demand();
            }
            catch (SecurityException s)
            {
                ToolLog.Log(s.Message);
            }
            #endregion
            doc.Save(recordpath);
        }

        /// <summary>20150505 add by Dick for 初始化已存在的紀錄</summary>
        private static List<string> GetRecord()
        {
            List<string> result = new List<string>();
            string RecordFilePath = Path.Combine(ToolLog.ToolPath, Default.Record);
            if (File.Exists(RecordFilePath))
            {
                XmlDocument doc = XmlFile.LoadXml(RecordFilePath);
                XmlNode root = doc.SelectSingleNode(Default.Root);
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

        /// <summary>遞回所有頁面 </summary>
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
                Thread.Sleep(1500);
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
                                #region 20150513 加入功能 如果POST失敗則過5秒後在Post 次  如果10次都失敗則放棄
                                long length = 0;
                                int count = 0;
                                bool Faill = false;
                                do
                                {
                                    Thread.Sleep(Default.Second * 5);
                                    length = webinfo.POST(PostAddress, li, true);
                                    if (length == 8055)
                                    {
                                        Faill = true;
                                    }
                                    else
                                    {
                                        Faill = false;
                                    }
                                    if (count == 10)
                                    {
                                        Faill = false;
                                    }
                                    count++;
                                } while (Faill);
                                #endregion
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
                Thread.Sleep(1500); //修改撈資料頻率
            }
            foreach (string str in pSiteplus.Index)
            {
                string temp = str.Replace(Formate, string.Empty).Replace(Default.HtmlExtend, string.Empty);
                int result = 0;
                if (int.TryParse(temp, out result))
                {
                    if (result > index)
                    {
                        Thread.Sleep(1500);
                        Tag = result;
                        index = result;
                        pSiteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + Path.DirectorySeparatorChar + Default.index + result + Default.HtmlExtend);
                        Recursive(ref index, pSiteplus, li, Site, Formate, pCondition, doc, root);
                    }
                }
            }
        }

        /// <summary>工具集 #60 將miupix的圖片轉換成實際圖片位置
        /// </summary>
        /// <param name="Url">欲轉換網址</param>
        /// <returns></returns>
        public string GetMiupixImg(string Url)
        {
            //20150616 工具集 #68  避掉相關網址
            StreamReader sq = this.GetWebInfo(Url, true, "http://miupix.cc/");
            string line = string.Empty;
            string result = string.Empty;
            if (sq != null)
            {
                while ((line = sq.ReadLine()) != null)
                {
                    if (line.IndexOf("img src") != -1 && line.IndexOf("alt") != -1 && line.IndexOf("thumb") != -1)
                    {
                        MatchCollection matches = Regex.Matches(line, "<img src=\"[^\"]+", RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            result = match.Value;
                            result += "\" />";
                        }
                    }
                }
            }
            else
            {
                ToolLog.Log(LogType.Exclude, Url);
            }
            return result;
        }
    }
}
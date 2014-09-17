using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GetSite
{
    public class GetSite
    {
        Dictionary<string, int> Month = new Dictionary<string, int>();
        FileTool.ToolLog log;
        public GetSite(string LogPath)
        {
            Month.Add("Jan",1);
            Month.Add("Feb",2);
            Month.Add("Mar",3);
            Month.Add("Apr",4);
            Month.Add("May",5);
            Month.Add("Jun",6);
            Month.Add("Jul",7);
            Month.Add("Aug",8);
            Month.Add("Sep",9);
            Month.Add("Oct",10);
            Month.Add("Nov",11);
            Month.Add("Dec",12);
            log = new FileTool.ToolLog(LogPath);          
        }

        /// <summary>
        /// 抓取文章內容
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public StreamReader GetWebInfo(string Url)
        {  
            StreamReader reader =null;                           
            WebRequest myWebRequest = WebRequest.Create(Url);
            myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
                Stream DataStream = response.GetResponseStream();
                reader = new StreamReader(DataStream, Encoding.UTF8);
            }
            catch (Exception ex) {
                log.Log(FileTool.LogType.Error, ex.Message);
            }
            return reader;
        }

        /// <summary>
        /// 抓取PTT文章列表         
        /// </summary>
        /// <param name="BaseUrl"></param>
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



        public SiteInfo GetInfo(string Url)
        {
            SiteInfo Info = new SiteInfo();
            Info.Address = Url;
            StreamReader reader = this.GetWebInfo(Url);
            string str=string.Empty;
            if (reader != null)
            {
            string temp = reader.ReadToEnd();         
                MatchCollection matches = Regex.Matches(temp, "<title>[^\"]+</title>", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    Info.Title = match.Value.Replace("\"", "'").Replace("</title>", "").Replace("<title>", "").Replace("--","") ;                   
                }
                matches = Regex.Matches(temp, "</span></div>[^\"]+<span class=\"f2\">", RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    Info.Context = match.Value.Replace("\"", "'").Replace("</span></div>", "").Replace("<span class=\"f2\">", "").Replace("--", "");    
                }
                if(Info.Context==null) {
                    MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(temp));
                    StreamReader secoend = new StreamReader(memory);  
                    StringBuilder sb =new StringBuilder();
                    string line = string.Empty;
                    while( (line =secoend.ReadLine())!=null)
                    {
                        if(line.IndexOf("<span class=\"article-meta-value\">") != -1) {
                            while((line = secoend.ReadLine()) != null) {
                                if(line.IndexOf("<span class=\"f2\">") != -1) {
                                    break;
                                }

                                #region 20140908 修改網址，轉換成圖片連結
                                if (line.IndexOf("http://ppt.cc/")!=-1)
                                {
                                    matches = Regex.Matches(line, "\">[^\"]+</a>", RegexOptions.IgnoreCase);
                                    foreach (Match match in matches)
                                    {                                     
                                        line = match.Value.Replace("\">", "<br/><img  src=\"").Replace("</a>", "@.jpg\"  /><br/><br/>");
                                    }
                                }
                                #endregion

                                #region 轉youtube 網址
                                if(line.ToLower().Replace(".", "").Trim().IndexOf("youtube") != -1) {
                                    matches = Regex.Matches(line, "https://www.youtube.com[^\"]+", RegexOptions.IgnoreCase);
                                    if(matches.Count > 0) {
                                        foreach(Match match in matches) {
                                            line = "<a href =\"" + match.Value + "\">影片連結</a>";
                                        }
                                    }
                                    else {
                                        matches = Regex.Matches(line, "http://youtu.be[^\"]+", RegexOptions.IgnoreCase);
                                        foreach(Match match in matches) {
                                            line = "<a href =\"" + match.Value + "\">影片連結</a>";
                                        }
                                    }
                                }  
                                #endregion     
                           
                                #region 加入data-original
                                matches = Regex.Matches(line, "[^\"]+.jpg</a>", RegexOptions.IgnoreCase);
                                foreach(Match match in matches) {
                                    string href = match.Value.Replace(">", "").Replace("</a", "");
                                    line = line.Replace(".jpg\"", " .jpg\" data-original=\"" + href + "\"");
                                }
                                #endregion
                                

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
                foreach(Match match in matches) {
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
    }

    /// <summary>
    /// 輔助類別
    /// </summary>
    public class SitePlus
    {       
        public List<string> Context = new List<string>();
        public List<string> Index = new List<string>();
    }


    public class SiteInfo
    {
       public string Author { set; get; }
       public DateTime PostDate { set; get;}
       public string Address { set; get; }
       public string Title { set; get; }
       public string Context { set; get; }
       public StringBuilder Push = new StringBuilder();
       public List<string> PushList = new List<string>();

    }
}

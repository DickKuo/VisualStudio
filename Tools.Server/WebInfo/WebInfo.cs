using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using CommTool;
using WebInfo.Business.DataEntities;
using WebInfo.Business;
using System.Xml;
using HtmlAgilityPack;

namespace WebInfo
{
    public class WebInfo : IWebInfoService
    {
        private class Default
        {
            public const string RecordEnd = "記錄結束";
            public const string RecordFormat = "寫入紀錄 {0} ";
            public const string Re = "Re: ";
            public const string PostError = "POST失敗：";
            public const int Zero = 0;
            public const int MinItem = 0;
            public const int FirstItem = 0;
            public const string Post = "Post";
            public const string Get = "Get";
            public const string ContentType = "text/xml; encoding='utf-8'";
            public const string UTF8 = "utf-8";
            public const string PostMessage = "Post Start";
            public const string JsonKey = "json";
            public const string JSonStringFormat = "{0}={1}";
            public const string DefaultLogPath = @"C:\SLog";
            public const string ResourceAddress = "https://www.ptt.cc/bbs/Beauty/";
            public const string HTML = ".html";
        }

        private string _pLogPath;

        public WebInfo(string pLogPath) {
            ToolLog.ToolPath = pLogPath;
            _pLogPath = pLogPath;
        }

        public WebInfo() {
            ToolLog.ToolPath = Default.DefaultLogPath;
        }

        /// <summary>PostHttp資料給指定位置</summary>
        /// <param name="Data"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string HttpPostMethod(string Data, string Url) {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Data);
            var request = System.Net.HttpWebRequest.Create(Url) as System.Net.HttpWebRequest;
            request.Method = Default.Post;
            request.ContentType = Default.ContentType;
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, Default.Zero, bytes.Length);
            requestStream.Close();
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            string responseStr = string.Empty;
            if (response.StatusCode == HttpStatusCode.OK) {
                Stream responseStream = response.GetResponseStream();
                responseStr = new StreamReader(responseStream).ReadToEnd();
            }
            return responseStr;
        }
        
        /// <summary> 抓取網頁資訊</summary>
        /// <param name="pUrl">網址</param>
        /// <returns></returns>
        public StreamReader GetResponse(string pUrl) {
            WebRequest myWebRequest = WebRequest.Create( pUrl);
            myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
            Stream DataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(DataStream, Encoding.Default);
            return reader;
        }
        
        /// <summary>抓取網頁資訊</summary>
        /// <param name="pUrl"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public StreamReader GetResponse(string pUrl,Encoding Coding) { 
            WebRequest myWebRequest = WebRequest.Create(pUrl);
            myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
            Stream DataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(DataStream, Coding);
            return reader;
        }
              
        /// <summary> 抓取網頁資訊To String </summary>
        /// <param name="pUrl">網址</param>
        /// <returns></returns>
        public virtual string GetStringResponse(string pUrl) {
            string result = string.Empty;
            try {
                StreamReader reader = GetResponse(pUrl);
                result = reader.ReadToEnd();
            }
            catch (Exception ex) {
                result = ex.Message;
            }
            return result;
        }
        
        /// <summary>抓取網頁資訊To String  </summary>
        /// <param name="pUrl"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public virtual string GetStringResponse(string pUrl, Encoding Code) {
            string result = string.Empty;
            try {
                StreamReader reader = GetResponse(pUrl, Code);
                result = reader.ReadToEnd();
            }
            catch (Exception ex) {
                result = ex.Message;
            }
            return result;
        }
        
        /// <summary>Post功能</summary>
        /// 20160909 加入紀錄POST的JSON資料 By Dick 
        /// <param name="Address">網址</param>
        /// <param name="SiteInfoList">文章列</param>
        /// <returns></returns>
        public long POST(string Address, List<SiteInfo> SiteInfoList) {
            long length = Default.Zero;
            SiteInfo info = new SiteInfo();
            if (SiteInfoList.Count > Default.Zero) {
                info = SiteInfoList[Default.FirstItem];
            }
            try {
                if (info != null) {
                    ToolLog.Log(Default.PostMessage + info.Title);
                    Beauty Data = new Beauty();
                    Data.Author = info.Author;
                    Data.Content = info.Context;
                    Data.Title = info.Title;
                    Data.Guid = info.Address.Replace(Default.ResourceAddress, string.Empty).Replace(Default.HTML, string.Empty);
                    string strJson = string.Format(Default.JSonStringFormat, Default.JsonKey, JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented));
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Address);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = Default.Post;
                    byte[] byteArray = Encoding.UTF8.GetBytes(strJson);
                    request.ContentLength = byteArray.Length;
                    using (Stream dataStream = request.GetRequestStream()) {
                        dataStream.Write(byteArray, Default.Zero, byteArray.Length);
                    }
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                        Stream receiveStream = response.GetResponseStream();
                        Encoding encode = System.Text.Encoding.GetEncoding(Default.UTF8);
                        StreamReader readStream = new StreamReader(receiveStream, encode);
                        length = response.ContentLength;
                        string st = readStream.ReadToEnd();
                        if (st.IndexOf("Dis") != -1) {
                            DateTime date = DateTime.Now;
                        }
                    }
                    ToolLog.Record(strJson);
                }
            }
            catch (WebException ex) {
                ToolLog.Log(CommTool.LogType.Error, Default.PostError + ex.Message);
                length = 8055;
            }
            return length;
        }

        /// <summary>用GET方式PO文</summary>
        /// <param name="Address"></param>
        /// <param name="SiteInfoList"></param>
        public void BueatyPostContentByGetMothed(string Address, List<SiteInfo> SiteInfoList) {
            SiteInfo info = new SiteInfo();
            if (SiteInfoList.Count > Default.Zero) {
                info = SiteInfoList[Default.FirstItem];
            }
            try {
                ToolLog.Log(Default.PostMessage + info.Title);
                Beauty Data = new Beauty();
                Data.Author = SiteInfoList[Default.MinItem].Author;
                Data.Content = SiteInfoList[Default.MinItem].Context;
                Data.Title = SiteInfoList[Default.MinItem].Title;
                string strJson = string.Format(Default.JSonStringFormat, Default.JsonKey, JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented));
                string FullPath = Address + strJson;
                GetStringResponse(FullPath);        
                ToolLog.Record(strJson);
            }
            catch (Exception ex) {
                ToolLog.Log(CommTool.LogType.Error, Default.PostError + ex.Message);  
            }
        }

        /// <summary>直接針對網址進行解析內容及傳輸</summary>
        /// 20150410 add by Dick for 直接針對網址進行解析內容及傳輸。
        /// <param name="Url">指定網址</param>
        /// <param name="pCondition">標題限制必須包含字串</param>
        /// <param name="PushCount">推文數量</param>
        /// <param name="PostAdress">分析後傳送位址</param>
        public void GetBueatyDirtory(string Url, string pCondition, int PushCount, string PostAdress) {
            List<SiteInfo> li = new List<SiteInfo>();
            GetSite info = new GetSite(_pLogPath);
            SiteInfo SiteInfo = info.GetInfo(Url);
            if (SiteInfo.Title != null) {
                if (SiteInfo.Title.IndexOf(pCondition) != -1 && SiteInfo.PushList.Count > PushCount) {
                    SiteInfo.PushList.Clear();
                    if (SiteInfo.Title.IndexOf(Default.Re) == -1) {
                        li.Add(SiteInfo);
                        long length = POST(PostAdress, li);
                        ToolLog.Log(string.Format(Default.RecordFormat, SiteInfo.Title));
                        ToolLog.Log(Default.RecordEnd);
                        li.Clear();
                    }
                }
            }
        }

        /// <summary>直接針對網址進行解析內容及傳輸</summary>
        /// 20150410 add by Dick for 直接針對網址進行解析內容及傳輸。
        /// 20150428 modified by Dick for 不需要過濾標題條件
        /// <param name="Url">指定網址</param>
        /// <param name="PushCount">推文數量</param>
        /// <param name="PostAdress">分析後傳送位址</param>
        public void GetBueatyDirtory(string Url, int PushCount, string PostAdress) {
            List<SiteInfo> li = new List<SiteInfo>();
            GetSite info = new GetSite(_pLogPath);
            SiteInfo SiteInfo = info.GetInfo(Url);
            if (SiteInfo.Title != null) {
                if (SiteInfo.PushList.Count > PushCount) {
                    SiteInfo.PushList.Clear();
                    if (SiteInfo.Title.IndexOf(Default.Re) == -1) {
                        li.Add(SiteInfo);
                        long length = POST(PostAdress, li);
                        ToolLog.Log(string.Format(Default.RecordFormat, SiteInfo.Title));
                        ToolLog.Log(Default.RecordEnd);
                        li.Clear();
                    }
                }
            }
        }

        /// <summary>取得網站的HtmlDocument</summary>
        /// <param name="ppweburl"></param>
        /// <returns></returns>
        public HtmlDocument GetWebHtmlDocument(string ppweburl, Encoding Encode) {
            HttpWebRequest MyHttpWebRequest = HttpWebRequest.Create(ppweburl) as HttpWebRequest;
            HttpWebResponse MyHttpWebResponse = MyHttpWebRequest.GetResponse() as HttpWebResponse;
            StreamReader myStreamReader = new StreamReader(MyHttpWebResponse.GetResponseStream(), Encode);
            HtmlAgilityPack.HtmlDocument _HtmlDocument = new HtmlAgilityPack.HtmlDocument();
            _HtmlDocument.LoadHtml(myStreamReader.ReadToEnd());
            myStreamReader.Close();
            MyHttpWebResponse = null;
            MyHttpWebRequest = null;
            return _HtmlDocument;
        }

        /// <summary>取得網站的Tag陣列</summary>
        /// <param name="Url"></param>
        /// <param name="Tag"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public HtmlNodeCollection GetWebHtmlDocumentNodeCollection(string Url, string Tag, Encoding Code)
        {
            HtmlAgilityPack.HtmlDocument _HtmlDocument = GetWebHtmlDocument(Url, Code);
            HtmlAgilityPack.HtmlNodeCollection anchors = _HtmlDocument.DocumentNode.SelectNodes(Tag);
            return anchors;
        }

        /// <summary>取得網站的Tag陣列</summary>
        /// <param name="Url"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public HtmlNodeCollection GetWebHtmlDocumentNodeCollection(string Url, string Tag) {
            HtmlAgilityPack.HtmlDocument _HtmlDocument = GetWebHtmlDocument(Url, Encoding.Default);
            HtmlAgilityPack.HtmlNodeCollection anchors = _HtmlDocument.DocumentNode.SelectNodes(Tag);
            return anchors;
        }
    }
}


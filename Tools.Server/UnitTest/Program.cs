using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebInfo;
using System.IO;
using WebInfo.Business.DataEntities;
using System.Reflection;
using CommTool;
using CommTool.Business.Services;
using System.Text.RegularExpressions;
using CommTool.Business.Metadata;
using System.Net;
using Stock;
using System.Data;

namespace UnitTest {   
    class Program {

        private class Default {
            public const string TimeFormat = "yyyy/MM/dd";
        }

        static void Main(string[] args) {

            //StreamReader sw = new StreamReader(@"C:\Users\Dick\Desktop\新增資料夾\111.txt",Encoding.Default);
            //StringBuilder sb = new StringBuilder();
            //string  context = sw.ReadToEnd();
            //MatchCollection matches = Regex.Matches(context, "</span></div>[^\"]+<span class=\"f2\">", RegexOptions.Multiline);
            //foreach (Match match in matches)
            //{
            //    string resul = match.Value;
            //    GetAnalysis(sb, context, resul);
            //}
            //ToolLog.Log("");
            //System.Attribute attribute = null;
            //Assembly assembly = Assembly.LoadFrom(@"C:\Users\Dick\Desktop\Black\CommTool.Service.dll");
            //object obj = assembly.CreateInstance("TestAddin");
            //ITestService Itest = CallServihce.GetService<ITestService>(); 
            //Itest.HelloWord();              <div id="main-container">   <span class="f2">
            //  System.Type[] types = ii.GetTypes();
            //object[] customAttributes=  types[6].GetCustomAttributes(true);
            //if (customAttributes != null)
            //{
            //    object[] array = customAttributes;
            //    for (int i = 0; i < array.Length; i++)
            //    {
            //        System.Attribute attribute = (System.Attribute)array[i];
            //        if (attribute.GetType() == this._searchAttribute)
            //        {
            //            pFindAttribute = attribute;
            //            //return true;
            //        }
            //    }
            //}
            //string ttt = sb.ToString();



            //CommTool.ConfigManager ma = new ConfigManager(@"C:\HRSource\V5.1.6(Standard)\8101224567-世代流通資\Export\appSettings.config","",true);


            //WebInfo.GetSite Site = new GetSite(@"D:\Test\LogText");
            //StreamReader Sr = Site.GetWebInfo(@"http://www.taifex.com.tw/chinese/3/7_9.asp");
            //string end = Sr.ReadToEnd();


            //WebInfo.WebInfo Web = new WebInfo.WebInfo(@"D:\Test\LogText");

            //string result = Web.GetStringResponse(@"http://www.taifex.com.tw/chinese/3/7_9.asp", Encoding.UTF8);

            //HtmlAgilityPack.HtmlDocument _HtmlDocument = GetWebHtmlDocument(@"http://www.taifex.com.tw/chinese/3/7_9.asp",  Encoding.UTF8);

            //HtmlAgilityPack.HtmlNodeCollection anchors = _HtmlDocument.DocumentNode.SelectNodes("111");

            //HtmlAgilityPack.HtmlNodeCollection anchors = _HtmlDocument.DocumentNode.SelectNodes(Default.ItemListTag);
            //string address = "http://118.99.187.60/wordpress/api_beauty.php";

            //List<SiteInfo> SiteInfoList = new List<SiteInfo>();

            //SiteInfo info = new SiteInfo();
            //info.Address = "https://www.ptt.cc/bbs/Beauty/M.1470846108.A.25E.html";
            //info.Author = "作者</span><span class='article-meta-value'>yoyonigo (呦呦二號)";
            //info.Context = "<a href='http://i.imgur.com/gg4iZXS.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/gg4iZXS.jpg</a><div class='richcontent'><img src='//i.imgur.com/gg4iZXS.jpg' data-original='//i.imgur.com/gg4iZXS.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/OyPMdM3.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/OyPMdM3.jpg</a><div class='richcontent'><img src='//i.imgur.com/OyPMdM3.jpg' data-original='//i.imgur.com/OyPMdM3.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/U8gRYV4.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/U8gRYV4.jpg</a><div class='richcontent'><img src='//i.imgur.com/U8gRYV4.jpg' data-original='//i.imgur.com/U8gRYV4.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/zUlHueY.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/zUlHueY.jpg</a><div class='richcontent'><img src='//i.imgur.com/zUlHueY.jpg' data-original='//i.imgur.com/zUlHueY.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/zoPgrn3.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/zoPgrn3.jpg</a><div class='richcontent'><img src='//i.imgur.com/zoPgrn3.jpg' data-original='//i.imgur.com/zoPgrn3.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/mPqhI4L.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/mPqhI4L.jpg</a><div class='richcontent'><img src='//i.imgur.com/mPqhI4L.jpg' data-original='//i.imgur.com/mPqhI4L.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/Yzykl9T.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/Yzykl9T.jpg</a><div class='richcontent'><img src='//i.imgur.com/Yzykl9T.jpg' data-original='//i.imgur.com/Yzykl9T.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/GvkBkYs.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/GvkBkYs.jpg</a><div class='richcontent'><img src='//i.imgur.com/GvkBkYs.jpg' data-original='//i.imgur.com/GvkBkYs.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/9co2EDG.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/9co2EDG.jpg</a><div class='richcontent'><img src='//i.imgur.com/9co2EDG.jpg' data-original='//i.imgur.com/9co2EDG.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/gfdc2nN.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/gfdc2nN.jpg</a><div class='richcontent'><img src='//i.imgur.com/gfdc2nN.jpg' data-original='//i.imgur.com/gfdc2nN.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/UgoVQKu.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/UgoVQKu.jpg</a><div class='richcontent'><img src='//i.imgur.com/UgoVQKu.jpg' data-original='//i.imgur.com/UgoVQKu.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/3uxGapf.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/3uxGapf.jpg</a><div class='richcontent'><img src='//i.imgur.com/3uxGapf.jpg' data-original='//i.imgur.com/3uxGapf.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/Q4kWy06.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/Q4kWy06.jpg</a><div class='richcontent'><img src='//i.imgur.com/Q4kWy06.jpg' data-original='//i.imgur.com/Q4kWy06.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/tORruW0.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/tORruW0.jpg</a><div class='richcontent'><img src='//i.imgur.com/tORruW0.jpg' data-original='//i.imgur.com/tORruW0.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/zVrCjAh.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/zVrCjAh.jpg</a><div class='richcontent'><img src='//i.imgur.com/zVrCjAh.jpg' data-original='//i.imgur.com/zVrCjAh.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/VZzFC6p.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/VZzFC6p.jpg</a><div class='richcontent'><img src='//i.imgur.com/VZzFC6p.jpg' data-original='//i.imgur.com/VZzFC6p.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/FDWHOKA.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/FDWHOKA.jpg</a><div class='richcontent'><img src='//i.imgur.com/FDWHOKA.jpg' data-original='//i.imgur.com/FDWHOKA.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/2Bw4c3y.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/2Bw4c3y.jpg</a><div class='richcontent'><img src='//i.imgur.com/2Bw4c3y.jpg' data-original='//i.imgur.com/2Bw4c3y.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/JU6Y1Rm.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/JU6Y1Rm.jpg</a><div class='richcontent'><img src='//i.imgur.com/JU6Y1Rm.jpg' data-original='//i.imgur.com/JU6Y1Rm.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/UZqTnKi.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/UZqTnKi.jpg</a><div class='richcontent'><img src='//i.imgur.com/UZqTnKi.jpg' data-original='//i.imgur.com/UZqTnKi.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/TWUB1wl.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/TWUB1wl.jpg</a><div class='richcontent'><img src='//i.imgur.com/TWUB1wl.jpg' data-original='//i.imgur.com/TWUB1wl.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/abzORYR.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/abzORYR.jpg</a><div class='richcontent'><img src='//i.imgur.com/abzORYR.jpg' data-original='//i.imgur.com/abzORYR.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/Zm51EDw.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/Zm51EDw.jpg</a><div class='richcontent'><img src='//i.imgur.com/Zm51EDw.jpg' data-original='//i.imgur.com/Zm51EDw.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/DYEmKLl.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/DYEmKLl.jpg</a><div class='richcontent'><img src='//i.imgur.com/DYEmKLl.jpg' data-original='//i.imgur.com/DYEmKLl.jpg' class='lazy' alt='' /></div><a href='http://i.imgur.com/DM6sEBw.jpg' target='_blank' rel='nofollow'>http://i.imgur.com/DM6sEBw.jpg</a><div class='richcontent'><img src='//i.imgur.com/DM6sEBw.jpg' data-original='//i.imgur.com/DM6sEBw.jpg' class='lazy' alt='' /></div>";
            //info.Title = "[正324234234234- 批踢踢實業坊";
            ////info.PostDate = DateTime.Now;
            //SiteInfoList.Add(info);

            //Web.GetBueatyDirtory("https://www.ptt.cc/bbs/Beauty/M.1478850577.A.35F.html", 0, "http://robertlan.16mb.com/api_beauty.php");


            //ToolLog.Log(Default.PostMessage + info.Title);
            //PostData Data = new PostData();
            //Data.Author = SiteInfoList[Default.MinItem].Author;
            //Data.Content = SiteInfoList[Default.MinItem].Context;
            //Data.Title = SiteInfoList[Default.MinItem].Title;
            //string strJson = string.Format( "json=" + JsonConvert.SerializeObject(info, Newtonsoft.Json.Formatting.Indented));
            ////string FullPath = Address +strJson;


            //string result = Web.GetStringResponse("https://www.ptt.cc/bbs/Beauty/M.1478850577.A.35F.html", Encoding.UTF8);
                        
            //Encoding myEncoding = Encoding.GetEncoding("gb2312");
            //address = address + System.Web.HttpUtility.UrlEncode(strJson, myEncoding);
            //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(address);
            //req.Method = "GET";
            //using (WebResponse wr = req.GetResponse()) {
            //    //在這裡對接收到的頁面內容進行處理
            //}            

            //string QueryString = string.Format("{0}?Author={1}&Context={2}&Title={3}",address,info.Author, info.Context,info.Title);

            //string QueryString = Newtonsoft.Json.JsonConvert.SerializeObject(info, Newtonsoft.Json.Formatting.Indented);
            //Web.GetResponse(QueryString.Replace("'","\""));
            //Web.POST(address, SiteInfoList);

            //Web.BueatyPostContentByGetMothed(address, SiteInfoList);


            //List<Option> OptionsList = new List<Option>();
            //Stock.StockData stock = new Stock.StockData();
            //for (int i = 0; i <= 2; i++) {
            //    DateTime NowDate = DateTime.Now.AddMonths(i);
            //    List<Option> options = stock.GetOptionDaily("https://tw.screener.finance.yahoo.net/future/aa03?fumr=futurepart&opmr=optionpart&opcm=WTXO&opym=" + NowDate.ToString("yyyyMM"), Encoding.UTF8);
            //    OptionsList.AddRange(options);
            //}
            //stock.SaveOpionData(OptionsList);

            //stock.SaveOptionHistroy(@"D:\Test\BBA63A05-4767-47BC-AE1F-B3380F71925C.csv");

            //PropertyInfo[] infos = typeof(Stock.Weighted).GetProperties();
            //DataTable dt = new DataTable();
            //foreach (PropertyInfo info in infos) {
            //    dt.Columns.Add(info.Name);
            //}

            //for (int i = 1; i <= 12; i++) {
            //    dt = CommTool.Files.ReadCSV(string.Format(@"D:\Test\MI_5MINS_HIST100{0}.csv", i.ToString("00")), dt);
            //}
            //stock.SaveWeighted(dt);

            //stock.GetOptionWeek(1, 8);
            //Console.WriteLine("統計結束");
            //Console.ReadLine();
                         
            CommTool.Files.DownLoadFile("https://www.taifex.com.tw/3_2_3_getcontract.asp?date1=" + DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + "&date2=" + DateTime.Now.ToString("yyyy/MM/dd") + "", @"D:\Test\TestDownLoad.csv");


            


        }

       


       


        /// <summary>取得網站的HtmlDocument</summary>
        /// <param name="ppweburl"></param>
        /// <returns></returns>
        //private static  HtmlDocument GetWebHtmlDocument(string ppweburl, Encoding Encode) {
        //    HttpWebRequest MyHttpWebRequest = HttpWebRequest.Create(ppweburl) as HttpWebRequest;
        //    HttpWebResponse MyHttpWebResponse = MyHttpWebRequest.GetResponse() as HttpWebResponse;
        //    StreamReader myStreamReader = new StreamReader(MyHttpWebResponse.GetResponseStream(), Encode);
        //    HtmlAgilityPack.HtmlDocument _HtmlDocument = new HtmlAgilityPack.HtmlDocument();
        //    _HtmlDocument.LoadHtml(myStreamReader.ReadToEnd());
        //    myStreamReader.Close();
        //    MyHttpWebResponse = null;
        //    MyHttpWebRequest = null;
        //    return _HtmlDocument;
        //}


        private static void GetAnalysis(StringBuilder sb, string context, string resul) {
            if (resul.IndexOf("http") == -1) {
                if (resul.IndexOf("<a href") == -1) {
                    string[] array = context.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    bool Into = false;

                    foreach (string str in array) {
                        if (str.IndexOf("class=\"article-meta-tag\">時間</span><span") != -1) {
                            Into = true;
                            continue;
                        }
                        if (str.IndexOf("class=\"f2\">※") != -1) {
                            Into = false;
                        }

                        if (Into) {
                            sb.AppendFormat("{0}\r\n", str);
                        }
                    }
                }
            }
        }
    }

    public class TestAddin : IServiceProviderAddin {
        IServiceEntry[] _serviceEntries;
        public IServiceEntry[] ServiceEntries {
            get {
                return _serviceEntries;
            }
        }
    }

}

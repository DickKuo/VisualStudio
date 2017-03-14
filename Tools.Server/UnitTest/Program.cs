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


            WebInfo.WebInfo Web = new WebInfo.WebInfo(@"D:\Test\LogText");

            //string result = Web.GetStringResponse(@"http://www.taifex.com.tw/chinese/3/7_9.asp", Encoding.UTF8);

            //HtmlAgilityPack.HtmlDocument _HtmlDocument = GetWebHtmlDocument(@"http://www.taifex.com.tw/chinese/3/7_9.asp",  Encoding.UTF8);

            //HtmlAgilityPack.HtmlNodeCollection anchors = _HtmlDocument.DocumentNode.SelectNodes("111");

            //HtmlAgilityPack.HtmlNodeCollection anchors = _HtmlDocument.DocumentNode.SelectNodes(Default.ItemListTag);
            //string address = "http://118.99.187.60/wordpress/api_beauty.php";


            //Web.GetBueatyDirtory("https://www.ptt.cc/bbs/Beauty/M.1486780171.A.9C3.html", 0, "http://robertlan.16mb.com/api_beauty.php");

            Stock.StockData data = new StockData();
            decimal result=  data.CalculateStopPrice(decimal.Parse("12.5"),"9550",decimal.Parse("9650"));


            Console.Read();
            //ToolLog.Log(Default.PostMessage + info.Title);
            //PostData Data = new PostData();
            //Data.Author = SiteInfoList[Default.MinItem].Author;
            //Data.Content = SiteInfoList[Default.MinItem].Context;
            //Data.Title = SiteInfoList[Default.MinItem].Title;
            //string strJson = string.Format( "json=" + JsonConvert.SerializeObject(info, Newtonsoft.Json.Formatting.Indented));
            ////string FullPath = Address +strJson;

            //LineMessage Line = new LineMessage();

            //Message msg = new Message();
            //msg.type = "text";
            //msg.text = "TryText";
            //Line.to = "dick22707";
            //Line.messages.Add(msg);




            //Web.HttpPostMethod(Newtonsoft.Json.JsonConvert.SerializeObject(Line), "https://api.line.me/v2/bot/message/push");
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
            Stock.CalendarData CalendarDB = new CalendarData();
            //Stock.StockData data=new StockData();
            for (int i = 14; i <= 19;i++ ) {
                Calendar _Calendar = CalendarDB.GetCalendar(new DateTime(2017, 12, i));
                _Calendar.Week = "201712";
                _Calendar.NearMonth1 = "201801";
                _Calendar.NearMonth2 = "201802";
                CalendarDB.UpdateCalendar(_Calendar);
            }
            //data.UpdateWeekPointEndPrice("201703W2","call");
             

            //Stock.StockData stock = new Stock.StockData();
            //List<Option> OptionsList = new List<Option>();
            //LineClient clinent = new LineMessagingAPISDK.LineClient("x9D+Uh67zOnuoqYZBXk4QsCuTLDURikSHVz0kN7XbbVCdbZKz1Rjb6qAuNbSckcpODTpWMOFLPZUyY7cpgpwC0TBCsgK0XeUS/fYSl4U9Fs9p2yKMlp2BapJNdQI97RAl1N1OHjDAX8yaxHyDj2iXwdB04t89/1O/w1cDnyilFU=");
            //PushMessage PMessage = new LineMessagingAPISDK.Models.PushMessage();
            //PMessage.To = "dick22707";

            //TextMessage txMessage = new LineMessagingAPISDK.Models.TextMessage("Test");
            //txMessage.Id = "dick22707";
            //txMessage.Text = "Hello www";

            //PMessage.Messages.Add(txMessage);

            //clinent.PushAsync(PMessage);


            //Web.HttpPostMethod("key=76e2db97-a330-4418-a6bf-f26072695969&id=dick22707&msg=test","https://isbaas.azurewebsites.net/api/LineWebHook");
            //stock.GetOptionWeekWithStop(1,8,1);

             //stock.GetNumberOfContractsAndMaill();
            //for (int i = 0; i <= 2; i++) {
            //    DateTime NowDate = DateTime.Now.AddMonths(i);
            //    List<Option> options = stock.GetOptionDaily("https://tw.screener.finance.yahoo.net/future/aa03?fumr=futurepart&opmr=optionpart&opcm=WTXO&opym=" + NowDate.ToString("yyyyMM"), Encoding.UTF8);
            //  
            //}

            //List<Option> options = stock.GetOptionDaily("https://tw.screener.finance.yahoo.net/future/aa03?fumr=futurepart&opmr=optionpart&opcm=WTXO&opym=", "201702", Encoding.UTF8);
            //OptionsList.AddRange(options);

            //stock.SaveOpionData(OptionsList);

            //stock.SaveOptionHistroy(@"D:\Test\BBA63A05-4767-47BC-AE1F-B3380F71925C.csv");

            //PropertyInfo[] infos = typeof(Stock.HistoryOption).GetProperties();
            //DataTable dt = new DataTable();
            //foreach (PropertyInfo info in infos) {
            //    dt.Columns.Add(info.Name);
            //}

            //for (int i = 1; i <= 12; i++) {
            // dt = CommTool.Files.ReadCSV(string.Format(@"D:\Test\MI_5MINS_HIST100{0}.csv", i.ToString("00")), dt);
            //}
            //stock.SaveWeighted(dt);



            //CommTool.MailData _MailData = new MailData();


            //dt = CommTool.Files.ReadCSV(@"D:\Test\201612.csv", dt);

            //stock.SaveOptionHistoryData(dt);

            //WebInfo.MotorData Motor = new WebInfo.MotorData();
            //Motor.GetMotorData();
            //for (int i =23; i <= 28; i++) {
            //DateTime Spa = new DateTime(2017, 2, 22);
            //Calendar _Calendar = CalendarDB.GetCalendar(Spa);
            //    //_Calendar.IsWorkDay = true;
            //    _Calendar.Week = "201702W5";
            //_Calendar.NearMonth1 = "201703W1";
            //    _Calendar.NearMonth2 = "201704";

            //    if (i == 22) {
            //        _Calendar.Week = "201702W4";
            //        _Calendar.NearMonth1 = "201702W5";
            //        _Calendar.NearMonth2 = "201703";
            //    }

            //CalendarDB.UpdateCalendar(_Calendar);
            //}
            //stock.GetOptionEveryDay("https://tw.screener.finance.yahoo.net/future/aa03?fumr=futurepart&opmr=optionpart&opcm=WTXO&opym=");

            //Weighted _Weighted = stock.GetWeightedDaily("https://tw.screener.finance.yahoo.net/future/aa03?fumr=futurepart&opmr=optionpart&opcm=WTXO&opym=");
            //stock.SaveWeighted(_Weighted);
            //int week = 1;
            //for (int i = 2; i <= 31; i++) {
            //    DateTime Dat = new DateTime(2017, 3, i);
            //    Calendar _Calendar = CalendarDB.GetCalendar(Dat);

            //    switch (week)
            //    {
            //        case 1:
            //        case 2:
            //            _Calendar.Week = string.Format("{0}{1}W{2}", Dat.Year, Dat.Month,week);
            //            _Calendar.NearMonth1 = string.Format("{0}{1}", Dat.Year, Dat.Month);
            //            _Calendar.NearMonth2 = string.Format("{0}{1}", Dat.AddMonths(1).Year, Dat.AddMonths(1).Month);
            //            break;
            //        case 4:
            //        case 5:
            //            _Calendar.Week = string.Format("{0}{1}W{2}", Dat.Year, Dat.Month,week);
            //            _Calendar.NearMonth1 = string.Format("{0}{1}", Dat.AddMonths(1).Year, Dat.AddMonths(1).Month);
            //            _Calendar.NearMonth2 = string.Format("{0}{1}", Dat.AddMonths(2).Year, Dat.AddMonths(2).Month);
            //            break;
            //    }                

            //    if (week == 3) {
            //        _Calendar.Week = string.Format("{0}{1}", Dat.Year, Dat.Month);
            //        _Calendar.NearMonth1 = string.Format("{0}{1}", Dat.AddMonths(1).Year, Dat.AddMonths(1).Month);
            //        _Calendar.NearMonth2 = string.Format("{0}{1}", Dat.AddMonths(2).Year, Dat.AddMonths(2).Month);
            //    }
                

            //    if (Dat.DayOfWeek.ToString() == "Wednesday") {
            //        if (week == 1) {
            //            _Calendar.NearMonth1 = string.Format("{0}{1}W2", Dat.Year, Dat.Month);
            //            _Calendar.NearMonth2 = string.Format("{0}{1}", Dat.AddMonths(1).Year, Dat.AddMonths(1).Month);
            //        }
            //        if (week == 3) {
            //            _Calendar.NearMonth1 = string.Format("{0}{1}W4", Dat.Year, Dat.Month);
            //            _Calendar.NearMonth2 = string.Format("{0}{1}", Dat.AddMonths(1).Year, Dat.AddMonths(1).Month);
            //        }
            //        if (week == 4) {
            //            _Calendar.NearMonth1 = string.Format("{0}{1}W5", Dat.Year, Dat.Month);
            //            _Calendar.NearMonth2 = string.Format("{0}{1}", Dat.AddMonths(1).Year, Dat.AddMonths(1).Month);
            //        }
            //        week++;
            //    }
            //        //_Calendar.Remark = "周選結算";


            //    CalendarDB.UpdateCalendar(_Calendar);
            //}


            //stock.GetOptionWeekWithStop(1, 8, 1);
            
            //Console.WriteLine("統計結束");
            //Console.ReadLine();


            //for (int i = 1; i <= 12; i++) {
            //    stock.SaveOpen_InterestHistory(string.Format(@"D:\Test\Ratios2016_{0}.csv",i.ToString("00")));
            //} 
                         
            ////CommTool.Files.DownLoadFile("https://www.taifex.com.tw/3_2_3_getcontract.asp?date1=" + DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + "&date2=" + DateTime.Now.ToString("yyyy/MM/dd") + "", @"D:\Test\TestDownLoad.csv");
            //object username = new object();
            //CommTool.ObjectUtility.ReadRegistry("UserName", ref username);
             
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

    public class LineMessage {

        public string to { set; get; }

        public List<Message> messages = new List<Message>();
    }


    public class Message {
       public string type { set; get; }

       public string text { set; get; }
    }

}

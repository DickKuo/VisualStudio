using GetSite;
//using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions; 

namespace Reptile
{
    public class CRMResult {
        public int SN { set; get; }

        public bool Result { set; get; }

        public int ErrorCode { set; get; }
    }

    class Program
    {
        public class Default
        {
            public const int BaseTimeYear = 1970;
            public const int BaseTimeMonth = 01;
            public const int BaseTimeDay = 01;
        }

        static int Tag { set; get; }

        static int PushCount { set; get; }

        static DateTime RecordTime { set; get; }

        static List<string> RecordList = new List<string>();

        static int ChangePoint = 0;

        static void Main(string[] args)
        {
            string Keyin = "1"; //Console.ReadLine();

            if (!string.IsNullOrEmpty(Keyin))
            {
                foreach (var file in Directory.GetFiles(@"C:\Users\Administrator\Desktop\新增資料夾 (4)"))
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        string content = reader.ReadToEnd();

                        string DecodeContent = s52d(content);


                       

                    }
                }
            }

            //OptionTest();
            //Stock.StockDAO DAO = new Stock.StockDAO();
            //DAO.RefreshList();

        }//end Main

        static void s52f()
        {
            N = s52s.Length;
            N2 = N * N;
            for (var x = 0; x < s52s.Length; x++)
            {
                s52r[(int)s52s[x]] = x;
            }
            s52t = false;
        }

        public static string s52s = "8ABC7DLO5MN6Z9EFGdeJfghijkHIVrstuvwWSTUXYabclmnopqKPQRxyz01234";
        static bool s52t = true;
        static int N, N2;
        static int[] s52r = new int[128];

        private static string s52e(string n)
        {
            if (s52t) s52f();
            int l = n.Length, a, x = 0;
            List<char> t = new List<char>(l * 3);
            for (; x < l; x++)
            {
                a = (int)n[x];
                if (a < N2)
                {
                    t.Add(s52s[a / N]);
                    t.Add(s52s[a % N]);
                }
                else
                {
                    t.Add(s52s[a / N2 + 5]);
                    t.Add(s52s[(a / N) % N]);

                    t.Add(s52s[a % N]);
                }
            }
            string s = new string(t.ToArray());
            return s.Length.ToString().Length + s.Length.ToString() + s;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string s52d(string n)
        {
            if (s52t) s52f();
            int c;
            if (!int.TryParse(n[0].ToString(), out c)) return "";
            if (!int.TryParse(n.Substring(1, c), out c)) return "";
            int x = c.ToString().Length + 1;
            if (n.Length != c + x) return "";
            int nl = n.Length, a;
            List<char> t = new List<char>(nl * 3);
            for (; x < nl; x++)
            {
                a = s52r[(int)n[x]];
                x++;
                //if (a < 5)
                if ((a * N + s52r[(int)n[x]]) / N == a)
                {
                    c = a * N + s52r[(int)n[x]];
                }
                else
                {
                    c = (a - 5) * N2 + s52r[(int)n[x]] * N;
                    x++;
                    c += s52r[(int)n[x]];
                }
                t.Add((char)c);
            }
            return new string(t.ToArray());
        }


        private static void USG()
        {

            // "http://localhost:47656/";  //;

            string host = "https://www.usgfx.com/usgapiserver/";
            StringBuilder ErrorCodeSb = new StringBuilder();
            WebInfo.WebInfo web = new WebInfo.WebInfo();
            StringBuilder sb = new StringBuilder();

            foreach (string line in File.ReadAllLines(@"D:\CompanyAsy\Temp\CreateUser.txt"))
            {
                string[] SpLine = Regex.Split(line, "\t", RegexOptions.IgnoreCase);
                string SN = SpLine[0];
                //string Account = SpLine[1];
                //string ClientSource = SpLine[2];

                string result = web.HttpPostMethod(string.Format("SN={0}", SN), host + "OutSideAPI/CreateUser");
                string str = string.Format("Account={0}___{1}", line, result);
                sb.AppendLine(str);
                Console.WriteLine(str);
                Thread.Sleep(200);

                try
                {
                    CRMResult C_Result = Newtonsoft.Json.JsonConvert.DeserializeObject<CRMResult>(result);
                    if (C_Result.ErrorCode == 99 || C_Result.ErrorCode == 0)
                    {
                        result = web.HttpPostMethod(string.Format("SN={0}", SN), host + "OutSideAPI/UpdateCustomerApproveStatus");
                        str = string.Format("{0}", result);
                        sb.AppendLine(str);
                        Console.WriteLine(str);
                        Thread.Sleep(200);
                    }//end C_Result.ErrorCode
                }
                catch (Exception ex)
                {

                }//end try

            }//end foreach            

            //Adviser(host, ErrorCodeSb, web, sb);
            System.IO.File.WriteAllText(@"D:\CompanyAsy\Temp\Result.txt", sb.ToString(), System.Text.Encoding.UTF8);
            System.IO.File.WriteAllText(@"D:\CompanyAsy\Temp\ResultError.txt", ErrorCodeSb.ToString(), System.Text.Encoding.UTF8);
            Console.WriteLine("End");
            Console.Read();
        }

        private static void Adviser(string host, StringBuilder ErrorCodeSb, WebInfo.WebInfo web, StringBuilder sb)
        {
            for (int i = 0; i <= 3; i++)
            {
                foreach (string line in File.ReadAllLines(@"D:\CompanyAsy\Temp\ErrorAdviserList.txt"))
                {
                    string result = web.HttpPostMethod(string.Format("AdviserSN={0}", line), host + "OutSideAPI/CreateSalesOrIB");
                    string str = string.Format("AdviserNo={0}___{1}", line, result);

                    sb.AppendLine(str);
                    Console.WriteLine(str);
                    Thread.Sleep(200);
                    try
                    {
                        CRMResult C_Result = Newtonsoft.Json.JsonConvert.DeserializeObject<CRMResult>(result);
                        if (C_Result.ErrorCode == 99 || C_Result.ErrorCode == 0)
                        {
                            result = web.HttpPostMethod(string.Format("AdviserSN={0}", line), host + "OutSideAPI/UpdateIBApproveStatus");
                            str = string.Format("UpdateIBApproveStatus :  {0} ", result);
                            sb.AppendLine(str);
                            Console.WriteLine(str);

                            Thread.Sleep(200);
                            CRMConflict cRM = new CRMConflict();
                            CRMRequest cRMRequest = new CRMRequest();

                            double TotalSeconds = DateTime.UtcNow.AddHours(8).Subtract(new DateTime(Default.BaseTimeYear, Default.BaseTimeMonth, Default.BaseTimeDay)).TotalSeconds;
                            string TimeStamp = TotalSeconds.ToString();

                            cRMRequest.IBCode = line;
                            cRMRequest.timestamp = TimeStamp;
                            string feature = GetSHA256Enc(cRMRequest.IBCode + "phenixAdmin" + cRMRequest.timestamp);
                            cRMRequest.feature = feature;
                            string JsonData = Newtonsoft.Json.JsonConvert.SerializeObject(cRMRequest);

                            result = web.HttpPostMethod(string.Format("{0}", JsonData), host + "OutSideAPI/StuffCRMIBAndCustomer");

                            str = string.Format("StuffCRMIBAndCustomer=> {0}", result);
                            sb.AppendLine(str);
                            Console.WriteLine(str);
                            Thread.Sleep(200);
                        }
                        else
                        {
                            ErrorCodeSb.AppendLine(result);
                        }
                        Thread.Sleep(200);
                    }
                    catch
                    {
                    }
                }//end foreach
            }//end for
        }


        /// <summary>取得經SHA256加密過的字串</summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static string GetSHA256Enc(string Value)
        {
            byte[] source = Encoding.Default.GetBytes(Value);//將字串轉為Byte[]
            using (SHA256 sha256 = SHA256Managed.Create())
            {
                byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
                return Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串
            }
        }


        /// <summary>選擇權模擬</summary>
        private static void OptionTest()
        {
            Stock.WeekPoint WPoint = new Stock.WeekPoint();
            Stock.StockDAO dao = new Stock.StockDAO();
            Stock.TradeRecordDAO RecordDAO = new Stock.TradeRecordDAO();
            decimal MyAccount = 100000;

            List<Stock.WeekPoint> ListWeek = dao.GetAllWeekPointByYear(2019);
            Stock.WeekPoint WeekPointFirst = ListWeek[0];
            Stock.WeekPoint WeekPointLast = ListWeek[ListWeek.Count - 1];

            List<Stock.TradeRecord> ListTradeRecord = RecordDAO.GetListTradeRecordByDueDay(WeekPointFirst.TradeDate.ToString("yyyy/MM/dd"), WeekPointLast.TradeDate.ToString("yyyy/MM/dd"));


            List<Stock.TradeRecord> CheckList = new List<Stock.TradeRecord>();

            int Index = 24;

            var ListRe = ListWeek.GetRange(Index, (ListWeek.Count - 1) - Index);

            //CheckList.Add(ListTradeRecord[23]);
            //ListTradeRecord[24].Type = "Sell";
            //CheckList.Add(ListTradeRecord[24]);
            //CheckList.Add(ListTradeRecord[25]);
            //CheckList.Add(ListTradeRecord[26]);
            //CheckList.Add(ListTradeRecord[27]);

            int count = 0;

            decimal tempPrice = 0;

            foreach (var itemRecord in ListRe)
            {
                WPoint.OP = itemRecord.OP;
                WPoint.Contract = itemRecord.Contract;
                WPoint.DueMonth = itemRecord.DueMonth;

                Stock.WeightedDAO weightedda = new Stock.WeightedDAO();

                DateTime begin = new DateTime();
                DateTime end = new DateTime();

                Stock.TradeRecord Record = new Stock.TradeRecord();

                Record.OP = itemRecord.OP;
                Record.Contract = itemRecord.Contract;
                Record.DueMonth = itemRecord.DueMonth;
                Record.Type = "Sell";
                Record.TradeDate = itemRecord.TradeDate;
                Record.Price = Convert.ToDecimal(itemRecord.Price);

                //Stock.TradeRecord Record = RecordDAO.GetTradeRecordBySN(itemRecord.SN);

                Console.WriteLine("Loading....");

                List<Stock.Option> OptionList = dao.GetListOptionByWeekPoint(WPoint);

                Stock.Option LastOption = new Stock.Option();
                if (OptionList.Count > 0)
                {
                    Stock.Option firstOption = OptionList[0];
                    LastOption = OptionList[OptionList.Count - 1];
                    begin = Convert.ToDateTime(firstOption.Time);
                    end = Convert.ToDateTime(LastOption.Time);
                }

                List<Stock.Weighted> WeighList = weightedda.GetWeightedByDay(begin, end);

                //count++;
                if (WeighList.Count == 0)
                {
                    Console.WriteLine(string.Format("WeighList count is zero .... pass this weekpoint :{0} , {1} , {2} ", WPoint.OP, WPoint.DueMonth, WPoint.Contract));
                    continue;
                }
                Console.WriteLine("Start Printing ");
                StringBuilder Sb = new StringBuilder();

                foreach (var item in OptionList)
                {
                    if (Convert.ToDateTime(item.Time) > Record.TradeDate)
                    {
                        try
                        {
                            string PrintString = string.Format("OP={0} , Clinch={1} , time ={2} , Volume ={3} , DueMonth={4} , NumberOfContracts ={5} ",
                                item.OP, item.Clinch, item.Time, item.Volume, item.DueMonth, item.NumberOfContracts);

                            Sb.AppendLine(PrintString);
                            Console.WriteLine(PrintString);
                            var wei = WeighList.Select(x => x.TradeTimestamp = item.TradeTimestamp);
                            dao.CalculateStopTestPoint(RecordDAO, Record, WeighList[2], item, Sb, begin, end, ref MyAccount, ref tempPrice);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                using (StreamWriter SW = new StreamWriter(string.Format(@"C:\Log\{0}_{1}ChangeTimes{2}.txt", WPoint.DueMonth, WPoint.OP, Record.ChangeTimes)))
                {
                    SW.Write(Sb.ToString());
                    SW.Close();
                    SW.Dispose();
                }

                if (Record.Type == "Sell")
                {
                    MyAccount += (Record.Price - LastOption.Clinch) * 50 - 18;
                }
                else
                {
                    MyAccount += ((LastOption.Clinch - Record.Price) * 50 - 18) * 5;
                }

                Console.WriteLine(string.Format("New Amount:{0}", MyAccount));



                Thread.Sleep(3120);

                //XmlDocument doc = FileTool.XmlFile.LoadXml(Directory.GetCurrentDirectory() + "\\" + "Record.xml");
                //XmlNodeList NodeList = doc.SelectNodes("/root/Title");
                //foreach(XmlNode node in NodeList)
                //{
                //    RecordList.Add(node.InnerText);
                //}
                //while(true) {
                // Console.WriteLine("啟動請按r");
                //int key = Console.Read();
                //if(key.Equals(114))
                //Start();
            }
            Console.WriteLine(string.Format("End Amount:{0}", MyAccount));
            Console.Read();
            //}
        }

        public static string Address { set; get; }
        private static void Start()
        {
            GetSite.GetSite site = new GetSite.GetSite(@"C:\Users\Dick\Desktop\22");
            XmlDocument doc = FileTool.XmlFile.LoadXml(Directory.GetCurrentDirectory() + "\\" + "Config.xml");
            XmlNode PostAddress = doc.SelectSingleNode(@"/root/PostAddress");
            XmlNode NodeCondition = doc.SelectSingleNode(@"/root/Condition");
            XmlNode NodeSite = doc.SelectSingleNode(@"/root/Site");
            XmlNode NodeStart = doc.SelectSingleNode(@"/root/Start");
            XmlNode NodePushCount = doc.SelectSingleNode(@"/root/PushCount");
            XmlNode NodeRecordTime = doc.SelectSingleNode(@"/root/ReCordTime");

            RecordTime = Convert.ToDateTime(NodeRecordTime.InnerText);
            PushCount = Convert.ToInt32(NodePushCount.InnerText);
            string Condition = NodeCondition.InnerText;
            Address = PostAddress.InnerText;
            Tag = Convert.ToInt32(NodeStart.InnerText);
            string Site = NodeSite.InnerText;
            SitePlus siteplus = site.GetUrlList("https://www.ptt.cc/bbs/" + Site + "/index" + Tag + ".html");
            List<SiteInfo> SiteInfoList = new List<SiteInfo>();
            Recursive(Tag, siteplus, SiteInfoList, Site, "/bbs/" + Site + "/index", Condition);             //"SiteInfoList=" + 

            //List<SiteInfo> temp = new List<SiteInfo>();          
            //foreach (SiteInfo info in SiteInfoList)
            //{
            //    temp.Add(info);

            //    //if (temp.Count > 9)
            //    //{
            //        //Console.WriteLine(info.Title);
            //        //Console.WriteLine(info.PostDate);
            //        //Console.Write(info.Context);
            //        long length = POST(Address, temp);

            //        temp.Clear();
            //    //}
            //}

            //long length = POST(Address, SiteInfoList);

            //if(length > 0)
            //Console.WriteLine("OK");
            //NodeStart.InnerText = Tag.ToString();
            //NodeRecordTime.InnerText = RecordTime.ToString("yyyy/MM/dd HH:mm:ss");
            XmlDocument Recorddoc = FileTool.XmlFile.LoadXml(Directory.GetCurrentDirectory() + "\\" + "Record.xml");
            XmlNode root = doc.SelectSingleNode("root");
            foreach (string str in RecordList)
            {
                XmlElement element = Recorddoc.CreateElement("Title");
                element.InnerText = str;
                root.AppendChild(element);
            }
            Recorddoc.Save(Directory.GetCurrentDirectory() + "\\" + "Record.xml");

            doc.Save(Directory.GetCurrentDirectory() + "\\" + "Config.xml");
        }

        private static long POST(string Address, List<SiteInfo> SiteInfoList)
        {
            //"SiteInfoList =" +
            string strJson = "SiteInfoList=" + JsonConvert.SerializeObject(SiteInfoList, Newtonsoft.Json.Formatting.Indented);
            //byte[] postBytes = Encoding.ASCII.GetBytes(strJson);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Address);
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://dickguo.net63.net/chat/getInfo/");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] byteArray = Encoding.UTF8.GetBytes(strJson);

            //byte[] byteArray = Encoding.ASCII.GetBytes(strJson);
            request.ContentLength = byteArray.Length;


            //System.Net.WebResponse resp = request.GetResponse();
            //if(resp == null) {
            //    Console.WriteLine("Error");
            //}
            //else {
            //    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            //    Console.Write(sr.ReadToEnd().Trim());
            //}

            //request.ContentType = @"application/json";

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
                        StreamWriter sw = new StreamWriter(@"C:\Users\Dick\Desktop\Json\" + date.ToString("yyyymmddHHmmss") + ".txt");
                        sw.Write(strJson);
                        sw.Close();
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.ToString());
                // Log exception and throw as for GET example above

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
        static void Recursive(int index, SitePlus pSiteplus, List<SiteInfo> li, string Site, string Formate, string pCondition)
        {
            GetSite.GetSite site = new GetSite.GetSite(@"C:\Users\Dick\Desktop\22");
            Console.WriteLine(index.ToString());
            foreach (string str in pSiteplus.Context)
            {
                GetSite.SiteInfo info = site.GetInfo(@"https://www.ptt.cc" + str);
                if (info.Title != null)
                {
                    if (info.Title.IndexOf(pCondition) != -1 && info.PushList.Count > PushCount)
                    {
                        Thread.Sleep(1000);
                        if (info.Title.IndexOf("Re: ") == -1)
                        {
                            info.PushList.Clear();
                            li.Add(info);
                            //if (info.PostDate > RecordTime)
                            //{                           
                            if (!RecordList.Contains(info.Title))
                            {
                                long length = POST(Address, li);

                                RecordList.Add(info.Title);
                                RecordTime = info.PostDate;
                            }
                            //}
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
                        Recursive(result, pSiteplus, li, Site, Formate, pCondition);
                    }
                }
            }
        }

    }


}

﻿using System;
using CommTool;
using System.IO;
using System.Net;
using System.Linq;
using System.Data;
using System.Text;
using HtmlAgilityPack;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Stock
{
    public class StockDAO : BaseData
    {
        private class Default
        {
            public const string sqlconnection = "sqlconnection";
            public const int Second = 1000;
            public const string Html = ".html";
            public const string TableTag = "//table[@class='ext-big-tbl']";
            public const string HtmlTr = "//tr";
            public const string Call = "Call";
            public const string Put = "Put";
            public const string MomStart = "opym";
            public const string NullSing = "--";
            public const string TimeFormat = "yyyy/MM/dd";
            public const int FirstItem = 0;
            public const int SecondItem = 1;
            public const int ThirdItem = 2;
            public const string StopWarning = "停損警戒";
        }

        private class SP
        {
            public const string AddOption = "AddOption";
            public const string AddWeekPoint = "AddWeekPoint";
            public const string GetDueMonth = "GetDueMonth";
            public const string GetOptionHistory = "GetOptionHistory";
            public const string GetMaxNumberOfContracts = "GetMaxNumberOfContracts";
            public const string GetMaxVolume = "GetMaxVolume";
            public const string GetWeekPointByDueMonthAndOP = "GetWeekPointByDueMonthAndOP";
            public const string GetOptionByDueMonthAndOP = "GetOptionByDueMonthAndOP";
            public const string GetOptionByMonthAndContractAndOP = "GetOptionByMonthAndContractAndOP";
            public const string GetListOptionCondition = "GetListOptionCondition";
            public const string GetOptionQuotesByDuMonthAndTime = "GetOptionQuotesByDuMonthAndTime";
            public const string GetListOptionByWeekPoint = "GetListOptionByWeekPoint";
            public const string GetListOption = "GetListOption";
            public const string UpdateWeekPoint = "UpdateWeekPoint";
            public const string SaveOpenInterest = "SaveOpenInterest";
            public const string SaveOption = "SaveOption";
            public const string SaveOptionHistory = "SaveOptionHistory";
            public const string GetAllWeekPointByYear = "GetAllWeekPointByYear";
            public const string StoreToHistory = "StoreToHistory";
        }

        private class SPParameter
        {
            public const string OP = "OP";
            public const string Buy = "Buy";
            public const string Sell = "Sell";
            public const string Clinch = "Clinch";
            public const string Change = "Change";
            public const string Open = "Open";
            public const string Total = "Total";
            public const string Time = "Time";
            public const string Contract = "Contract";
            public const string Mom = "Mom";
            public const string Start = "Start";
            public const string End = "End";
            public const string Volume = "Volume";
            public const string NumberOfContracts = "NumberOfContracts";
            public const string DueMonth = "DueMonth";
            public const string Today = "Today";
            public const string StopPrice = "StopPrice";
            public const string BuyStopPrice = "BuyStopPrice";
            public const string TradeDate = "TradeDate";
            public const string Price = "Price";
            public const string Option = "Option";
            public const string Opening_Price = "Opening_Price";
            public const string Highest = "Highest";
            public const string Lowest = "Lowest";
            public const string Closing = "Closing";
            public const string Settlement = "Settlement";
            public const string History_Highest = "History_Highest";
            public const string History_Lowest = "History_Lowest";
            public const string GreatBuy = "GreatBuy";
            public const string GreatSell = "GreatSell";
            public const string Remark = "Remark";
            public const string Futures = "Futures";
            public const string OpenPrice = "OpenPrice";
            public const string HighestPrice = "HighestPrice";
            public const string LowestPrice = "LowestPrice";
            public const string ClosingPrice = "ClosingPrice";
            public const string SN = "SN";
            public const string PutVolume = "PutVolume";
            public const string CallVolume = "CallVolume";
            public const string Ratios = "Ratios";
            public const string PutOpenInterest = "PutOpenInterest";
            public const string CallOpenInterest = "CallOpenInterest";
            public const string OpenInterestRatios = "OpenInterestRatios";
            public const string ClosePrice = "ClosePrice";
            public const string BeginTime = "BeginTime";
            public const string EndTime = "EndTime";
            public const string TradeTimestamp = "TradeTimestamp";
            public const string OptionView = "OptionView";
            public const string Message = "Message";
            public const string AddTime = "AddTime";
            public const string Years = "Years";
        }

        private string _stockNum;

        private string _url;

        private Stock _stock;

        #region  Parameter

        public string StockNum
        {
            set
            {
                _stockNum = value;
            }
        }

        public string URL
        {
            set
            {
                _url = value;
            }
        }

        #endregion

        public StockDAO()
        {
            _stock = new Stock();
        }

        //public StockData(string pStockNum) {
        //    _stock = new Stock();
        //    StockNum = pStockNum;
        //    _stock.StockNum = this._stockNum;
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic[Default.sqlconnection] = string.Empty;
        //    SQLHelper.SHelper.InitSHelper(dic);
        //}

        public Stock GetStockData(string Url, string ConnetionString)
        {
            try
            {
                WebRequest myWebRequest = WebRequest.Create(Url + _stock.StockNum);
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
                Stream DataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(DataStream, Encoding.Default);
                string ResponseFromService = reader.ReadToEnd();
                int start = ResponseFromService.IndexOf("align=center width=105>");
                int end = ResponseFromService.IndexOf("width=137");
                int diff = end - start;
                string temp = ResponseFromService.Substring(start, diff).Replace("<td align=\"center\" bgcolor=\"#FFFfff\" nowrap>", string.Empty).Replace("</td>", string.Empty).Replace("<b>", string.Empty).Replace("</b>", string.Empty).Replace("<font color=#009900>", string.Empty).Replace("<font color=#ff0000>", string.Empty).Replace("<font color=#000000>", string.Empty);
                StringReader sr = new StringReader(temp);
                sr.ReadLine();
                sr.ReadLine();
                _stock.StockTime = Convert.ToDateTime(sr.ReadLine());
                _stock.Price = Convert.ToDecimal(sr.ReadLine());
                _stock.BuyPrice = Convert.ToDecimal(sr.ReadLine());
                string SelllPriceTemp = sr.ReadLine().Trim();
                _stock.SellPrice = Convert.ToDecimal(SelllPriceTemp == "－" ? "0" : SelllPriceTemp);
                _stock.Change = sr.ReadLine().Trim();
                _stock.Quantity = Convert.ToDecimal(sr.ReadLine());
                _stock.Yesterday = Convert.ToDecimal(sr.ReadLine());
                _stock.Start = Convert.ToDecimal(sr.ReadLine());
                _stock.Highest = Convert.ToDecimal(sr.ReadLine());
                _stock.Lowest = Convert.ToDecimal(sr.ReadLine());
                _stock.IsSucess = true;
            }
            catch (Exception ex)
            {
                #region  20150324 modifed 修改為新的方式撰寫SQL
                string sql = string.Format(@"insert into ErrorLog values({0},{1},{2},{3})", "001", ex.Message, DateTime.Now, this._stock.StockNum);
                SQLHelper.SHelper.ExeNoQuery(sql);
                #endregion
            }
            return this._stock;
        }

        public void SetkData(string ConnetionString)
        {
            if (this._stock.IsSucess)
            {
                #region  20150324 modifed 修改為新的方式撰寫SQL
                string sql = string.Format(@"Insert into StockData values({0}, {1},{2},{3}, {4},{5}, {6}, {7}, {8},{9},{10})", this._stock.StockNum,
                    this._stock.StockTime, this._stock.Price, this._stock.BuyPrice, this._stock.SellPrice, this._stock.Change, this._stock.Quantity,
                   this._stock.Yesterday, this._stock.Start, this._stock.Highest, this._stock.Lowest);
                SQLHelper.SHelper.ExeNoQuery(sql);
                #endregion
            }
        }

        /// <summary> 更新股票清單  EPS 也同步更新
        /// </summary>
        public void RefreshList()
        {
            for (int i = 1101; i < 9999; i++)
            {
                pNewRefreshList(i.ToString());
                Thread.Sleep(2 * Default.Second);
            }
        }

        private string RefreshStockList()
        {
            try
            {
                WebRequest myWebRequest = WebRequest.Create(_url + _stockNum + Default.Html);
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
                Stream DataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(DataStream, Encoding.Default);
                string ResponseFromService = reader.ReadToEnd();
                int start = ResponseFromService.IndexOf("description");
                int end = ResponseFromService.IndexOf("公司資料，查詢 ");
                int deffence = end - start;
                StringBuilder sb = new StringBuilder();
                sb.Append(ResponseFromService.Substring(start, deffence).Replace("description\" content=\"", string.Empty).Replace("(" + _stockNum + ")", string.Empty));
                sb.Append(",");
                string[] sArray = Regex.Split(ResponseFromService, "table", RegexOptions.IgnoreCase);
                string[] arry = Regex.Split(sArray[23], "tr", RegexOptions.IgnoreCase);
                for (int i = 3; i < arry.Length; i++)
                {
                    string[] parry = Regex.Split(arry[i], "td", RegexOptions.IgnoreCase);
                    if (parry.Length > 8)
                    {
                        sb.Append(parry[5].Replace("bgcolor=\"#FFFAE8\">", string.Empty).Replace("</", string.Empty).Replace("height=\"25\"", string.Empty).Trim());
                        sb.Append(parry[7].Replace("align=\"center\">", string.Empty).Replace("</", string.Empty));
                        sb.Append(",");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                CommTool.ToolLog log = new CommTool.ToolLog();
                CommTool.ToolLog.Log(CommTool.LogType.Error, _stockNum.ToString());
                ToolLog.Log(ex);
                return "error";
            }
        }

        private void pRefreshList(string pCode)
        {
            this.StockNum = pCode;
            this.URL = "https://tw.stock.yahoo.com/d/s/company_";
            string temp = this.NewRefreshStockList();
            if (temp != "error")
            {
                string[] arry = temp.Split(',');
                #region  20150324 modifed by Dick for 修改維新的方式撰寫SQL
                string sql = string.Format(@"select count(*) from StockList where StockNum={0} and EPS1={1} and EPS2={2} and EPS3 ={3} and EPS4 ={4} ", pCode, arry[1], arry[2], arry[3], arry[4]);
                DataTable dt = SQLHelper.SHelper.ExeDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int tag = Convert.ToInt32(dt.Rows[0][0]);
                    if (tag == 0)
                    {
                        sql = string.Format(@"Insert into  StockList values({0},{1},{2},{3},{4},{5},{6}) ", pCode, arry[0], arry[1], arry[2], arry[3], arry[4], DateTime.Now.Date);
                        SQLHelper.SHelper.ExeNoQuery(sql);
                    }
                }
                #endregion
            }
        }

        private void pNewRefreshList(string pCode)
        {
            this.StockNum = pCode;
            this.URL = "https://tw.stock.yahoo.com/d/s/company_";
            string temp = this.NewRefreshStockList();
            //if (temp != "error")
            //{
            //    string[] arry = temp.Split(',');
            //    #region  20150324 modifed by Dick for 修改維新的方式撰寫SQL
            //    string sql = string.Format(@"select count(*) from StockList where StockNum={0} and EPS1={1} and EPS2={2} and EPS3 ={3} and EPS4 ={4} ", pCode, arry[1], arry[2], arry[3], arry[4]);
            //    DataTable dt = SQLHelper.SHelper.ExeDataTable(sql);
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        int tag = Convert.ToInt32(dt.Rows[0][0]);
            //        if (tag == 0)
            //        {
            //            sql = string.Format(@"Insert into  StockList values({0},{1},{2},{3},{4},{5},{6}) ", pCode, arry[0], arry[1], arry[2], arry[3], arry[4], DateTime.Now.Date);
            //            SQLHelper.SHelper.ExeNoQuery(sql);
            //        }
            //    }
            //    #endregion
            //}
            Console.WriteLine(temp);
        }

        /// <summary>NewFreshStockList</summary>
        /// <returns></returns>
        private string NewRefreshStockList()
        {
            CommTool.ToolLog log = new CommTool.ToolLog();
            try
            {
                WebRequest myWebRequest = WebRequest.Create(_url + _stockNum + Default.Html);
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
                Stream DataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(DataStream, Encoding.Default);
                string ResponseFromService = reader.ReadToEnd();
                int start = ResponseFromService.IndexOf("description");
                int end = ResponseFromService.IndexOf("公司資料，查詢 ");
                int deffence = end - start;
                StringBuilder sb = new StringBuilder();

                sb.Append(string.Format("{0} ,", _stockNum));
                sb.Append(ResponseFromService.Substring(start, deffence).Replace("description\" content=\"", string.Empty).Replace("(" + _stockNum + ")", string.Empty));
                sb.Append(",");
                string[] sArray = Regex.Split(ResponseFromService, "table", RegexOptions.IgnoreCase);
                string[] arry = Regex.Split(sArray[9], "tr", RegexOptions.IgnoreCase);
                decimal Bouns = 0;
                for (int i = 3; i < arry.Length; i++)
                {
                    string[] parry = Regex.Split(arry[i], "td", RegexOptions.IgnoreCase);
                    if (parry.Length > 8)
                    {
                        sb.Append(parry[5].Replace("bgcolor=\"#FFFAE8\">", string.Empty).Replace("</", string.Empty).Replace("height=\"25\"", string.Empty)
                            .Replace("width=\"20%\\", string.Empty).Replace("width=\"113\"", string.Empty).Trim());
                        string temp = parry[7].Replace("align=\"center\">", string.Empty).Replace("</", string.Empty).Replace("width=\"83\"", string.Empty);
                        sb.Append(temp);
                        temp = temp.Replace("元", string.Empty);
                        if (i == 3)
                        {
                            Bouns = Convert.ToDecimal(temp);
                        }
                        sb.Append(",");
                    }
                }

                string[] EPS_arry = Regex.Split(sArray[11], "tr", RegexOptions.IgnoreCase);
                decimal[] EPS_Array = new decimal[4];
                int count = 0;
                for (int i = 3; i <= 9; i += 2)
                {
                    string[] parry = Regex.Split(EPS_arry[i], "td", RegexOptions.IgnoreCase);
                    string Temp = parry[7].Replace("align=\"center\">", string.Empty).Replace("元</", string.Empty);
                    decimal Dor = Convert.ToDecimal(Temp);
                    EPS_Array[count] = Dor;
                    count++;
                }

                string _Url = string.Format("https://tw.stock.yahoo.com/q/ts?s={0}", _stockNum);
                decimal reuslt = StockPriceNow(_Url);
                decimal Rate = (Bouns / reuslt) * 100;
                sb.Append(string.Format("成交價:{0},殖利率:{1} ", reuslt.ToString("##.###"), Rate.ToString("##.###")));
                sb.AppendLine(string.Format(" EPS_First :{0} ,EPS_Second :{1},EPS_Third :{2},EPS_Fourth :{3} ", EPS_Array[0], EPS_Array[1], EPS_Array[2], EPS_Array[3]));
                bool IsImport = false;

                if (EPS_Array[3] < EPS_Array[2] & EPS_Array[2] < EPS_Array[1] & EPS_Array[1] < EPS_Array[0])
                {
                    IsImport = true; ///最佳進步獎
                }

                if (Rate > 5 && IsImport)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    CommTool.ToolLog.Log(CommTool.LogType.Exclude, sb.ToString());
                }
                else if (Rate > 5)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    CommTool.ToolLog.Log(CommTool.LogType.Normal, sb.ToString());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ToolLog.Log(ex);
                //CommTool.ToolLog.Log(CommTool.LogType.Error, _stockNum.ToString());
                return "error";
            }

        }//end  NewRefreshStockList

        /// <summary>目前成交價</summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        private decimal StockPriceNow(string Url)
        {
            decimal Result = 0;
            try
            {
                WebRequest myWebRequest = WebRequest.Create(Url);
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
                Stream DataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(DataStream, Encoding.Default);
                string ResponseFromService = reader.ReadToEnd();
                string[] sArray = Regex.Split(ResponseFromService, "table", RegexOptions.IgnoreCase);
                string[] arry = Regex.Split(sArray[10], "tr", RegexOptions.IgnoreCase);
                string[] TdArry = Regex.Split(arry[5], "td", RegexOptions.IgnoreCase);
                string Temp = TdArry[7].Replace("class=\"high\">", string.Empty).Replace("</", string.Empty);
                Result = Convert.ToDecimal(Temp);
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
            return Result;
        }

        /// <summary>追蹤EPS 連4季 都是上升的股票 最佳進步獎
        /// </summary>
        /// <returns></returns>
        public string TraceEPS()
        {
            #region  20150324 modifed by Dick for 修改維新的方式撰寫SQL
            DataTable dt = SQLHelper.SHelper.ExeDataTable("Select StockNum,StockName,EPS1,EPS2,EPS3,EPS4 from StockList Group by StockNum,StockName,EPS1,EPS2,EPS3,EPS4");
            StringBuilder sb = new StringBuilder();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    double EPS1 = Convert.ToDouble(EPSSplit(dr[2].ToString()));
                    double EPS2 = Convert.ToDouble(EPSSplit(dr[3].ToString()));
                    double EPS3 = Convert.ToDouble(EPSSplit(dr[4].ToString()));
                    double EPS4 = Convert.ToDouble(EPSSplit(dr[5].ToString()));
                    if (EPS1 > EPS2 & EPS2 > EPS3 & EPS3 > EPS4)
                    {
                        sb.Append(dr[0].ToString());
                        sb.Append("   ");
                        sb.Append(dr[1].ToString());
                        sb.Append("   ");
                        sb.Append(dr[2].ToString());
                        sb.Append("   ");
                        sb.Append(dr[3].ToString());
                        sb.Append("   ");
                        sb.Append(dr[4].ToString());
                        sb.Append("   ");
                        sb.Append(dr[5].ToString());
                        sb.Append("\r\n");
                    }
                }
            }
            #endregion
            return sb.ToString();
        }

        private string EPSSplit(string eps)
        {
            int start = eps.IndexOf("季");
            int end = eps.IndexOf("元");
            int deff = end - start;
            if (deff > 0)
            {
                string temp = eps.Substring(start + 1, deff - 1);
                return temp;
            }
            else
            {
                return "0";
            }
        }

        /// <summary>抓取當日的資料</summary>
        /// 20170126 modifed by Dick
        /// 20170202 修正由這邊控制抓取資料的時間區間 modifed by Dick
        /// 20170203 加入抓取大盤資料的功能
        /// 20170208 加入發送Maill的功能
        /// 20170327 modified by Dick for 非同步方式抓取資料
        /// 20170518 modified by Dick for 換個網站抓選擇權
        /// 20170914 modified by Dick for 大盤跟期貨資訊換網站抓取
        /// <param name="Url"></param>
        /// <returns></returns>
        public dynamic GetOptionEveryDay(string CapitalfuturesUrl, string WeightedUrl, int Channel = 2)
        {
            CalendarDAO CalendarDB = new CalendarDAO();
            DateTime TimeStamp = DateTime.Now;
            Calendar _Calendar = CalendarDB.GetCalendar(TimeStamp);
            string Message = "NotTradeTime";
            if (_Calendar.IsWorkDay)
            {
                TimeSpan StartTimeSpan = TimeStamp.Subtract(new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 8, 44, 59));
                TimeSpan EndTimeSpan = TimeStamp.Subtract(new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 13, 45, 20));
                if (StartTimeSpan.TotalSeconds >= 0 && EndTimeSpan.TotalSeconds <= 0)
                {
                    try
                    {
                        string TradeTimestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        var tasks = new List<Task<int>>();
                        List<Option> ListOption = new List<Option>();
                        Task Task1 = null; Task Task2 = null; Task Task3 = null;

                        if (Channel == 1)
                        {
                            Task1 = Task.Factory.StartNew(() => { ListOption.AddRange(GetOptionDaily(CapitalfuturesUrl, _Calendar.Week, Encoding.UTF8, TradeTimestamp)); });//周選 
                            Task2 = Task.Factory.StartNew(() => { ListOption.AddRange(GetOptionDaily(CapitalfuturesUrl, _Calendar.NearMonth1, Encoding.UTF8, TradeTimestamp)); });//近月選1   
                            Task3 = Task.Factory.StartNew(() => { ListOption.AddRange(GetOptionDaily(CapitalfuturesUrl, _Calendar.NearMonth2, Encoding.UTF8, TradeTimestamp)); }); //近月選2  
                        }

                        if (Channel == 2)
                        {
                            Task1 = Task.Factory.StartNew(() => { ListOption.AddRange(GetOptionDailyCapitalfutures(CapitalfuturesUrl, _Calendar.Week, Encoding.UTF8, TradeTimestamp)); });//周選 
                            Task2 = Task.Factory.StartNew(() => { ListOption.AddRange(GetOptionDailyCapitalfutures(CapitalfuturesUrl, _Calendar.NearMonth1, Encoding.UTF8, TradeTimestamp)); });//近月選1   
                            Task3 = Task.Factory.StartNew(() => { ListOption.AddRange(GetOptionDailyCapitalfutures(CapitalfuturesUrl, _Calendar.NearMonth2, Encoding.UTF8, TradeTimestamp)); }); //近月選2                                 
                        }

                        Task Task4 = Task.Factory.StartNew(() => {
                            WeightedDAO WeightedDAO = new WeightedDAO();
                            Weighted _Weighted = WeightedDAO.GetWeightedDaily(WeightedUrl);
                            if (_Weighted != null)
                            {
                                _Weighted.TradeTimestamp = TradeTimestamp;
                                WeightedDAO.SaveWeighted(_Weighted);
                            }
                        });

                        Task.WaitAll(Task1, Task2, Task3, Task4);
                        SaveOpionData(ListOption);
                        AddOption(ListOption);
                        Message = string.Format("GetOptionOK :ListOption=>{0} ", ListOption.Count.ToString());
                        ToolLog.Log(Message);
                        GetNumberOfContractsAndMaill();
                        return Message;
                    }
                    catch (Exception ex)
                    {
                        ToolLog.Log(ex);
                        Message = "Error";
                        return Message;
                    }
                }
                else
                {
                    ToolLog.Log(Message);
                    return Message;
                }
            }
            else
            {
                Message = "NotWorkDay";
                ToolLog.Log(Message);
                return Message;
            }
        }

        /// <summary>取得選擇權價格清單，預設編碼</summary>
        /// <param name="Url">POST位置</param>
        /// <returns></returns>
        public List<Option> GetOptionDaily(string Url, string Contract, string TradeTimestamp)
        {
            return GetOptionDaily(Url, Contract, Encoding.Default, TradeTimestamp);
        }

        /// <summary>取得選擇權價格清單 (yahoo)</summary>
        /// 20161110 add by Dick
        /// 20170126 modifed by Dick
        /// 20170202 修正網頁上資訊不足時出現陣列長度錯誤  modifed by Dick
        /// <param name="Url"></param>
        /// <param name="Contract"></param>
        /// <param name="UrlEncoding"></param>
        /// <param name="IsGetWeighed">是否抓取大盤</param>
        /// <returns></returns>
        public List<Option> GetOptionDaily(string Url, string Contract, Encoding UrlEncoding, string TradeTimestamp)
        {
            WebInfo.WebInfo Info = new WebInfo.WebInfo();
            List<Option> list = new List<Option>();
            StreamReader SR = Info.GetResponse(Url + Contract, UrlEncoding);
            HtmlAgilityPack.HtmlDocument _HtmlDocument = new HtmlAgilityPack.HtmlDocument();
            _HtmlDocument.LoadHtml(SR.ReadToEnd());
            SR.Close();
            HtmlAgilityPack.HtmlNodeCollection anchors = _HtmlDocument.DocumentNode.SelectNodes(Default.TableTag);
            if (anchors != null)
            {
                if (anchors.Count > 3)
                {
                    HtmlAgilityPack.HtmlNodeCollection Nodes = anchors[3].SelectNodes(Default.HtmlTr);
                    if (Nodes != null)
                    {
                        if (Nodes.Count >= 45)
                        {
                            for (int i = 29; i <= 45; i++)
                            {
                                Option Call = new Option();
                                Call.OP = Default.Call;
                                Call.DueMonth = Contract;
                                Call.Buy = Nodes[i].ChildNodes[1].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDecimal(Nodes[i].ChildNodes[1].InnerText);
                                Call.Sell = Nodes[i].ChildNodes[3].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDecimal(Nodes[i].ChildNodes[3].InnerText);
                                Call.Clinch = Nodes[i].ChildNodes[5].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDecimal(Nodes[i].ChildNodes[5].InnerText);
                                Call.Change = Nodes[i].ChildNodes[7].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDecimal(Nodes[i].ChildNodes[7].InnerText);
                                Call.NumberOfContracts = Nodes[i].ChildNodes[9].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToInt32(Nodes[i].ChildNodes[9].InnerText);
                                Call.Volume = Nodes[i].ChildNodes[11].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToInt32(Nodes[i].ChildNodes[11].InnerText);
                                Call.Time = Nodes[i].ChildNodes[13].InnerText.Trim() == Default.NullSing ? DateTime.Now.ToString(BaseConst.TimeFormatComplete) : Convert.ToDateTime(Nodes[i].ChildNodes[13].InnerText).ToString(BaseConst.TimeFormatComplete);
                                Call.Contract = Nodes[i].ChildNodes[15].InnerText.Trim();
                                Call.TradeTimestamp = TradeTimestamp;
                                list.Add(Call);
                                Option Put = new Option();
                                Put.OP = Default.Put;
                                Put.DueMonth = Contract;
                                Put.Contract = Nodes[i].ChildNodes[15].InnerText.Trim();
                                Put.Buy = Nodes[i].ChildNodes[17].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDecimal(Nodes[i].ChildNodes[17].InnerText);
                                Put.Sell = Nodes[i].ChildNodes[19].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDecimal(Nodes[i].ChildNodes[19].InnerText);
                                Put.Clinch = Nodes[i].ChildNodes[21].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDecimal(Nodes[i].ChildNodes[21].InnerText);
                                Put.Change = Nodes[i].ChildNodes[23].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDecimal(Nodes[i].ChildNodes[23].InnerText);
                                Put.NumberOfContracts = Nodes[i].ChildNodes[25].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToInt32(Nodes[i].ChildNodes[25].InnerText);
                                Put.Volume = Nodes[i].ChildNodes[27].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToInt32(Nodes[i].ChildNodes[27].InnerText);
                                Put.Time = Nodes[i].ChildNodes[29].InnerText.Trim() == Default.NullSing ? DateTime.Now.ToString(BaseConst.TimeFormatComplete) : Convert.ToDateTime(Nodes[i].ChildNodes[29].InnerText).ToString(BaseConst.TimeFormatComplete);
                                Put.TradeTimestamp = TradeTimestamp;
                                list.Add(Put);
                            }
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>抓取周選資料(群益)</summary>
        /// 20170518 add by Dick
        /// 20170519 modifed by Dick 修正抓取資料錯誤         
        /// <param name="Url"></param>
        /// <param name="Contract"></param>
        /// <param name="UrlEncoding"></param>
        /// <param name="TradeTimestamp"></param>
        /// <returns></returns>
        public List<Option> GetOptionDailyCapitalfutures(string Url, string Contract, Encoding UrlEncoding, string TradeTimestamp)
        {
            WebInfo.WebInfo Info = new WebInfo.WebInfo();
            List<Option> list = new List<Option>();
            string TempCode = System.Web.HttpUtility.UrlEncode("_台選", Encoding.GetEncoding("BIG5")).ToUpper();//將繁體中文轉成Uri
            TempCode = TempCode.Replace("X", "x");
            string Temp = Contract.Replace(DateTime.Now.Year.ToString(), string.Empty).Replace(DateTime.Now.AddYears(1).Year.ToString(), string.Empty).ToLower();
            string[] arr = Temp.Split('w');
            string GetParameter = string.Empty;
            int StartTag = 0;
            int EndTag = 0;
            if (arr.Length > 1)
            {
                GetParameter = string.Format("Sname=TX{0}{1}{2}W{0}{1}&xy=1:7", arr[1], arr[0], TempCode);
                StartTag = 11;
                EndTag = 87;
            }
            else
            {
                GetParameter = string.Format("Sname=TXO{0}{1}{0}&xy=1:7", Temp, TempCode);
                StartTag = 91;
                EndTag = 171;
            }

            string FullUrl = string.Format("{0}{1}", Url, GetParameter);
            HtmlNodeCollection anchors = Info.GetWebHtmlDocumentNodeCollection(FullUrl, "//table[@class='type-03']", Encoding.Default);
            if (anchors != null)
            {
                if (EndTag > anchors[0].ChildNodes.Count)
                {
                    StartTag = 47;
                    EndTag = 121;
                }
                while (StartTag <= EndTag)
                {
                    try
                    {
                        Option Call = new Option();
                        Call.OP = Default.Call;
                        HtmlNode Node = anchors[0].ChildNodes[StartTag];
                        string C1 = Node.ChildNodes[1].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string C2 = Node.ChildNodes[3].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string C3 = Node.ChildNodes[5].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string C4 = Node.ChildNodes[7].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string C5 = Node.ChildNodes[9].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string C6 = Node.ChildNodes[13].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        Call.Buy = string.IsNullOrEmpty(C1) == true ? 0 : Convert.ToDecimal(C1);
                        Call.Sell = string.IsNullOrEmpty(C2) == true ? 0 : Convert.ToDecimal(C2);
                        Call.Clinch = string.IsNullOrEmpty(C3) == true ? 0 : Convert.ToDecimal(C3);
                        Call.Change = string.IsNullOrEmpty(C4) == true ? 0 : Convert.ToDecimal(C4);
                        Call.Volume = string.IsNullOrEmpty(C5) == true ? 0 : Convert.ToInt32(C5);
                        Call.Contract = C6.Replace(",", string.Empty);
                        Call.TradeTimestamp = TradeTimestamp;
                        Call.DueMonth = Contract;
                        Call.Time = DateTime.Now.ToString(CommTool.BaseConst.TimeFormatComplete);
                        if (Call.Volume > 0)
                        {
                            list.Add(Call);
                        }
                        HtmlNode PutNode = anchors[1].ChildNodes[StartTag];
                        string P1 = PutNode.ChildNodes[1].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string P2 = PutNode.ChildNodes[3].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string P3 = PutNode.ChildNodes[5].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string P4 = PutNode.ChildNodes[7].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string P5 = PutNode.ChildNodes[9].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        string P6 = PutNode.ChildNodes[11].InnerText.Replace("-&nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace(",", string.Empty).Trim();
                        Option Put = new Option();
                        Put.OP = Default.Put;
                        Put.Buy = string.IsNullOrEmpty(P2) == true ? 0 : Convert.ToDecimal(P2);
                        Put.Sell = string.IsNullOrEmpty(P3) == true ? 0 : Convert.ToDecimal(P3);
                        Put.Clinch = string.IsNullOrEmpty(P4) == true ? 0 : Convert.ToDecimal(P4);
                        Put.Change = string.IsNullOrEmpty(P5) == true ? 0 : Convert.ToDecimal(P5);
                        Put.Volume = string.IsNullOrEmpty(P6) == true ? 0 : Convert.ToInt32(P6);
                        Put.Contract = Call.Contract;
                        Put.TradeTimestamp = TradeTimestamp;
                        Put.DueMonth = Contract;
                        Put.Time = DateTime.Now.ToString(BaseConst.TimeFormatComplete);
                        if (Put.Volume > 0)
                        {
                            list.Add(Put);
                        }
                    }
                    catch (Exception ex)
                    {
                        ToolLog.Log(ex.StackTrace.ToString());
                    }
                    StartTag = StartTag + 4;
                }
            }
            return list;
        }

        /// <summary>儲存選擇權交易歷史資訊</summary>
        /// <param name="Options"></param>
        public void SaveOpionData(List<Option> Options)
        {
            try
            {
                foreach (Option op in Options)
                {
                    if (op.Volume > 0)
                    {
                        USP.AddParameter(SPParameter.OP, op.OP);
                        USP.AddParameter(SPParameter.Buy, op.Buy);
                        USP.AddParameter(SPParameter.Change, op.Change);
                        USP.AddParameter(SPParameter.Clinch, op.Clinch);
                        USP.AddParameter(SPParameter.Contract, op.Contract);
                        USP.AddParameter(SPParameter.Sell, op.Sell);
                        USP.AddParameter(SPParameter.Time, op.Time);
                        USP.AddParameter(SPParameter.DueMonth, op.DueMonth);
                        USP.AddParameter(SPParameter.NumberOfContracts, op.NumberOfContracts);
                        USP.AddParameter(SPParameter.TradeTimestamp, op.TradeTimestamp);
                        USP.AddParameter(SPParameter.Volume, op.Volume);
                        USP.ExeProcedureNotQuery(SP.SaveOption);
                    }
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }

        /// <summary>結構方式儲存選擇權交易歷史資訊</summary>
        /// <param name="Options"></param>
        public void AddOption(List<Option> Options)
        {
            try
            {
                DataTable OptionDataTable = CommTool.ObjectUtility.ToDataTable(Options, Option.GetTableTypeColumn());
                USP.AddParameter(SPParameter.OptionView, OptionDataTable);
                USP.ExeProcedureNotQuery(SP.AddOption);
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }

        /// <summary>取得每周焦點</summary>
        /// <param name="weekPoint"></param>
        /// <returns></returns>
        public WeekPoint GetWeekPointByDueMonthAndOP(WeekPoint weekPoint)
        {
            try
            {
                USP.AddParameter(SPParameter.OP, weekPoint.OP);
                USP.AddParameter(SPParameter.DueMonth, weekPoint.DueMonth);
                return USP.ExeProcedureGetObject(SP.GetWeekPointByDueMonthAndOP, weekPoint);
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
                return null;
            }
        }

        /// <summary>儲存選擇權交易歷史資訊</summary>
        /// <param name="dt"></param>
        public void SaveOptionHistoryData(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    decimal Temp = Convert.ToDecimal(dr[SPParameter.Opening_Price]);
                    if (Temp != 0)
                    {
                        USP.AddParameter(SPParameter.TradeDate, dr[SPParameter.TradeDate]);
                        USP.AddParameter(SPParameter.Contract, dr[SPParameter.Contract]);
                        USP.AddParameter(SPParameter.DueMonth, dr[SPParameter.DueMonth]);
                        USP.AddParameter(SPParameter.Price, dr[SPParameter.Price]);
                        USP.AddParameter(SPParameter.Option, dr[SPParameter.Option]);
                        USP.AddParameter(SPParameter.Opening_Price, dr[SPParameter.Opening_Price]);
                        USP.AddParameter(SPParameter.Highest, dr[SPParameter.Highest]);
                        USP.AddParameter(SPParameter.Lowest, dr[SPParameter.Lowest]);
                        USP.AddParameter(SPParameter.Closing, dr[SPParameter.Closing]);
                        USP.AddParameter(SPParameter.Volume, dr[SPParameter.Volume]);
                        USP.AddParameter(SPParameter.Settlement, dr[SPParameter.Settlement]);
                        USP.AddParameter(SPParameter.NumberOfContracts, dr[SPParameter.NumberOfContracts]);
                        USP.AddParameter(SPParameter.GreatBuy, dr[SPParameter.GreatBuy]);
                        USP.AddParameter(SPParameter.GreatSell, dr[SPParameter.GreatSell]);
                        USP.AddParameter(SPParameter.History_Highest, dr[SPParameter.History_Highest]);
                        USP.AddParameter(SPParameter.History_Lowest, dr[SPParameter.History_Lowest]);
                        USP.AddParameter(SPParameter.Remark, dr[SPParameter.Remark]);
                        USP.ExeProcedureNotQuery(SP.SaveOptionHistory);
                    }
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }

        /// <summary>每周操作紀錄</summary>
        /// 20170208 add by Dick 
        /// 20170227 add by Dick for 追加新欄位
        /// <param name="_WeekPoint"></param>
        public void AddWeekPoint(WeekPoint _WeekPoint)
        {
            try
            {
                USP.AddParameter(SPParameter.TradeDate, _WeekPoint.TradeDate);
                USP.AddParameter(SPParameter.OP, _WeekPoint.OP);
                USP.AddParameter(SPParameter.Contract, _WeekPoint.Contract);
                USP.AddParameter(SPParameter.Price, _WeekPoint.Price);
                USP.AddParameter(SPParameter.Volume, _WeekPoint.Volume);
                USP.AddParameter(SPParameter.StopPrice, _WeekPoint.StopPrice);
                USP.AddParameter(SPParameter.DueMonth, _WeekPoint.DueMonth);
                USP.AddParameter(SPParameter.BuyStopPrice, _WeekPoint.BuyStopPrice);
                USP.ExeProcedureHasResult(SP.AddWeekPoint);
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }

        public DataTable SelectOptionHistory(string DueMonth, string Option, string Start, string End)
        {
            try
            {
                USP.AddParameter(SPParameter.DueMonth, DueMonth);
                USP.AddParameter(SPParameter.Option, Option);
                USP.AddParameter(SPParameter.Start, Start);
                USP.AddParameter(SPParameter.End, End);
                return USP.ExeProcedureGetDataTable(SP.GetOptionHistory);
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
                return null;
            }
        }

        /// <summary>取得周選契約</summary>
        /// <returns></returns>
        public DataTable GetDueMonth()
        {
            try
            {
                return USP.ExeProcedureGetDataTable(SP.GetDueMonth);
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
                return null;
            }
        }

        /// <summary>計算最大交易量的周選勝率及賺賠結果</summary>
        /// <param name="BaseMoney">雙SELL的組數</param>
        /// <param name="SmallPoint">最低買進點數</param>
        public void GetOptionWeek(int BaseMoney, int SmallPoint)
        {
            DataTable Report = new DataTable();
            Report.Columns.Add(SPParameter.TradeDate);
            Report.Columns.Add(SPParameter.Option);
            Report.Columns.Add(SPParameter.DueMonth);
            Report.Columns.Add(SPParameter.Price);
            Report.Columns.Add(SPParameter.Closing);
            Report.Columns.Add(SPParameter.OpenPrice);
            Report.Columns.Add(SPParameter.ClosingPrice);
            List<string> MonthsList = new List<string>();
            DateTime NowDate = new DateTime(2012, 11, 20);
            string[] Option = new string[] { "賣權", "買權" };
            DateTime Month = NowDate;
            DataTable DueMonthTable = this.GetDueMonth();
            if (DueMonthTable.Rows.Count > 0)
            {
                foreach (DataRow dr in DueMonthTable.Rows)
                {
                    MonthsList.Add(dr[Default.FirstItem].ToString());
                }
            }
            string NowDueMont, NowPrice = string.Empty;
            bool IsNoData = false;
            bool IsPass = false;
            DataRow NewRow = Report.NewRow();
            Decimal StartPrice = 0;
            Decimal EndPrice = 0;
            Decimal Result = 0;
            int weekcount = 0;
            int win = 0;
            int PassDay = 0;
            foreach (string Due in MonthsList)
            {
                weekcount++;
                decimal week = 0;
                PassDay += 6;
                foreach (string OP in Option)
                {
                    DateTime Temp = NowDate.AddDays(PassDay);
                    while (Temp <= DateTime.Now)
                    {
                        Temp = Temp.AddDays(1);
                        if (!IsNoData)
                        {
                            DataRow row = GETPrice(NewRow, OP, Temp, Due);
                            NowDueMont = Due;
                            NowPrice = row.ItemArray[3].ToString();
                            if (!string.IsNullOrEmpty(NowPrice))
                            {
                                IsNoData = true;
                                Report.Rows.Add(row);
                                var temp = Convert.ToDecimal(row.ItemArray[4]);
                                if (temp > SmallPoint)
                                {
                                    StartPrice = temp;
                                }
                                else
                                {
                                    IsPass = true;
                                }
                            }
                        }
                        else
                        {
                            DataRow row = GetResultByPrice(NewRow, OP, Temp, Due, NowPrice);
                            if (row.ItemArray[3].ToString() != string.Empty)
                            {
                                Report.Rows.Add(row);
                            }
                        }
                        if (Report.Rows.Count > 0)
                        {
                            DataRow LastRow = Report.Rows[Report.Rows.Count - 1];
                            if (Convert.ToDateTime(LastRow.ItemArray[0]).AddMonths(2) <= Temp)
                            {
                                break;
                            }
                        }
                    }
                    IsNoData = false;
                    if (!IsPass)
                    {
                        if (Report.Rows.Count > 0)
                        {
                            DataRow EndRow = Report.Rows[Report.Rows.Count - 1];
                            EndPrice = Convert.ToDecimal(EndRow.ItemArray[4].ToString());
                            week += (StartPrice - EndPrice) * BaseMoney;
                        }
                    }
                    Report.Rows.Clear();
                    IsPass = false;
                }
                Result += week;
                if (week > 0)
                {
                    win++;
                }
                string Message = string.Format("契約:{0} ,賺賠:{1},總賺賠：{2}", Due, week.ToString("00000.##"), Result.ToString("00000.##"));
                CommTool.ToolLog.Log(Message);
            }
            CommTool.ToolLog.Log(string.Format("勝率:{0}%", (win * 100 / weekcount)));
        }

        /// <summary>計算最大交易量的周選勝率及賺賠結果，遇到收盤操過買進價格的數倍時停損</summary>
        /// <param name="BaseMoney"></param>
        /// <param name="SmallPoint"></param>
        /// <param name="StopBase"></param>
        public void GetOptionWeekWithStop(int BaseMoney, decimal SmallPoint, decimal StopBase)
        {
            DataTable Report = new DataTable();
            Report.Columns.Add(SPParameter.TradeDate);
            Report.Columns.Add(SPParameter.Option);
            Report.Columns.Add(SPParameter.DueMonth);
            Report.Columns.Add(SPParameter.Price);
            Report.Columns.Add(SPParameter.Closing);
            Report.Columns.Add(SPParameter.OpenPrice);
            Report.Columns.Add(SPParameter.ClosingPrice);
            Report.Columns.Add(SPParameter.Highest);
            Report.Columns.Add(SPParameter.Opening_Price);
            List<string> MonthsList = new List<string>();
            DateTime NowDate = new DateTime(2012, 11, 20);
            string[] Option = new string[] { "賣權", "買權" };
            DateTime Month = NowDate;
            DataTable DueMonthTable = this.GetDueMonth();
            if (DueMonthTable.Rows.Count > 0)
            {
                foreach (DataRow dr in DueMonthTable.Rows)
                {
                    MonthsList.Add(dr[Default.FirstItem].ToString());
                }
            }
            string NowDueMont, NowPrice = string.Empty;
            bool IsNoData = false;
            bool IsPass = false;
            DataRow NewRow = Report.NewRow();
            Decimal StartPrice = 0;
            Decimal EndPrice = 0;
            Decimal Result = 0;
            int weekcount = 0;
            int win = 0;
            int PassDay = 0;
            decimal buyPoint = 0;
            foreach (string Due in MonthsList)
            {
                weekcount++;
                decimal week = 0;
                PassDay += 6;
                foreach (string OP in Option)
                {
                    decimal StopPrice = 0;
                    DateTime Temp = NowDate.AddDays(PassDay);
                    while (Temp <= DateTime.Now)
                    {
                        Temp = Temp.AddDays(1);
                        if (!IsNoData)
                        {
                            DataRow row = GETPrice(NewRow, OP, Temp, Due);
                            NowDueMont = Due;
                            NowPrice = row.ItemArray[3].ToString();
                            if (!string.IsNullOrEmpty(NowPrice))
                            {
                                IsNoData = true;
                                Report.Rows.Add(row);
                                var temp = Convert.ToDecimal(row.ItemArray[4]);
                                if (temp > SmallPoint)
                                {
                                    StartPrice = temp;
                                    StopPrice = CalculateStopPrice(StartPrice, row.ItemArray[3].ToString(), Convert.ToDecimal(row.ItemArray[6].ToString()));
                                    ToolLog.Log(string.Format("契約:{0} , 日期:{1} , Option :{2} , 價格:{3} ,停損價格 {4}", Due, row.ItemArray[0].ToString(), OP, temp, StopPrice));
                                }
                                else
                                {
                                    IsPass = true;
                                }
                            }
                        }
                        else
                        {
                            DataRow row = GetResultByPrice(NewRow, OP, Temp, Due, NowPrice);
                            if (row.ItemArray[3].ToString() != string.Empty)
                            {
                                Report.Rows.Add(row);
                            }
                        }
                        if (Report.Rows.Count > 0)
                        {
                            DataRow LastRow = Report.Rows[Report.Rows.Count - 1];
                            if (Convert.ToDateTime(LastRow.ItemArray[0]).AddMonths(2) <= Temp)
                            {
                                break;
                            }
                        }
                    }
                    IsNoData = false;
                    if (!IsPass)
                    {
                        if (Report.Rows.Count > 0)
                        {
                            decimal Loss = 0;
                            decimal BuyPoint = 0;
                            DataRow EndRow = Report.Rows[Report.Rows.Count - 1];
                            EndPrice = Convert.ToDecimal(EndRow.ItemArray[4].ToString());
                            bool IsOver = false;
                            for (int i = 1; i < Report.Rows.Count; i++)
                            {
                                DataRow row = Report.Rows[i];
                                decimal TempPrice = Convert.ToDecimal(row.ItemArray[4].ToString());
                                decimal HighPrice = Convert.ToDecimal(row.ItemArray[7].ToString());
                                ToolLog.Log(string.Format("契約:{0} , 日期:{1} , Option :{2} , 契約價格:{3} , 點數:{4} , 最高點{5}", Due, row.ItemArray[0].ToString(), OP, row.ItemArray[3].ToString(), TempPrice, HighPrice));
                                if (!IsOver)
                                {
                                    if (TempPrice > (StopPrice + StartPrice) || HighPrice > (StopPrice + StartPrice))
                                    {
                                        Loss += ((StopPrice + StartPrice) - StartPrice) + 10;
                                        StartPrice = (StopPrice + StartPrice);
                                        IsOver = true;
                                    }
                                }
                                else
                                {
                                    if (TempPrice <= StartPrice)
                                    {
                                        BuyPoint = ((TempPrice - StartPrice) - 10) * 5;
                                        StartPrice = TempPrice;
                                        IsOver = false;
                                    }
                                }

                                //if (!IsOver) {
                                //    if (TempPrice >= (StartPrice * StopBase)) {
                                //        buyPoint = (EndPrice - TempPrice) * (5 * BaseMoney);
                                //        EndPrice = TempPrice;
                                //        CommTool.ToolLog.Log(string.Format("契約:{0} , 買價:{1} , 停損價：{2} ,Option :{3} , 轉買方價差 {4}", Due, StartPrice.ToString("00000.##"), TempPrice.ToString("00000.##"), OP, buyPoint));
                                //        IsOver = true;
                                //    }
                                //}
                            }
                            if (EndPrice > StartPrice)
                            {
                                BuyPoint = (EndPrice - StartPrice) * 5;
                            }
                            else
                            {
                                BuyPoint += (StartPrice - EndPrice);
                            }
                            //if (!IsOver) {
                            //    week += (StartPrice - EndPrice) * BaseMoney;
                            //}
                            //else {
                            week += (BuyPoint - Loss);
                            //}
                        }
                    }
                    Report.Rows.Clear();
                    IsPass = false;
                    ToolLog.Log(string.Empty);
                }
                Result += (week + buyPoint);
                buyPoint = 0;
                if (week > 0)
                {
                    win++;
                }
                string Message = string.Format("契約:{0} ,賺賠:{1},總賺賠：{2}", Due, week.ToString("00000.##"), Result.ToString("00000.##"));
                ToolLog.Log(Message);
                ToolLog.Log(string.Empty);
            }
            ToolLog.Log(string.Format("勝率:{0}%", (win * 100 / weekcount)));
        }

        /// <summary>計算Sell停損價格</summary>
        /// <param name="Price">點數</param>
        /// <param name="Contact"></param>
        /// <param name="ClosePrice"></param>
        /// <returns></returns>
        public decimal CalculateStopPrice(decimal Price, string Contact, decimal ClosePrice)
        {
            decimal BasePrice = (Price * 50) + (17000 - (Convert.ToDecimal(Contact) - ClosePrice));
            return Price + ((BasePrice * decimal.Parse("0.1")) / 50);
        }

        /// <summary>計算Buy停損價格</summary>
        /// 20170227 add by Dick
        /// <param name="Price"></param>
        /// <returns></returns>
        public decimal CalculateBuyStopPrice(decimal Price)
        {
            return Price * (decimal)0.8;
        }

        /// <summary>取得最大交易量的契約</summary>
        /// <param name="Source"></param>
        /// <param name="Option"></param>
        /// <param name="Temp"></param>
        /// <param name="DueMonth"></param>
        /// <returns></returns>
        private DataRow GETPrice(DataRow Source, string Option, DateTime Temp, string DueMonth)
        {
            DataTable dt = this.SelectOptionHistory(DueMonth, Option, Temp.Date.ToString(Default.TimeFormat), Temp.Date.ToString(Default.TimeFormat));
            DataRow NewRow = Source.Table.NewRow();
            if (dt.Rows.Count > 0)
            {
                var temp = dt.Select("Volume= MAX(Volume)");
                var Price = temp[Default.FirstItem][SPParameter.Price].ToString();
                var Closing = Convert.ToDecimal(temp[Default.FirstItem][SPParameter.Closing]);
                NewRow[SPParameter.TradeDate] = Temp.Date.ToString(Default.TimeFormat);
                NewRow[SPParameter.Option] = Option;
                NewRow[SPParameter.DueMonth] = DueMonth;
                NewRow[SPParameter.Price] = Price;
                NewRow[SPParameter.Closing] = Closing;
                NewRow[SPParameter.OpenPrice] = Convert.ToDecimal(temp[Default.FirstItem][SPParameter.OpenPrice]);
                NewRow[SPParameter.ClosingPrice] = Convert.ToDecimal(temp[Default.FirstItem][SPParameter.ClosingPrice]);
                NewRow[SPParameter.Highest] = Convert.ToDecimal(temp[Default.FirstItem][SPParameter.Highest]);
                NewRow[SPParameter.Opening_Price] = Convert.ToDecimal(temp[Default.FirstItem][SPParameter.Opening_Price]);
            }
            return NewRow;
        }

        /// <summary>取得特定契約的價格</summary>
        /// <param name="Source"></param>
        /// <param name="Option"></param>
        /// <param name="Temp"></param>
        /// <param name="DueMonth"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        private DataRow GetResultByPrice(DataRow Source, string Option, DateTime Temp, string DueMonth, string Price)
        {
            DataTable dt = this.SelectOptionHistory(DueMonth, Option, Temp.Date.ToString(Default.TimeFormat), Temp.Date.ToString(Default.TimeFormat));
            DataRow NewRow = Source.Table.NewRow();
            if (dt.Rows.Count > 0)
            {
                if (dt.Select(string.Format("Price= {0}", Price)).Length > 0)
                {
                    var row = dt.Select(string.Format("Price= {0}", Price))[0];
                    NewRow[SPParameter.TradeDate] = Temp.Date.ToString(Default.TimeFormat);
                    NewRow[SPParameter.Option] = Option;
                    NewRow[SPParameter.DueMonth] = DueMonth;
                    NewRow[SPParameter.Price] = Price;
                    NewRow[SPParameter.Closing] = row[SPParameter.Closing];
                    NewRow[SPParameter.OpenPrice] = Convert.ToDecimal(row[SPParameter.OpenPrice]);
                    NewRow[SPParameter.ClosingPrice] = Convert.ToDecimal(row[SPParameter.ClosingPrice]);
                    NewRow[SPParameter.Highest] = Convert.ToDecimal(row[SPParameter.Highest]);
                    NewRow[SPParameter.Opening_Price] = Convert.ToDecimal(row[SPParameter.Opening_Price]);
                }
            }
            return NewRow;
        }

        /// <summary>儲存每月的選擇權歷史資料</summary>
        /// <param name="DataResource"></param>
        public void SaveOptionHistroy(string DataResource)
        {
            PropertyInfo[] infos = typeof(HistoryOption).GetProperties();
            DataTable dt = new DataTable();
            foreach (PropertyInfo info in infos)
            {
                dt.Columns.Add(info.Name);
            }
            CommToolFiles.ReadCSV(DataResource, dt);
            this.SaveOptionHistoryData(dt);
        }

        /// <summary>將資料儲存到歷史表資料表內</summary>
        public void StoreToHistory()
        {
            USP.ExeProcedureNotQuery(SP.StoreToHistory);
        }

        /// <summary>儲存買賣未平昌</summary>
        /// <param name="DataResource"></param>
        public void SaveOpen_InterestHistory(string DataResource)
        {
            PropertyInfo[] infos = typeof(Open_Interest).GetProperties();
            DataTable dt = new DataTable();
            foreach (PropertyInfo info in infos)
            {
                if (!info.Name.Equals(SPParameter.SN))
                    dt.Columns.Add(info.Name);
            }
            CommToolFiles.ReadCSV(DataResource, dt);
            SaveOpenInterestData(dt);
        }

        /// <summary>儲存買賣未平昌資料</summary>
        /// <param name="dt"></param>
        public void SaveOpenInterestData(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    USP.AddParameter(SPParameter.TradeDate, dr[SPParameter.TradeDate]);
                    USP.AddParameter(SPParameter.CallOpenInterest, dr[SPParameter.CallOpenInterest]);
                    USP.AddParameter(SPParameter.CallVolume, dr[SPParameter.CallVolume]);
                    USP.AddParameter(SPParameter.OpenInterestRatios, dr[SPParameter.OpenInterestRatios]);
                    USP.AddParameter(SPParameter.PutOpenInterest, dr[SPParameter.PutOpenInterest]);
                    USP.AddParameter(SPParameter.PutVolume, dr[SPParameter.PutVolume]);
                    USP.AddParameter(SPParameter.Ratios, dr[SPParameter.Ratios]);
                    USP.ExeProcedureNotQuery(SP.SaveOpenInterest);
                }
            }
        }

        /// <summary>每周三取得操作指標，並且發送Maill</summary>
        /// 20170208 add by Dick 收盤後抓取每周的操作並發送Maill
        /// 20170222 modified by Dick 修正訊息資料發送錯誤
        /// 20170227 modified by Dick 修正訊息錯誤，追加買方策略停損價格
        /// 20170308 add by Dick 加入周選結算結果，最大未平昌點數過小時，就使用最大交易量作為基準
        public void GetNumberOfContractsAndMaill()
        {
            try
            {
                CalendarDAO CalendarDB = new CalendarDAO();
                Calendar _Calendar = CalendarDB.GetCalendar(DateTime.Now);
                if (_Calendar.IsSettlement)
                {
                    DateTime TimeStamp = DateTime.Now;
                    DateTime Start = new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 13, 30, 0);
                    DateTime End = new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 13, 45, 0);
                    TimeSpan StartTimeSpan = TimeStamp.Subtract(Start);
                    TimeSpan EndTimeSpan = TimeStamp.Subtract(End);
                    WeightedDAO WeightedDAO = new WeightedDAO();
                    if (StartTimeSpan.TotalSeconds >= 0 && EndTimeSpan.TotalSeconds <= 0)
                    {
                        if (!_Calendar.IsMaill)
                        {
                            List<string> OPList = new List<string>();
                            OPList.Add(Default.Call);
                            OPList.Add(Default.Put);
                            StringBuilder SB = new StringBuilder();
                            SB.AppendLine(string.Format("次周交易:{0}", _Calendar.NearMonth1));
                            foreach (string OP in OPList)
                            {
                                USP.AddParameter(SPParameter.OP, OP);
                                USP.AddParameter(SPParameter.DueMonth, _Calendar.NearMonth1);
                                DataTable dt = null;
                                if (_Calendar.NearMonth1.IndexOf("W") != -1)
                                {
                                    dt = USP.ExeProcedureGetDataTable(SP.GetMaxVolume);
                                }
                                else
                                {
                                    dt = USP.ExeProcedureGetDataTable(SP.GetMaxNumberOfContracts);
                                    #region 最大未平昌點數過小時，就使用最大交易量作為基準
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        if (Convert.ToDecimal(dt.Rows[0][0]) < 8)
                                        {
                                            USP.AddParameter(SPParameter.OP, OP);
                                            USP.AddParameter(SPParameter.DueMonth, _Calendar.NearMonth1);
                                            dt = USP.ExeProcedureGetDataTable(SP.GetMaxVolume);
                                        }
                                    }
                                    else
                                    {
                                        USP.AddParameter(SPParameter.OP, OP);
                                        USP.AddParameter(SPParameter.DueMonth, _Calendar.NearMonth1);
                                        dt = USP.ExeProcedureGetDataTable(SP.GetMaxVolume);
                                    }
                                    #endregion
                                }
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    Weighted _Weighted = WeightedDAO.GetWeighted();
                                    decimal StopPrice = 0;
                                    if (_Weighted != null)
                                    {
                                        StopPrice = this.CalculateStopPrice(Convert.ToDecimal(dt.Rows[0][0]), dt.Rows[0][1].ToString(), _Weighted.Futures);
                                    }
                                    WeekPoint _WeekPoint = new WeekPoint();
                                    _WeekPoint.OP = OP;
                                    _WeekPoint.TradeDate = DateTime.Now;
                                    _WeekPoint.Price = dt.Rows[0][0].ToString();
                                    _WeekPoint.Contract = dt.Rows[0][1].ToString();
                                    _WeekPoint.Volume = dt.Rows[0][3].ToString();
                                    _WeekPoint.StopPrice = StopPrice.ToString();
                                    _WeekPoint.BuyStopPrice = CalculateBuyStopPrice(StopPrice).ToString();
                                    _WeekPoint.DueMonth = dt.Rows[0][5].ToString();
                                    AddWeekPoint(_WeekPoint);
                                    SB.AppendLine(string.Format("方向:{0} ,   價格:{1}  ,   契約:{2}  ,   交易量:{3} ,   建議停損價格:{4}  , 買方停損價:{5}  ",
                                                    OP, _WeekPoint.Price, _WeekPoint.Contract, _WeekPoint.Volume, StopPrice, _WeekPoint.BuyStopPrice));

                                    #region 更新上周的結果                                       
                                    WeekPoint LastWeek = new WeekPoint();
                                    LastWeek.DueMonth = _Calendar.Week;
                                    LastWeek.OP = OP;
                                    UpdateWeekPointEndPrice(LastWeek);
                                    #endregion
                                }
                            }

                            MailData MailDB = new MailData();
                            DataTable MaillDataTable = MailDB.GetSendMail();
                            if (MaillDataTable != null && MaillDataTable.Rows.Count > 0)
                            {
                                foreach (DataRow dr in MaillDataTable.Rows)
                                {
                                    MailDB.RegistrySend(dr[1].ToString(), "每周操作指標", SB.ToString());
                                }
                            }
                        }
                        _Calendar.IsMaill = true;
                        CalendarDB.UpdateCalendar(_Calendar);
                    }
                }
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }

        /// <summary>紀錄周選結算價格</summary>
        /// 20170308 add by Dick 更新周選結算結果
        /// <param name="DueMonth"></param>
        /// <param name="Op"></param>
        public void UpdateWeekPointEndPrice(WeekPoint _WeekPoint)
        {
            try
            {
                WeekPoint Result = GetWeekPointByDueMonthAndOP(_WeekPoint);
                USP.AddParameter(SPParameter.DueMonth, _WeekPoint.DueMonth);
                USP.AddParameter(SPParameter.OP, _WeekPoint.OP);
                WeekPoint OptionResult = USP.ExeProcedureGetObject(SP.GetWeekPointByDueMonthAndOP, new WeekPoint()) as WeekPoint;
                #region 更新ClosePrice
                USP.AddParameter(SPParameter.DueMonth, _WeekPoint.DueMonth);
                USP.AddParameter(SPParameter.OP, _WeekPoint.OP);
                USP.AddParameter(SPParameter.Contract, _WeekPoint.Contract);
                Option OPResult = USP.ExeProcedureGetObject(SP.GetOptionByMonthAndContractAndOP, new Option()) as Option;
                _WeekPoint.ClosePrice = OPResult.Clinch.ToString();
                #endregion 
                this.UpdateWeekPoint(_WeekPoint);
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
            }
        }

        /// <summary>更新周選資訊</summary>
        /// <param name="_WeekPoint"></param>
        public void UpdateWeekPoint(WeekPoint _WeekPoint)
        {
            try
            {
                USP.AddParameter(SPParameter.BuyStopPrice, _WeekPoint.BuyStopPrice);
                USP.AddParameter(SPParameter.ClosePrice, _WeekPoint.ClosePrice);
                USP.AddParameter(SPParameter.Contract, _WeekPoint.Contract);
                USP.AddParameter(SPParameter.DueMonth, _WeekPoint.DueMonth);
                USP.AddParameter(SPParameter.OP, _WeekPoint.OP);
                USP.AddParameter(SPParameter.Price, _WeekPoint.Price);
                USP.AddParameter(SPParameter.SN, _WeekPoint.SN);
                USP.AddParameter(SPParameter.StopPrice, _WeekPoint.StopPrice);
                USP.AddParameter(SPParameter.TradeDate, _WeekPoint.TradeDate);
                USP.AddParameter(SPParameter.Volume, _WeekPoint.Volume);
                USP.ExeProcedureNotQuery(SP.UpdateWeekPoint);
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
        }

        /// <summary>取得指定的OPtion</summary>
        /// <param name="_WeekPoint"></param>
        /// <returns></returns>
        public Option GetOptionByMonthAndContractAndOP(WeekPoint _WeekPoint)
        {
            USP.AddParameter(SPParameter.DueMonth, _WeekPoint.DueMonth);
            USP.AddParameter(SPParameter.OP, _WeekPoint.OP);
            USP.AddParameter(SPParameter.Contract, _WeekPoint.Contract);
            return USP.ExeProcedureGetObject(SP.GetOptionByMonthAndContractAndOP, new Option()) as Option;
        }//end GetOptionByMonthAndContractAndOP

        /// <summary>取得周選紀錄(年)</summary>
        /// 2019/07/29        
        /// <param name="Year"></param>
        /// <returns></returns>
        public List<WeekPoint> GetAllWeekPointByYear(int Year)
        {
            List<WeekPoint> ListWeekPoint = new List<WeekPoint>();
            try
            {
                USP.AddParameter(SPParameter.Years, Year);
                ListWeekPoint = USP.ExeProcedureGetObjectList(SP.GetAllWeekPointByYear, new WeekPoint());
            }
            catch (Exception ex)
            {
                ToolLog.Log(ex);
            }
            return ListWeekPoint;
        }//end GetAllWeekPointByYear

        /// <summary>時間區間內的選擇權資料</summary>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="_WeekPoint"></param>
        /// <returns></returns>
        public List<Option> GetListOptionCondition(DateTime BeginTime, DateTime EndTime, WeekPoint _WeekPoint)
        {
            List<Option> ListOption = new List<Option>();
            try
            {
                USP.AddParameter(SPParameter.BeginTime, BeginTime);
                USP.AddParameter(SPParameter.EndTime, EndTime);
                USP.AddParameter(SPParameter.Contract, _WeekPoint.Contract);
                USP.AddParameter(SPParameter.DueMonth, _WeekPoint.DueMonth);
                USP.AddParameter(SPParameter.OP, _WeekPoint.OP);
                ListOption = USP.ExeProcedureGetObjectList(SP.GetListOptionCondition, new Option());
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
            }
            return ListOption;
        }//end GetListOptionCondition

        public List<Option> GetListOption(DateTime BeginTime, DateTime EndTime)
        {
            List<Option> ListOption = new List<Option>();
            try
            {
                USP.AddParameter(SPParameter.BeginTime, BeginTime);
                USP.AddParameter(SPParameter.EndTime, EndTime);
                ListOption = USP.ExeProcedureGetObjectList(SP.GetListOption, new Option());
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
            }
            return ListOption;
        }//end GetListOption

        /// <summary>指定條件，撈取區間內的走勢資訊</summary>
        /// <param name="_WeekPoint"></param>
        /// <returns></returns>
        public List<Option> GetListOptionByWeekPoint(WeekPoint _WeekPoint)
        {
            List<Option> ListOption = new List<Option>();
            try
            {
                USP.AddParameter(SPParameter.Contract, _WeekPoint.Contract);
                USP.AddParameter(SPParameter.DueMonth, _WeekPoint.DueMonth);
                USP.AddParameter(SPParameter.OP, _WeekPoint.OP);
                ListOption = USP.ExeProcedureGetObjectList(SP.GetListOptionByWeekPoint, new Option());
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
            }
            return ListOption;
        }//end GetListOptionByWeekPoint

        /// <summary>監控價格，到達警戒時寄發信件</summary>
        /// 20170316 add by Dick 加入功能。
        public void ControlPrice()
        {
            CalendarDAO CalendarDB = new CalendarDAO();
            Calendar _Calendar = CalendarDB.GetCalendar(DateTime.Now);
            if (_Calendar.IsWorkDay)
            {
                TradeRecordDAO RecordDB = new TradeRecordDAO();
                List<TradeRecord> RecordList = RecordDB.GetTradeRecord();
                WeightedDAO WeightedDAO = new WeightedDAO();
                foreach (TradeRecord Recorde in RecordList)
                {
                    if (Recorde.IsMail)
                    {
                        Weighted _Weighted = WeightedDAO.GetWeighted();
                        Option Result = GetOptionByMonthAndContractAndOP(new WeekPoint() { DueMonth = Recorde.DueMonth, OP = Recorde.OP, Contract = Recorde.Contract });
                        if (Recorde.PyeongchangTime == DateTime.MinValue)
                        {
                            Recorde.PyeongchangTime = DateTime.Now;
                        }
                        CalculateStopPoint(RecordDB, Recorde, _Weighted, Result);
                    }
                }
            }
        }

        /// <summary>計算停損及移動停利</summary>
        /// 20170316 add by Dick 加入功能。
        /// <param name="RecordDB"></param>
        /// <param name="Recorde"></param>
        /// <param name="_Weighted"></param>
        /// <param name="Result"></param>
        public void CalculateStopPoint(TradeRecordDAO RecordDB, TradeRecord Recorde, Weighted _Weighted, Option Result)
        {
            decimal StopPrice = 0m;
            string WarningMessage = string.Empty;
            if (Convert.ToDecimal(Recorde.Price) == 0m)
            {
                return;
            }
            if (Recorde.Type == TradeType.Sell.ToString())
            {
                StopPrice = this.CalculateStopPrice(Convert.ToDecimal(Recorde.Price), Recorde.Contract, _Weighted.Futures);
                decimal DynamicStopPrice = StopPrice;

                #region 隨時間讓停損價格流失
                CalendarDAO CalendarDB = new CalendarDAO();
                Calendar _Calendar = CalendarDB.GetDueMonthWeekStart(Recorde.DueMonth);
                int Days = DateTime.Now.Subtract(_Calendar.Daily).Days == 0 ? 1 : DateTime.Now.Subtract(_Calendar.Daily).Days;
                if (Days > 1)
                {
                    DynamicStopPrice = StopPrice - (1.1m * Days * 1.7m);
                }
                #endregion

                if ((Result.Clinch + 5) > DynamicStopPrice)
                {
                    Console.WriteLine("Warning");
                }
                else
                {
                    Recorde.Settlement = ((Recorde.Price - Result.Clinch) * Convert.ToInt32(Recorde.Lot)) - 2;
                    RecordDB.UpdateTradeRecord(Recorde);
                }
            }
            else
            {
                ///這邊要做停損跟停利的計算
                StopPrice = this.CalculateBuyStopPrice(Convert.ToDecimal(Recorde.Price));
                if ((Result.Clinch) < (StopPrice + 2))
                {
                    SendWarningMail(Recorde, StopPrice, Result, WarningMessage, Default.StopWarning);
                }
                else
                {
                    int NewLevel = 0;
                    decimal ButtomStopPrice = 0m;
                    decimal TopStopPrice = 0m;
                    for (int i = 0; i <= 50; i++)
                    {
                        ButtomStopPrice = ((StopPrice * ((1.1m) + (i * 0.1m))) - 1);
                        TopStopPrice = ((StopPrice * ((1.1m) + (i * 0.1m))) + 1m) + i * 3;
                        if (ButtomStopPrice < Result.Clinch && Result.Clinch < TopStopPrice)
                        {
                            NewLevel = i;
                        }
                    }
                    if (Recorde.Level == 0)
                    {
                        StopPrice = this.CalculateBuyStopPrice(Convert.ToDecimal(Recorde.Price));
                    }
                    else
                    {
                        StopPrice = this.CalculateBuyStopPrice(Convert.ToDecimal(((StopPrice * ((1.1m) + (Recorde.Level * 0.1m))) + 1)));
                        //過10額外加乘
                        int OverTen = Recorde.Level / 10;
                        StopPrice += OverTen * 5 * 1.7m;
                    }
                    if (NewLevel < Recorde.Level)
                    {
                        if (Result.Clinch <= StopPrice + 2)
                        {
                            SendWarningMail(Recorde, StopPrice, Result, WarningMessage, string.Format("跌落到第{0}階梯停利", NewLevel));
                        }
                    }
                    else
                    {
                        if (NewLevel > Recorde.Level)
                        {
                            Recorde.Level = NewLevel;
                            Recorde.Settlement = ((Result.Clinch - Recorde.Price) * Convert.ToInt32(Recorde.Lot)) - 2;
                            RecordDB.UpdateTradeRecord(Recorde);
                            SendWarningMail(Recorde, StopPrice, Result, WarningMessage, string.Format("目前到第{0}階梯", Recorde.Level));
                        }
                    }
                }
            }
        }

        /// <summary>寄送警告信件</summary>
        /// <param name="Recorde"></param>
        /// <param name="StopPrice"></param>
        /// <param name="Result"></param>
        /// <param name="WarningMessage"></param>
        private void SendWarningMail(TradeRecord Recorde, decimal StopPrice, Option Result, string WarningMessage, string Title)
        {
            WarningMessage = string.Format(" 操作價格: {3} 目前價格: {4} 停損價格: {5} 買/賣: {1} 方向: {2} 契約: {0}", Recorde.Contract, Recorde.Type, Recorde.OP,
                Convert.ToDecimal(Recorde.Price).ToString("#.00"), Result.Clinch.ToString("#.00"), StopPrice.ToString("#.00"));
            WarningMail(WarningMessage, Title);
        }

        /// <summary>警告信件</summary>
        /// <param name="WarningMessage"></param>
        private void WarningMail(string WarningMessage, string Title, bool IsLine = true)
        {
            CommTool.MailData MailDB = new CommTool.MailData();
            DataTable MaillDataTable = MailDB.GetSendMail();
            if (MaillDataTable != null && MaillDataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in MaillDataTable.Rows)
                {
                    WarningMessage = string.Format("發送位置  {0} 』 {1}", dr.ItemArray[3], WarningMessage);
                    DataTable MessageLog = MailDB.GetSendMessageLog(DateTime.Now);
                    var Temp = MessageLog.Select(string.Format("Message ='{0}'", WarningMessage)).ToList();
                    if (!Temp.Any())
                    {
                        if (IsLine)
                        {
                            MessageObj Obj = new MessageObj();
                            Obj.SendLineMessage(WarningMessage);
                            MailDB.AddSendMessageLog(DateTime.Now, WarningMessage);
                        }
                        else
                        {
                            MailDB.RegistrySend(dr[1].ToString(), Title, WarningMessage);
                        }
                    }
                }
            }
        }

        /// <summary>取得當前報價</summary>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="DueMonth"></param>
        /// <returns></returns>
        public List<Option> GetOptionQuotesByDuMonthAndTime(DateTime BeginTime, DateTime EndTime, string DueMonth)
        {
            List<Option> OptionList = new List<Option>();
            try
            {
                USP.AddParameter(SPParameter.BeginTime, BeginTime.ToString(Default.TimeFormat));
                USP.AddParameter(SPParameter.EndTime, EndTime.ToString(Default.TimeFormat));
                USP.AddParameter(SPParameter.DueMonth, DueMonth);
                OptionList = USP.ExeProcedureGetObjectList(SP.GetOptionQuotesByDuMonthAndTime, new Option()) as List<Option>;
            }
            catch (Exception ex)
            {
                CommTool.ToolLog.Log(ex);
            }
            return OptionList;
        }//end GetOptionQuotesByDuMonthAndTime

        /// <summary>抓取反轉訊號(未完成)</summary>
        /// <param name="RecordDB"></param>
        /// <param name="Recorde"></param>
        /// <param name="_Weighted"></param>
        /// <param name="Result"></param>
        public void CalculateStopPointSimulation(WeekPoint CallRecorde, WeekPoint PutRecorde, Weighted _Weighted, Option Result, Option PutResult, DateTime BeginTime, ref string WinOp)
        {
            bool Trans = false;
            string NowWinOp = string.Empty;
            string WarningMessage = string.Empty;

            if (Convert.ToDecimal(CallRecorde.Price) == 0m || Convert.ToDecimal(PutRecorde.Price) == 0m)
            {
                return;
            }
            if (Result.Clinch > PutResult.Clinch)
            {
                NowWinOp = "Call";
            }
            else
            {
                NowWinOp = "Put";
            }
            if (NowWinOp != WinOp)
            {
                Trans = true;
                WinOp = NowWinOp;
            }

            int Days = Convert.ToDateTime(Result.Time).Subtract(BeginTime).Days == 0 ? 1 : Convert.ToDateTime(Result.Time).Subtract(BeginTime).Days;
            if (Days > 1)
            {
                Console.WriteLine(string.Format(" Time :{0}  ,Call : {1}  , Put : {2} ", Result.Time, Result.Clinch, PutResult.Clinch));
                Thread.Sleep(1000);

                if (WinOp == "Call")
                {
                    decimal CallPrice = Convert.ToDecimal(CallRecorde.Price);
                    decimal Sub = 0;
                    if (30 > CallPrice && CallPrice > 20)
                    {
                        Sub = CallPrice - (decimal)(Days * 1.5);
                    }
                    if (50 > CallPrice && CallPrice > 30)
                    {
                        Sub = CallPrice - (decimal)(Days * 2.7);
                    }
                    if (70 > CallPrice && CallPrice > 50)
                    {
                        Sub = CallPrice - (decimal)(Days * 4.9);
                    }
                    if (PutResult.Clinch > Result.Clinch)
                    {
                    }
                    if (Sub != 0 && PutResult.Clinch > Sub && ((PutResult.Clinch - Result.Clinch) > Sub) && Trans)
                    {
                        MessageObj Obj = new MessageObj();
                        Obj.SendLineMessage("Call →  Put ");
                    }
                }
                else
                {
                    decimal PutPrice = Convert.ToDecimal(PutRecorde.Price);
                    decimal Sub = 0;
                    if (30 > PutPrice && PutPrice > 20)
                    {
                        Sub = PutPrice - (decimal)(Days * 1.5);
                    }
                    if (50 > PutPrice && PutPrice > 30)
                    {
                        Sub = PutPrice - (decimal)(Days * 2.7);
                    }
                    if (70 > PutPrice && PutPrice > 50)
                    {
                        Sub = PutPrice - (decimal)(Days * 4.9);
                    }
                    if (Result.Clinch > PutResult.Clinch)
                    {
                    }
                    if (Sub != 0 && Result.Clinch > Sub && ((Result.Clinch - PutResult.Clinch) > Sub) && Trans)
                    {
                        MessageObj Obj = new MessageObj();
                        Obj.SendLineMessage("Put → Call ");
                    }
                }
            }
        }

        /// <summary>計算停損及移動停利</summary>
        /// 20170316 add by Dick 加入功能。
        /// <param name="RecordDB"></param>
        /// <param name="Recorde"></param>
        /// <param name="_Weighted"></param>
        /// <param name="Result"></param>
        public void CalculateStopTestPoint(TradeRecordDAO RecordDB, TradeRecord Recorde, Weighted _Weighted, Option Result, StringBuilder SB, DateTime Begin, DateTime End, ref decimal MyAccount, ref decimal TempPrice)
        {
            decimal StopPrice = 0m;
            string WarningMessage = string.Empty;
            if (Convert.ToDecimal(Recorde.Price) == 0m)
            {
                return;
            }
            if (Recorde.Type == TradeType.Sell.ToString())
            {
                StopPrice = this.CalculateStopPrice(Convert.ToDecimal(Recorde.Price), Recorde.Contract, _Weighted.Futures);
                decimal DynamicStopPrice = StopPrice;

                #region 隨時間讓停損價格流失
                CalendarDAO CalendarDB = new CalendarDAO();
                Calendar _Calendar = CalendarDB.GetDueMonthWeekStart(Recorde.DueMonth);
                //int Days = DateTime.Now.Subtract(_Calendar.Daily).Days == 0 ? 1 : DateTime.Now.Subtract(_Calendar.Daily).Days;

                int Days = Recorde.TradeDate.Subtract(_Calendar.Daily).Days == 0 ? 1 : Recorde.TradeDate.Subtract(_Calendar.Daily).Days;

                if (Days > 1)
                {
                    DynamicStopPrice = StopPrice - (1.1m * Days * 1.7m);
                }
                #endregion

                if ((Result.Clinch + 5) > DynamicStopPrice)
                {
                    ////SendWarningMail(Recorde, DynamicStopPrice, Result, WarningMessage, Default.StopWarning);
                    if (Result.Clinch > DynamicStopPrice)
                    {
                        Recorde.Type = "Buy";
                        Recorde.ChangeTimes++;

                        decimal TempPoint = ((Recorde.Price - DynamicStopPrice) * 50) * Convert.ToInt32(Recorde.Lot);
                        MyAccount += TempPoint;
                        Recorde.Price = DynamicStopPrice;
                        Recorde.Lot = Convert.ToString(5);

                        string str = string.Format("To exchange from sell to buy then the price is {0} ", DynamicStopPrice);
                        SB.AppendLine(str);
                        Console.WriteLine(str);
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        if (TempPrice != DynamicStopPrice)
                        {
                            string str = string.Format("Warning.....價格逼近 Sell 的停損 , 停損價格=> {0}", DynamicStopPrice);
                            SB.AppendLine(str);
                            Console.WriteLine(str);
                            Thread.Sleep(1000);
                            if (TempPrice == 0)
                            {
                                TempPrice = DynamicStopPrice;
                            }
                        }
                        else
                        {
                            TempPrice = DynamicStopPrice;
                        }
                    }
                }
                else
                {
                    Recorde.Settlement = ((Recorde.Price - Result.Clinch) * Convert.ToInt32(Recorde.Lot)) - 2;
                    RecordDB.UpdateTradeRecord(Recorde);
                }
            }
            else
            {
                ///這邊要做停損跟停利的計算
                StopPrice = this.CalculateBuyStopPrice(Convert.ToDecimal(Recorde.Price));
                if ((Result.Clinch) < (StopPrice + 2))
                {
                    if (Result.Clinch < StopPrice)
                    {
                        MyAccount += ((StopPrice - Recorde.Price) * 50) * Convert.ToInt32(Recorde.Lot);
                        Recorde.ChangeTimes++;
                        Recorde.Type = "Sell";
                        Recorde.Lot = 3.ToString();
                        Recorde.Price = StopPrice;
                        string str = string.Format("To exchange from buy to sell then the price is {0} ", StopPrice);
                        SB.AppendLine(str);
                        Console.WriteLine(str);
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        string st = string.Format("Warning....The price changed, it neans you need to do some thing => {0} ", StopPrice);
                        SB.AppendLine(st);
                        Console.WriteLine(st);
                        Thread.Sleep(300);
                    }
                    SendWarningMail(Recorde, StopPrice, Result, WarningMessage, SB.ToString());
                }
                else
                {
                    int NewLevel = 0;
                    decimal ButtomStopPrice = 0m;
                    decimal TopStopPrice = 0m;
                    for (int i = 0; i <= 50; i++)
                    {
                        ButtomStopPrice = ((StopPrice * ((1.1m) + (i * 0.1m))) - 1);
                        TopStopPrice = ((StopPrice * ((1.1m) + (i * 0.1m))) + 1m) + i * 3;
                        if (ButtomStopPrice < Result.Clinch && Result.Clinch < TopStopPrice)
                        {
                            NewLevel = i;
                        }
                    }
                    if (Recorde.Level == 0)
                    {
                        StopPrice = this.CalculateBuyStopPrice(Convert.ToDecimal(Recorde.Price));
                    }
                    else
                    {
                        StopPrice = this.CalculateBuyStopPrice(Convert.ToDecimal(((StopPrice * ((1.1m) + (Recorde.Level * 0.1m))) + 1)));
                    }
                    if (NewLevel < Recorde.Level)
                    {
                        if (Result.Clinch <= StopPrice + 2)
                        {
                            if (StopPrice > Result.Clinch)
                            {
                                decimal TempPoint = ((Recorde.Price - StopPrice) * 50 - 18) * Convert.ToInt32(Recorde.Lot);
                                MyAccount += TempPoint;
                                Recorde.Type = "Sell";
                                Recorde.Price = StopPrice;
                                Recorde.ChangeTimes++;
                                Recorde.Lot = Convert.ToString(3);
                                string str = string.Format("To exchange from buy to sell then the price is {0} ", StopPrice);
                                SB.AppendLine(str);
                                Console.WriteLine(str);
                                Thread.Sleep(2000);
                            }
                            else
                            {
                                string str = string.Format("It downs to level{0}, it needs to be stopping", NewLevel);
                                SB.AppendLine(str);
                                Console.WriteLine(str);
                                Thread.Sleep(1500);
                            }
                            SendWarningMail(Recorde, StopPrice, Result, WarningMessage, SB.ToString());
                        }
                        //if ((StopPrice + 8) > Result.Clinch)
                        //{
                        //    //SendWarningMail(Recorde, StopPrice, Result, WarningMessage, string.Format("跌落到第{0}階梯", NewLevel));
                        //}
                    }
                    else
                    {
                        if (NewLevel > Recorde.Level)
                        {
                            Recorde.Level = NewLevel;
                            Recorde.Settlement = ((Result.Clinch - Recorde.Price) * Convert.ToInt32(Recorde.Lot)) - 2;
                            RecordDB.UpdateTradeRecord(Recorde);
                            string str = string.Format("It rise to new level =>{0}", NewLevel);
                            SB.AppendLine(str);
                            //Console.WriteLine(str);                            
                            //SendWarningMail(Recorde, StopPrice, Result, WarningMessage, string.Format("目前到第{0}階梯", Recorde.Level));
                            SendWarningMail(Recorde, StopPrice, Result, WarningMessage, SB.ToString());
                            Thread.Sleep(2000);
                        }
                    }
                }
            }
        }
    }
}
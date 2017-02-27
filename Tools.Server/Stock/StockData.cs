using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Reflection;
namespace Stock {
    public class StockData {
        private class Default {
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
        }

        private class SP {
            public const string SaveOption = "SaveOption";
            public const string SaveOptionHistory = "SaveOptionHistory";
            public const string SaveWeighted = "SaveWeighted";
            public const string GetDueMonth = "GetDueMonth";
            public const string GetOptionHistory = "GetOptionHistory";
            public const string SaveOpenInterest = "SaveOpenInterest";
            public const string GetMaxNumberOfContracts = "GetMaxNumberOfContracts";
            public const string GetMaxVolume = "GetMaxVolume";
            public const string GetWeighted = "GetWeighted";
            public const string AddWeekPoint = "AddWeekPoint";
        }

        private class SPParameter {
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
        }

        private class OptionHistory {
            public const string TradeDate = "TradeDate";
            public const string DueMonth = "DueMonth";
            public const string Price = "Price";
            public const string Option="Option";
            public const string Opening_Price = "Opening_Price";
            public const string Highest = "Highest";
            public const string Lowest = "Lowest";
            public const string Closing = "Closing";
            public const string Volume = "Volume";
            public const string Settlement = "Settlement";
            public const string NumberOfContracts = "NumberOfContracts";
            public const string History_Highest = "History_Highest";
            public const string History_Lowest = "History_Lowest";
            public const string Contract = "Contract";
            public const string GreatBuy = "GreatBuy";
            public const string GreatSell = "GreatSell";
            public const string Remark = "Remark";
        }

        private class WeightedHistory{
            public const string TradeDate = "TradeDate";
            public const string OpenPrice="OpenPrice";
            public const string HighestPrice="HighestPrice";
            public const string LowestPrice="LowestPrice";
            public const string ClosingPrice="ClosingPrice";
            public const string Price = "Price";
            public const string Change = "Change";
            public const string Futures = "Futures";
            public const string Remark="Remark";
            public const string Volume = "Volume";
        }

        private class OpenInterest {
            public const string SN = "SN";
            public const string TradeDate = "TradeDate";
            public const string PutVolume = "PutVolume";
            public const string CallVolume = "CallVolume";
            public const string Ratios = "Ratios";
            public const string PutOpenInterest = "PutOpenInterest";
            public const string CallOpenInterest = "CallOpenInterest";
            public const string OpenInterestRatios = "OpenInterestRatios";
        }

        private string _stockNum;

        private string _url;

        private Stock _stock;

        #region  Parameter
        public string StockNum {
            set {
                _stockNum = value;
            }
        }

        public string URL {
            set {
                _url = value;
            }
        }

        #endregion
        SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();
        
        //public StockData(string pStockNum) {
        //    _stock = new Stock();
        //    StockNum = pStockNum;
        //    _stock.StockNum = this._stockNum;
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic[Default.sqlconnection] = string.Empty;
        //    SQLHelper.SHelper.InitSHelper(dic);
        //}
        
        #region public mothed

        public Stock GetStockData(string Url, string ConnetionString) {
            try {
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
            catch (Exception ex) {
                #region  20150324 modifed 修改為新的方式撰寫SQL
                string sql = string.Format(@"insert into ErrorLog values({0},{1},{2},{3})", "001", ex.Message, DateTime.Now, this._stock.StockNum);
                SQLHelper.SHelper.ExeNoQuery(sql);
                #endregion
            }
            return this._stock;
        }

        public void SetkData(string ConnetionString) {
            if (this._stock.IsSucess) {
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
        public void RefreshList() {
            for (int i = 1100; i < 9999; i++) {
                pRefreshList(i.ToString());
                if (i % 5 == 0) {
                    System.Threading.Thread.Sleep(Default.Second);
                }
            }
        }

        private string RefreshStockList() {
            try {
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
                for (int i = 3; i < arry.Length; i++) {
                    string[] parry = Regex.Split(arry[i], "td", RegexOptions.IgnoreCase);
                    if (parry.Length > 8) {
                        sb.Append(parry[5].Replace("bgcolor=\"#FFFAE8\">", string.Empty).Replace("</", string.Empty).Replace("height=\"25\"", string.Empty).Trim());
                        sb.Append(parry[7].Replace("align=\"center\">", string.Empty).Replace("</", string.Empty));
                        sb.Append(",");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex) {
                CommTool.ToolLog log = new CommTool.ToolLog();
                CommTool.ToolLog.Log(CommTool.LogType.Error, _stockNum.ToString());
                return "error";
            }
        }

        private void pRefreshList(string pCode) {
            this.StockNum = pCode;
            this.URL = "http://tw.stock.yahoo.com/d/s/company_";
            string temp = this.RefreshStockList();
            if (temp != "error") {
                string[] arry = temp.Split(',');
                #region  20150324 modifed by Dick for 修改維新的方式撰寫SQL
                string sql = string.Format(@"select count(*) from StockList where StockNum={0} and EPS1={1} and EPS2={2} and EPS3 ={3} and EPS4 ={4} ", pCode, arry[1], arry[2], arry[3], arry[4]);
                DataTable dt = SQLHelper.SHelper.ExeDataTable(sql);
                if (dt != null && dt.Rows.Count > 0) {
                    int tag = Convert.ToInt32(dt.Rows[0][0]);
                    if (tag == 0) {
                        sql = string.Format(@"Insert into  StockList values({0},{1},{2},{3},{4},{5},{6}) ", pCode, arry[0], arry[1], arry[2], arry[3], arry[4], DateTime.Now.Date);
                        SQLHelper.SHelper.ExeNoQuery(sql);
                    }
                }
                #endregion
            }
        }

        /// <summary>追蹤EPS 連4季 都是上升的股票 最佳進步獎
        /// </summary>
        /// <returns></returns>
        public string TraceEPS() {
            #region  20150324 modifed by Dick for 修改維新的方式撰寫SQL
            DataTable dt = SQLHelper.SHelper.ExeDataTable("Select StockNum,StockName,EPS1,EPS2,EPS3,EPS4 from StockList Group by StockNum,StockName,EPS1,EPS2,EPS3,EPS4");
            StringBuilder sb = new StringBuilder();
            if (dt != null) {
                foreach (DataRow dr in dt.Rows) {
                    double EPS1 = Convert.ToDouble(EPSSplit(dr[2].ToString()));
                    double EPS2 = Convert.ToDouble(EPSSplit(dr[3].ToString()));
                    double EPS3 = Convert.ToDouble(EPSSplit(dr[4].ToString()));
                    double EPS4 = Convert.ToDouble(EPSSplit(dr[5].ToString()));
                    if (EPS1 > EPS2 & EPS2 > EPS3 & EPS3 > EPS4) {
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

        private string EPSSplit(string eps) {
            int start = eps.IndexOf("季");
            int end = eps.IndexOf("元");
            int deff = end - start;
            if (deff > 0) {
                string temp = eps.Substring(start + 1, deff - 1);
                return temp;
            }
            else {
                return "0";
            }
        }

        /// <summary>抓取當日的資料</summary>
        /// 20170126 modifed by Dick
        /// 20170202 修正由這邊控制抓取資料的時間區間 modifed by Dick
        /// 20170203 加入抓取大盤資料的功能
        /// 20170208 加入發送Maill的功能
        /// <param name="Url"></param>
        /// <returns></returns>
        public dynamic GetOptionEveryDay(string Url) {
            CalendarData CalendarDB = new CalendarData();
            DateTime TimeStamp =DateTime.Now ;
            Calendar _Calendar = CalendarDB.GetCalendar(TimeStamp);
            string Message = "NotTradeTime";
            if (_Calendar.IsWorkDay) {
                TimeSpan StartTimeSpan = TimeStamp.Subtract(new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 8, 44,59));
                TimeSpan EndTimeSpan = TimeStamp.Subtract(new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 13, 45,15));
                if (StartTimeSpan.TotalSeconds >= 0 && EndTimeSpan.TotalSeconds <= 0) {
                    try {
                        List<Option> ListOption = new List<Option>();
                        ListOption.AddRange(GetOptionDaily(Url, _Calendar.Week, Encoding.UTF8, true)); //周選  
                        ListOption.AddRange(GetOptionDaily(Url, _Calendar.NearMonth1, Encoding.UTF8)); //近月選1                               
                        ListOption.AddRange(GetOptionDaily(Url, _Calendar.NearMonth2, Encoding.UTF8)); //近月選2                              
                        SaveOpionData(ListOption);
                        Message = "GetOptionOK";
                        CommTool.ToolLog.Log(Message);
                        GetNumberOfContractsAndMaill();
                        return Message;
                    }
                    catch (Exception ex) {
                        CommTool.ToolLog.Log(ex);
                        Message = "Error";
                        return Message;
                    }
                }
                else {
                    CommTool.ToolLog.Log(Message);
                    return Message;
                }
            }
            else {
                Message = "NotWorkDay";
                CommTool.ToolLog.Log(Message);
                return Message;
            }
        }

        /// <summary>取得選擇權價格清單，預設編碼</summary>
        /// <param name="Url">POST位置</param>
        /// <returns></returns>
        public List<Option> GetOptionDaily(string Url, string Contract) {
            return GetOptionDaily(Url, Contract, Encoding.Default);
        }
        
        /// <summary>取得選擇權價格清單</summary>
        /// 20161110 add by Dick
        /// 20170126 modifed by Dick
        /// 20170202 修正網頁上資訊不足時出現陣列長度錯誤  modifed by Dick
        /// <param name="Url"></param>
        /// <param name="Contract"></param>
        /// <param name="UrlEncoding"></param>
        /// <param name="IsGetWeighed">是否抓取大盤</param>
        /// <returns></returns>
        public List<Option> GetOptionDaily(string Url, string Contract, Encoding UrlEncoding,bool IsGetWeighed=false) {
            WebInfo.WebInfo Info = new WebInfo.WebInfo();
            List<Option> list = new List<Option>();
            StreamReader SR = Info.GetResponse(Url + Contract, UrlEncoding);
            HtmlAgilityPack.HtmlDocument _HtmlDocument = new HtmlAgilityPack.HtmlDocument();
            _HtmlDocument.LoadHtml(SR.ReadToEnd());
            SR.Close();
            HtmlAgilityPack.HtmlNodeCollection anchors = _HtmlDocument.DocumentNode.SelectNodes(Default.TableTag);
            if (anchors != null) {
                if (anchors.Count > 3) {
                    HtmlAgilityPack.HtmlNodeCollection Nodes = anchors[3].SelectNodes(Default.HtmlTr);
                    if (Nodes != null) {
                        if (Nodes.Count >= 45) {
                            for (int i = 29; i <= 45; i++) {
                                Option Call = new Option();
                                Call.OP = Default.Call;
                                Call.DueMonth = Contract;
                                Call.buy = Nodes[i].ChildNodes[1].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[1].InnerText);
                                Call.sell = Nodes[i].ChildNodes[3].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[3].InnerText);
                                Call.clinch = Nodes[i].ChildNodes[5].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[5].InnerText);
                                Call.Change = Nodes[i].ChildNodes[7].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[7].InnerText);
                                Call.NumberOfContracts = Nodes[i].ChildNodes[9].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToInt32(Nodes[i].ChildNodes[9].InnerText);
                                Call.Volume = Nodes[i].ChildNodes[11].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToInt32(Nodes[i].ChildNodes[11].InnerText);
                                Call.Time = Nodes[i].ChildNodes[13].InnerText.Trim() == Default.NullSing ? DateTime.Now.ToString(CommTool.BaseConst.TimeFormatComplete) : Convert.ToDateTime(Nodes[i].ChildNodes[13].InnerText).ToString(CommTool.BaseConst.TimeFormatComplete);
                                Call.Contract = Nodes[i].ChildNodes[15].InnerText.Trim();
                                list.Add(Call);
                                Option Put = new Option();
                                Put.OP = Default.Put;
                                Put.DueMonth = Contract;
                                Put.Contract = Nodes[i].ChildNodes[15].InnerText.Trim();
                                Put.buy = Nodes[i].ChildNodes[17].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[17].InnerText);
                                Put.sell = Nodes[i].ChildNodes[19].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[19].InnerText);
                                Put.clinch = Nodes[i].ChildNodes[21].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[21].InnerText);
                                Put.Change = Nodes[i].ChildNodes[23].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[23].InnerText);
                                Put.NumberOfContracts = Nodes[i].ChildNodes[25].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToInt32(Nodes[i].ChildNodes[25].InnerText);
                                Put.Volume = Nodes[i].ChildNodes[27].InnerText.Trim() == Default.NullSing ? 0 : Convert.ToInt32(Nodes[i].ChildNodes[27].InnerText);
                                Put.Time = Nodes[i].ChildNodes[29].InnerText.Trim() == Default.NullSing ? DateTime.Now.ToString(CommTool.BaseConst.TimeFormatComplete) : Convert.ToDateTime(Nodes[i].ChildNodes[29].InnerText).ToString(CommTool.BaseConst.TimeFormatComplete);
                                list.Add(Put);
                            }
                        }
                    }
                }
            }
            if (IsGetWeighed) {
                Weighted _Weighted = GetWeightedDaily(_HtmlDocument);
                if (_Weighted != null) {
                    SaveWeighted(_Weighted);
                }
                else {
                    CommTool.ToolLog.Log("Weighted Is Null");
                }
            }
            _HtmlDocument = null;
            return list;
        }

        /// <summary>取得大盤價格走勢</summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public Weighted GetWeightedDaily(string Url)
        {
            WebInfo.WebInfo Info = new WebInfo.WebInfo();
            HtmlAgilityPack.HtmlDocument Doc =  Info.GetWebHtmlDocument(Url,Encoding.UTF8);           
            return GetWeighted(Doc);
        }

        /// <summary>取得大盤價格走勢</summary>
        /// <param name="Doc"></param>
        /// <returns></returns>
        public Weighted GetWeightedDaily(HtmlAgilityPack.HtmlDocument Doc) {
            return GetWeighted(Doc);
        }
        
        /// <summary>取得大盤價格走勢</summary>
        /// 20170203 抓取大盤走勢的功能，同期貨的一起抓  add by Dick 
        /// 20170209 修改TradeDate改成當下時間 modified by Dick
        /// <param name="Url"></param>
        /// <returns></returns>
        private Weighted GetWeighted(HtmlAgilityPack.HtmlDocument Doc) {
            DateTime TimeStamp = DateTime.Now;
            Weighted _Weighted = null;
            TimeSpan StartTimeSpan = TimeStamp.Subtract(new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 8, 59, 59));
            TimeSpan EndTimeSpan = TimeStamp.Subtract(new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 13, 45, 15));
            if (StartTimeSpan.TotalSeconds >= 0 && EndTimeSpan.TotalSeconds <= 0) {
                HtmlNode Tr = Doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/body[1]/table[1]/tbody[1]/tr[1]");
                HtmlNode NodeWeighted = Doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/body[1]/table[2]/tbody[1]/tr[1]");
                HtmlNode NearMonth = Doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/body[1]/table[2]/tbody[1]/tr[2]");
                if (Tr != null) {
                    try {
                        _Weighted = new Weighted();
                        int Start = Tr.ChildNodes[3].InnerText.IndexOf("（");
                        int End = Tr.ChildNodes[3].InnerText.IndexOf("）");
                        _Weighted.Price = decimal.Parse(Tr.ChildNodes[3].InnerText.Substring(0, Start));
                        _Weighted.Change = decimal.Parse(Tr.ChildNodes[3].InnerText.Trim().Substring(Start + 1, End - Start - 1));
                        _Weighted.HighestPrice = decimal.Parse(Tr.ChildNodes[7].InnerText);
                        _Weighted.LowestPrice = decimal.Parse(Tr.ChildNodes[11].InnerText);
                        _Weighted.Volume = Tr.ChildNodes[15].InnerText.Trim().Replace("（億）", string.Empty).Replace("\t", string.Empty); //過濾掉不必要的字元
                        if (NodeWeighted != null) {
                            _Weighted.Futures = decimal.Parse(NearMonth.ChildNodes[3].InnerText);
                            _Weighted.TradeDate = DateTime.Now;
                        }
                        if (NodeWeighted != null) {
                            _Weighted.OpenPrice = decimal.Parse(NodeWeighted.ChildNodes[15].InnerText);
                            _Weighted.ClosingPrice = decimal.Parse(NodeWeighted.ChildNodes[7].InnerText);
                        }
                    }
                    catch (Exception ex) {
                        CommTool.ToolLog.Log(ex);
                        _Weighted = null;
                    }
                }
            }
            return _Weighted;
        }

        /// <summary>儲存選擇權交易歷史資訊</summary>
        /// <param name="Options"></param>
        public void SaveOpionData(List<Option> Options) {         
            try {               
                foreach (Option op in Options) {
                    if (op.Volume > 0) {
                        USP.AddParameter(SPParameter.OP, op.OP);
                        USP.AddParameter(SPParameter.Buy, op.buy);
                        USP.AddParameter(SPParameter.Change, op.Change);
                        USP.AddParameter(SPParameter.Clinch, op.clinch);
                        USP.AddParameter(SPParameter.Contract, op.Contract);               
                        USP.AddParameter(SPParameter.Sell, op.sell);
                        USP.AddParameter(SPParameter.Time, op.Time);
                        USP.AddParameter(SPParameter.DueMonth, op.DueMonth);
                        USP.AddParameter(SPParameter.NumberOfContracts, op.NumberOfContracts);
                        USP.AddParameter(SPParameter.Volume, op.Volume);
                        USP.ExeProcedureNotQuery(SP.SaveOption);
                    }
                }
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
            }
        }

        /// <summary>儲存選擇權交易歷史資訊</summary>
        /// <param name="dt"></param>
        public void SaveOptionHistoryData(DataTable dt) {
            try {
                foreach (DataRow dr in dt.Rows) {
                    decimal Temp = Convert.ToDecimal(dr[OptionHistory.Opening_Price]);
                    if (Temp != 0) {
                        USP.AddParameter(OptionHistory.TradeDate, dr[OptionHistory.TradeDate]);
                        USP.AddParameter(OptionHistory.Contract, dr[OptionHistory.Contract]);
                        USP.AddParameter(OptionHistory.DueMonth, dr[OptionHistory.DueMonth]);
                        USP.AddParameter(OptionHistory.Price, dr[OptionHistory.Price]);
                        USP.AddParameter(OptionHistory.Option, dr[OptionHistory.Option]);
                        USP.AddParameter(OptionHistory.Opening_Price, dr[OptionHistory.Opening_Price]);
                        USP.AddParameter(OptionHistory.Highest, dr[OptionHistory.Highest]);
                        USP.AddParameter(OptionHistory.Lowest, dr[OptionHistory.Lowest]);
                        USP.AddParameter(OptionHistory.Closing, dr[OptionHistory.Closing]);
                        USP.AddParameter(OptionHistory.Volume, dr[OptionHistory.Volume]);
                        USP.AddParameter(OptionHistory.Settlement, dr[OptionHistory.Settlement]);
                        USP.AddParameter(OptionHistory.NumberOfContracts, dr[OptionHistory.NumberOfContracts]);
                        USP.AddParameter(OptionHistory.GreatBuy, dr[OptionHistory.GreatBuy]);
                        USP.AddParameter(OptionHistory.GreatSell, dr[OptionHistory.GreatSell]);
                        USP.AddParameter(OptionHistory.History_Highest, dr[OptionHistory.History_Highest]);
                        USP.AddParameter(OptionHistory.History_Lowest, dr[OptionHistory.History_Lowest]);
                        USP.AddParameter(OptionHistory.Remark, dr[OptionHistory.Remark]);
                        USP.ExeProcedureNotQuery(SP.SaveOptionHistory);
                    }
                }
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
            }
        }
        
        /// <summary>儲存大盤歷史資料</summary>
        /// 20170203 加入新欄位    add by Dick
        /// <param name="dt"></param>
        public void SaveWeighted(DataTable dt) {
            try {
                foreach (DataRow dr in dt.Rows) {
                    USP.AddParameter(WeightedHistory.TradeDate, Convert.ToDateTime(dr[WeightedHistory.TradeDate]));
                    USP.AddParameter(WeightedHistory.ClosingPrice, dr[WeightedHistory.ClosingPrice]);
                    USP.AddParameter(WeightedHistory.HighestPrice, dr[WeightedHistory.HighestPrice]);
                    USP.AddParameter(WeightedHistory.LowestPrice, dr[WeightedHistory.LowestPrice]);
                    USP.AddParameter(WeightedHistory.OpenPrice, dr[WeightedHistory.OpenPrice]);
                    USP.AddParameter(WeightedHistory.Price, dr[WeightedHistory.Price]);
                    USP.AddParameter(WeightedHistory.Futures, dr[WeightedHistory.Futures]);
                    USP.AddParameter(WeightedHistory.Remark, dr[WeightedHistory.Remark]);
                    USP.ExeProcedureNotQuery(SP.SaveWeighted);
                }
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
            }
        }

        /// <summary>每周操作紀錄</summary>
        /// 20170208 add by Dick 
        /// 20170227 add by Dick for 追加新欄位
        /// <param name="_WeekPoint"></param>
        public void AddWeekPoint(WeekPoint _WeekPoint) {
            try {
                USP.AddParameter(WeightedHistory.TradeDate, _WeekPoint.TradeDate);
                USP.AddParameter(SPParameter.OP, _WeekPoint.OP);
                USP.AddParameter(SPParameter.Contract, _WeekPoint.Contract);
                USP.AddParameter(WeightedHistory.Price, _WeekPoint.Price);
                USP.AddParameter(SPParameter.Volume, _WeekPoint.Volume);
                USP.AddParameter(SPParameter.StopPrice, _WeekPoint.StopPirce);
                USP.AddParameter(SPParameter.DueMonth　, _WeekPoint.DueMonth);
                USP.AddParameter(SPParameter.BuyStopPrice, _WeekPoint.BuyStopPrice);
                USP.ExeProcedureHasResult(SP.AddWeekPoint);
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
            }
        }

        /// <summary>儲存大盤歷史資料</summary>
        public void SaveWeighted(Weighted _Weighted) {
            try {
                USP.AddParameter(WeightedHistory.TradeDate, _Weighted.TradeDate);
                USP.AddParameter(WeightedHistory.ClosingPrice, _Weighted.ClosingPrice);
                USP.AddParameter(WeightedHistory.HighestPrice, _Weighted.HighestPrice);
                USP.AddParameter(WeightedHistory.LowestPrice, _Weighted.LowestPrice);
                USP.AddParameter(WeightedHistory.OpenPrice, _Weighted.OpenPrice);
                USP.AddParameter(WeightedHistory.Price, _Weighted.Price);
                USP.AddParameter(WeightedHistory.Futures, _Weighted.Futures);
                USP.AddParameter(WeightedHistory.Change, _Weighted.Change);
                USP.AddParameter(WeightedHistory.Volume, _Weighted.Volume);
                USP.AddParameter(WeightedHistory.Remark, _Weighted.Remark == null ? string.Empty : _Weighted.Remark);
                USP.ExeProcedureNotQuery(SP.SaveWeighted);                 
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
            }
        }
        
        /// <summary>取得最新一筆大盤資訊</summary>
        /// <returns></returns>
        public Weighted GetWeighted() {
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetWeighted);
            Weighted _Weighted = new Weighted();
            if (dt != null && dt.Rows.Count > CommTool.BaseConst.MinItems) {
                DataRow Row = dt.Rows[CommTool.BaseConst.ArrayFirstItem];
                PropertyInfo[] infos = typeof(Weighted).GetProperties();
                foreach (PropertyInfo info in infos) {
                    _Weighted.GetType().GetProperty(info.Name).SetValue(_Weighted, Row[info.Name], null);
                }
                return _Weighted;
            }
            else {
                return null;
            }      
        }

        public DataTable SelectOptionHistory(string DueMonth,string Option,string Start,string End) {
            try {
                USP.AddParameter(OptionHistory.DueMonth, DueMonth);
                USP.AddParameter(OptionHistory.Option, Option);
                USP.AddParameter(SPParameter.Start, Start);
                USP.AddParameter(SPParameter.End, End);                
                return USP.ExeProcedureGetDataTable(SP.GetOptionHistory);
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
                return null;
            }             
        }

        /// <summary>取得周選契約</summary>
        /// <returns></returns>
        public DataTable GetDueMonth() {            
            try {
               return USP.ExeProcedureGetDataTable(SP.GetDueMonth);
            }
            catch (Exception ex) {
                return null;
            }
        }

        /// <summary>計算最大交易量的周選勝率及賺賠結果</summary>
        /// <param name="BaseMoney">雙SELL的組數</param>
        /// <param name="SmallPoint">最低買進點數</param>
        public void GetOptionWeek(int BaseMoney, int SmallPoint) {          
            DataTable Report = new DataTable();
            Report.Columns.Add(OptionHistory.TradeDate);
            Report.Columns.Add(OptionHistory.Option);
            Report.Columns.Add(OptionHistory.DueMonth);
            Report.Columns.Add(OptionHistory.Price);
            Report.Columns.Add(OptionHistory.Closing);
            Report.Columns.Add(WeightedHistory.OpenPrice);
            Report.Columns.Add(WeightedHistory.ClosingPrice);
            List<string> MonthsList = new List<string>();
            DateTime NowDate = new DateTime(2012, 11, 20);
            string[] Option = new string[] { "賣權", "買權" };
            DateTime Month = NowDate;
            DataTable DueMonthTable = this.GetDueMonth();
            if (DueMonthTable.Rows.Count > 0) {
                foreach (DataRow dr in DueMonthTable.Rows) {
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
            foreach (string Due in MonthsList) {
                weekcount++;
                decimal week = 0;
                PassDay += 6;
                foreach (string OP in Option) {
                    DateTime Temp = NowDate.AddDays(PassDay);
                    while (Temp <= DateTime.Now) {
                        Temp = Temp.AddDays(1);
                        if (!IsNoData) {
                            DataRow row = GETPrice(NewRow, OP, Temp, Due);
                            NowDueMont = Due;
                            NowPrice = row.ItemArray[3].ToString();
                            if (!string.IsNullOrEmpty(NowPrice)) {
                                IsNoData = true;
                                Report.Rows.Add(row);
                                var temp = Convert.ToDecimal(row.ItemArray[4]);
                                if (temp > SmallPoint) {
                                    StartPrice = temp;
                                }
                                else {
                                    IsPass = true;
                                }
                            }
                        }
                        else {
                            DataRow row = GetResultByPrice(NewRow, OP, Temp, Due, NowPrice);
                            if (row.ItemArray[3].ToString() != string.Empty) {
                                Report.Rows.Add(row);
                            }
                        }
                        if (Report.Rows.Count > 0) {
                            DataRow LastRow = Report.Rows[Report.Rows.Count - 1];
                            if (Convert.ToDateTime(LastRow.ItemArray[0]).AddMonths(2) <= Temp) {
                                break;
                            }
                        }
                    }
                    IsNoData = false;
                    if (!IsPass) {
                        if (Report.Rows.Count > 0) {
                            DataRow EndRow = Report.Rows[Report.Rows.Count - 1];
                            EndPrice = Convert.ToDecimal(EndRow.ItemArray[4].ToString());
                            week += (StartPrice - EndPrice)*BaseMoney;
                        }
                    } 
                    Report.Rows.Clear();
                    IsPass = false;
                }
                Result += week;
                if (week > 0) {
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
        public void GetOptionWeekWithStop(int BaseMoney, decimal SmallPoint,decimal StopBase) {
            DataTable Report = new DataTable();
            Report.Columns.Add(OptionHistory.TradeDate);
            Report.Columns.Add(OptionHistory.Option);
            Report.Columns.Add(OptionHistory.DueMonth);
            Report.Columns.Add(OptionHistory.Price);
            Report.Columns.Add(OptionHistory.Closing);
            Report.Columns.Add(WeightedHistory.OpenPrice);
            Report.Columns.Add(WeightedHistory.ClosingPrice);
            Report.Columns.Add(OptionHistory.Highest);
            Report.Columns.Add(OptionHistory.Opening_Price);
            List<string> MonthsList = new List<string>();
            DateTime NowDate = new DateTime(2012, 11, 20);
            string[] Option = new string[] { "賣權", "買權" };
            DateTime Month = NowDate;
            DataTable DueMonthTable = this.GetDueMonth();
            if (DueMonthTable.Rows.Count > 0) {
                foreach (DataRow dr in DueMonthTable.Rows) {
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
            foreach (string Due in MonthsList) {
                weekcount++;
                decimal week = 0;
                PassDay += 6;
                foreach (string OP in Option) {
                    decimal StopPrice = 0;
                    DateTime Temp = NowDate.AddDays(PassDay);
                    while (Temp <= DateTime.Now) {
                        Temp = Temp.AddDays(1);
                        if (!IsNoData) {
                            DataRow row = GETPrice(NewRow, OP, Temp, Due);
                            NowDueMont = Due;
                            NowPrice = row.ItemArray[3].ToString();
                            if (!string.IsNullOrEmpty(NowPrice)) {
                                IsNoData = true;
                                Report.Rows.Add(row);
                                var temp = Convert.ToDecimal(row.ItemArray[4]);
                                if (temp > SmallPoint) {
                                    StartPrice = temp;
                                    StopPrice = CalculateStopPrice(StartPrice, row.ItemArray[3].ToString(), Convert.ToDecimal(row.ItemArray[6].ToString()));
                                    CommTool.ToolLog.Log(string.Format("契約:{0} , 日期:{1} , Option :{2} , 價格:{3} ,停損價格 {4}", Due, row.ItemArray[0].ToString(), OP, temp, StopPrice));                                  
                                }
                                else {
                                    IsPass = true;
                                }
                            }
                        }
                        else {
                            DataRow row = GetResultByPrice(NewRow, OP, Temp, Due, NowPrice);
                            if (row.ItemArray[3].ToString() != string.Empty) {
                                Report.Rows.Add(row);
                            }
                        }
                        if (Report.Rows.Count > 0) {
                            DataRow LastRow = Report.Rows[Report.Rows.Count - 1];
                            if (Convert.ToDateTime(LastRow.ItemArray[0]).AddMonths(2) <= Temp) {
                                break;
                            }
                        }
                    }
                    IsNoData = false;
                    if (!IsPass) {
                        if (Report.Rows.Count > 0) {
                            decimal Loss = 0;
                            decimal BuyPoint = 0;
                            DataRow EndRow = Report.Rows[Report.Rows.Count - 1];
                            EndPrice = Convert.ToDecimal(EndRow.ItemArray[4].ToString());
                            bool IsOver = false;
                            for (int i = 1; i < Report.Rows.Count; i++) {
                                DataRow row = Report.Rows[i];
                                decimal TempPrice = Convert.ToDecimal(row.ItemArray[4].ToString());
                                decimal HighPrice = Convert.ToDecimal(row.ItemArray[7].ToString());
                                CommTool.ToolLog.Log(string.Format("契約:{0} , 日期:{1} , Option :{2} , 契約價格:{3} , 點數:{4} , 最高點{5}", Due, row.ItemArray[0].ToString(), OP, row.ItemArray[3].ToString(), TempPrice, HighPrice));
                                if (!IsOver) {
                                    if (TempPrice > (StopPrice + StartPrice) || HighPrice > (StopPrice + StartPrice)) {
                                        Loss += ((StopPrice + StartPrice) - StartPrice) + 10;
                                        StartPrice = (StopPrice + StartPrice);
                                        IsOver = true;
                                    }
                                }
                                else {
                                    if (TempPrice <= StartPrice) {
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
                            if (EndPrice > StartPrice) {
                                BuyPoint = (EndPrice - StartPrice) * 5;
                            }
                            else {
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
                    CommTool.ToolLog.Log(string.Empty);
                }
                Result += (week + buyPoint);
                buyPoint = 0;
                if (week > 0) {
                    win++;
                }
                string Message = string.Format("契約:{0} ,賺賠:{1},總賺賠：{2}", Due, week.ToString("00000.##"), Result.ToString("00000.##"));
                CommTool.ToolLog.Log(Message);
                CommTool.ToolLog.Log(string.Empty);
            }
            CommTool.ToolLog.Log(string.Format("勝率:{0}%", (win * 100 / weekcount)));
        }

        /// <summary>計算Sell停損價格</summary>
        /// <param name="Price">點數</param>
        /// <param name="Contact"></param>
        /// <param name="ClosePrice"></param>
        /// <returns></returns>
        private decimal CalculateStopPrice(decimal Price, string Contact, decimal ClosePrice) {
            decimal BasePrice =(Price * 50) + (17000 - (Convert.ToDecimal(Contact) - ClosePrice));
            return Price + ((BasePrice * decimal.Parse("0.1")) / 50);
        }

        /// <summary>計算Buy停損價格</summary>
        /// 20170227 add by Dick
        /// <param name="Price"></param>
        /// <returns></returns>
        private decimal CalculateBuyStopPrice(decimal Price) {
            return Price * (decimal)0.8;
        }
        
        /// <summary>取得最大交易量的契約</summary>
        /// <param name="Source"></param>
        /// <param name="Option"></param>
        /// <param name="Temp"></param>
        /// <param name="DueMonth"></param>
        /// <returns></returns>
        private DataRow GETPrice( DataRow Source, string Option, DateTime Temp, string DueMonth) {
            DataTable dt =this.SelectOptionHistory(DueMonth, Option, Temp.Date.ToString(Default.TimeFormat), Temp.Date.ToString(Default.TimeFormat));
            DataRow NewRow = Source.Table.NewRow();
            if (dt.Rows.Count > 0) {                
                var temp = dt.Select("Volume= MAX(Volume)");
                var Price = temp[Default.FirstItem][OptionHistory.Price].ToString();
                var Closing = Convert.ToDecimal(temp[Default.FirstItem][OptionHistory.Closing]);
                NewRow[OptionHistory.TradeDate] = Temp.Date.ToString(Default.TimeFormat);
                NewRow[OptionHistory.Option] = Option;
                NewRow[OptionHistory.DueMonth] = DueMonth;
                NewRow[OptionHistory.Price] = Price;
                NewRow[OptionHistory.Closing] = Closing;
                NewRow[WeightedHistory.OpenPrice] = Convert.ToDecimal(temp[Default.FirstItem][WeightedHistory.OpenPrice]);
                NewRow[WeightedHistory.ClosingPrice] = Convert.ToDecimal(temp[Default.FirstItem][WeightedHistory.ClosingPrice]);
                NewRow[OptionHistory.Highest] = Convert.ToDecimal(temp[Default.FirstItem][OptionHistory.Highest]);
                NewRow[OptionHistory.Opening_Price] = Convert.ToDecimal(temp[Default.FirstItem][OptionHistory.Opening_Price]);
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
        private DataRow GetResultByPrice( DataRow Source, string Option, DateTime Temp, string DueMonth, string Price) {
            DataTable dt = this.SelectOptionHistory(DueMonth, Option, Temp.Date.ToString(Default.TimeFormat), Temp.Date.ToString(Default.TimeFormat));
            DataRow NewRow = Source.Table.NewRow();
            if (dt.Rows.Count > 0) {
                if (dt.Select(string.Format("Price= {0}", Price)).Length > 0) {
                    var row = dt.Select(string.Format("Price= {0}", Price))[0];
                    NewRow[OptionHistory.TradeDate] = Temp.Date.ToString(Default.TimeFormat);
                    NewRow[OptionHistory.Option] = Option;
                    NewRow[OptionHistory.DueMonth] = DueMonth;
                    NewRow[OptionHistory.Price] = Price;
                    NewRow[OptionHistory.Closing] = row[OptionHistory.Closing];
                    NewRow[WeightedHistory.OpenPrice] = Convert.ToDecimal(row[WeightedHistory.OpenPrice]);
                    NewRow[WeightedHistory.ClosingPrice] = Convert.ToDecimal(row[WeightedHistory.ClosingPrice]);
                    NewRow[OptionHistory.Highest] = Convert.ToDecimal(row[OptionHistory.Highest]);
                    NewRow[OptionHistory.Opening_Price] = Convert.ToDecimal(row[OptionHistory.Opening_Price]);
                }
            }
            return NewRow;
        }

        /// <summary>儲存每月的選擇權歷史資料</summary>
        /// <param name="DataResource"></param>
        public void SaveOptionHistroy(string DataResource) {
            PropertyInfo[] infos = typeof(HistoryOption).GetProperties();
            DataTable dt = new DataTable();
            foreach (PropertyInfo info in infos) {
                dt.Columns.Add(info.Name);
            }
            CommTool.Files.ReadCSV(DataResource, dt);
            this.SaveOptionHistoryData(dt) ;
        }

        /// <summary>儲存買賣未平昌</summary>
        /// <param name="DataResource"></param>
        public void SaveOpen_InterestHistory(string DataResource) {
            PropertyInfo[] infos = typeof(Open_Interest).GetProperties();
            DataTable dt = new DataTable();
            foreach (PropertyInfo info in infos) {
                if (!info.Name.Equals(OpenInterest.SN))
                dt.Columns.Add(info.Name);
            }

            CommTool.Files.ReadCSV(DataResource, dt);
            SaveOpenInterestData(dt);
        }

        /// <summary>儲存買賣未平昌資料</summary>
        /// <param name="dt"></param>
        public void SaveOpenInterestData(DataTable dt) {
            if (dt != null && dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {

                    USP.AddParameter(OpenInterest.TradeDate, dr[OpenInterest.TradeDate]);
                    USP.AddParameter(OpenInterest.CallOpenInterest, dr[OpenInterest.CallOpenInterest]);
                    USP.AddParameter(OpenInterest.CallVolume, dr[OpenInterest.CallVolume]);
                    USP.AddParameter(OpenInterest.OpenInterestRatios, dr[OpenInterest.OpenInterestRatios]);
                    USP.AddParameter(OpenInterest.PutOpenInterest, dr[OpenInterest.PutOpenInterest]);
                    USP.AddParameter(OpenInterest.PutVolume, dr[OpenInterest.PutVolume]);
                    USP.AddParameter(OpenInterest.Ratios, dr[OpenInterest.Ratios]);
                    USP.ExeProcedureNotQuery(SP.SaveOpenInterest);
                }
            }
        }
        
        /// <summary>每周三取得操作指標，並且發送Maill</summary>
        /// 20170208 add by Dick 收盤後抓取每周的操作並發送Maill
        /// 20170222 modified by Dick 修正訊息資料發送錯誤
        /// 20170227 modified by Dick 修正訊息錯誤，追加買方策略停損價格
        public void GetNumberOfContractsAndMaill() {
            try {
                if (DateTime.Now.DayOfWeek.ToString() == CommTool.BaseConst.Wednesday) {
                    DateTime TimeStamp = DateTime.Now;
                    DateTime Start = new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 13, 35, 0);
                    DateTime End = new DateTime(TimeStamp.Year, TimeStamp.Month, TimeStamp.Day, 13, 45, 0);
                    TimeSpan StartTimeSpan = TimeStamp.Subtract(Start);
                    TimeSpan EndTimeSpan = TimeStamp.Subtract(End);
                    if (StartTimeSpan.TotalSeconds >= 0 && EndTimeSpan.TotalSeconds <= 0) {
                        CalendarData CalendarDB = new CalendarData();
                        Calendar _Calendar = CalendarDB.GetCalendar(DateTime.Now);
                        if (!_Calendar.IsMaill) {
                            List<string> OPList = new List<string>();
                            OPList.Add(Default.Call);
                            OPList.Add(Default.Put);
                            StringBuilder SB = new StringBuilder();
                            SB.AppendLine(string.Format("次周交易:{0}", _Calendar.NearMonth1));
                            foreach (string OP in OPList) {
                                USP.AddParameter(SPParameter.OP, OP);
                                USP.AddParameter(SPParameter.DueMonth, _Calendar.NearMonth1);
                                DataTable dt = null;
                                if (_Calendar.NearMonth1.IndexOf("W") != -1) {
                                    dt = USP.ExeProcedureGetDataTable(SP.GetMaxVolume);
                                }
                                else {
                                    dt = USP.ExeProcedureGetDataTable(SP.GetMaxNumberOfContracts);
                                }
                                if (dt != null && dt.Rows.Count > 0) {
                                    Weighted _Weighted = this.GetWeighted();
                                    decimal StopPrice = 0;
                                    if (_Weighted != null) {
                                        StopPrice = this.CalculateStopPrice(Convert.ToDecimal(dt.Rows[0][0]), dt.Rows[0][1].ToString(), _Weighted.Futures);
                                    }
                                    WeekPoint _WeekPoint = new WeekPoint();
                                    _WeekPoint.OP = OP;
                                    _WeekPoint.TradeDate = DateTime.Now;
                                    _WeekPoint.Price = dt.Rows[0][0].ToString();
                                    _WeekPoint.Contract = dt.Rows[0][1].ToString();
                                    _WeekPoint.Volume = dt.Rows[0][3].ToString();                                    
                                    _WeekPoint.StopPirce = StopPrice.ToString();
                                    _WeekPoint.BuyStopPrice = CalculateBuyStopPrice(StopPrice).ToString();
                                    _WeekPoint.DueMonth = dt.Rows[0][5].ToString();
                                    AddWeekPoint(_WeekPoint);
                                    SB.AppendLine(string.Format("方向:{0} ,   價格:{1}  ,   契約:{2}  ,   交易量:{3} ,   建議停損價格:{4}  , 買方停損價:{5}  ", OP, _WeekPoint.Price, _WeekPoint.Contract, _WeekPoint.Volume, StopPrice, _WeekPoint.BuyStopPrice));
                                }
                            }
                            CommTool.MailData MailDB = new CommTool.MailData();
                            DataTable MaillDataTable = MailDB.GetSendMail();
                            if (MaillDataTable != null && MaillDataTable.Rows.Count > 0) {
                                foreach (DataRow dr in MaillDataTable.Rows) {
                                    MailDB.RegistrySend(dr[1].ToString(), "每周操作指標", SB.ToString());
                                }
                            }
                        }
                        _Calendar.IsMaill = true;
                        CalendarDB.UpdateCalendar(_Calendar);
                    }
                }
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
            }
        }

        #endregion
    }
}

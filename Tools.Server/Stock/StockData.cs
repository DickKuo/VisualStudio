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
            public const string SqlConnectionString = "";
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
        }

        private class SP {
            public const string SaveOption = "SaveOption";
            public const string SaveOptionHistory = "SaveOptionHistory";
            public const string SaveWeighted = "SaveWeighted";
            public const string GetDueMonth = "GetDueMonth";
            public const string GetOptionHistory = "GetOptionHistory";
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
            public const string Remark="Remark";
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
        
        public StockData(string pStockNum) {
            _stock = new Stock();
            StockNum = pStockNum;
            _stock.StockNum = this._stockNum;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic[Default.sqlconnection] = Default.SqlConnectionString;
            SQLHelper.SHelper.InitSHelper(dic);
        }

        public StockData() {
            USP.ConnectiinString = Default.SqlConnectionString;
        }

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


        /// <summary>取得選擇權價格清單，預設編碼</summary>
        /// <param name="Url">POST位置</param>
        /// <returns></returns>
        public List<Option> GetOptionDaily(string Url) {
            return GetOptionDaily(Url, Encoding.Default);
        }

        /// <summary>取得選擇權價格清單</summary>
        /// 20161110 add by Dick
        /// <param name="Url">POST位置</param>
        /// <param name="UrlEncoding">編碼</param>
        public List<Option> GetOptionDaily(string Url, Encoding UrlEncoding) {
            WebInfo.WebInfo Info = new WebInfo.WebInfo();
            List<Option> list = new List<Option>();
            StreamReader SR = Info.GetResponse(Url, UrlEncoding);
            HtmlAgilityPack.HtmlDocument _HtmlDocument = new HtmlAgilityPack.HtmlDocument();
            _HtmlDocument.LoadHtml(SR.ReadToEnd());
            SR.Close();
            HtmlAgilityPack.HtmlNodeCollection anchors = _HtmlDocument.DocumentNode.SelectNodes(Default.TableTag);
            if (anchors != null) {
                if (anchors.Count > 3) {
                    HtmlAgilityPack.HtmlNodeCollection Nodes = anchors[3].SelectNodes(Default.HtmlTr);
                    string Mom = string.Empty;
                    if (Url.IndexOf(Default.MomStart) != -1) {
                        Mom = Url.Substring(Url.IndexOf(Default.MomStart) + 5, 6);
                    }
                    for (int i = 29; i <= 45; i++) {
                        Option Call = new Option();
                        Call.OP         = Default.Call;
                        Call.buy        = Nodes[i].ChildNodes[1].InnerText.Trim()   == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[1].InnerText);
                        Call.sell       = Nodes[i].ChildNodes[3].InnerText.Trim()   == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[3].InnerText);
                        Call.clinch     = Nodes[i].ChildNodes[5].InnerText.Trim()   == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[5].InnerText);
                        Call.Change     = Nodes[i].ChildNodes[7].InnerText.Trim()   == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[7].InnerText);
                        Call.Open       = Nodes[i].ChildNodes[9].InnerText.Trim()   == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[9].InnerText);
                        Call.Total      = Nodes[i].ChildNodes[11].InnerText.Trim()  == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[11].InnerText);
                        Call.Time       = Nodes[i].ChildNodes[13].InnerText.Trim()  == Default.NullSing ? DateTime.Now : Convert.ToDateTime(Nodes[i].ChildNodes[13].InnerText);
                        Call.Contract   = Nodes[i].ChildNodes[15].InnerText.Trim();
                        Call.Mom = Mom;
                        list.Add(Call);
                        Option Put = new Option();
                        Put.OP          = Default.Put;
                        Put.Contract    = Nodes[i].ChildNodes[15].InnerText.Trim();
                        Put.buy         = Nodes[i].ChildNodes[17].InnerText.Trim()  == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[17].InnerText);
                        Put.sell        = Nodes[i].ChildNodes[19].InnerText.Trim()  == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[19].InnerText);
                        Put.clinch      = Nodes[i].ChildNodes[21].InnerText.Trim()  == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[21].InnerText);
                        Put.Change      = Nodes[i].ChildNodes[23].InnerText.Trim()  == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[23].InnerText);
                        Put.Open        = Nodes[i].ChildNodes[25].InnerText.Trim()  == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[25].InnerText);
                        Put.Total       = Nodes[i].ChildNodes[27].InnerText.Trim()  == Default.NullSing ? 0 : Convert.ToDouble(Nodes[i].ChildNodes[27].InnerText);
                        Put.Time        = Nodes[i].ChildNodes[29].InnerText.Trim()  == Default.NullSing ? DateTime.Now : Convert.ToDateTime(Nodes[i].ChildNodes[29].InnerText);
                        Put.Mom = Mom;
                        list.Add(Put);
                    }
                }
            }
            _HtmlDocument = null;
            return list;
        }

        /// <summary>儲存選擇權交易歷史資訊</summary>
        /// <param name="Options"></param>
        public void SaveOpionData(List<Option> Options) {         
            try {               
                foreach (Option op in Options) {
                    USP.AddParameter(SPParameter.OP, op.OP);
                    USP.AddParameter(SPParameter.Buy, op.buy);
                    USP.AddParameter(SPParameter.Change, op.Change);
                    USP.AddParameter(SPParameter.Clinch, op.clinch);
                    USP.AddParameter(SPParameter.Contract, op.Contract);
                    USP.AddParameter(SPParameter.Open, op.Open);
                    USP.AddParameter(SPParameter.Sell, op.sell);
                    USP.AddParameter(SPParameter.Time, op.Time);
                    USP.AddParameter(SPParameter.Total, op.Total);
                    USP.AddParameter(SPParameter.Mom, op.Mom);
                    USP.ExeProcedureNotQuery(SP.SaveOption);
                }
            }
            catch (Exception ex) {
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
            }
        }


        /// <summary>儲存大盤歷史資料</summary>
        /// <param name="dt"></param>
        public void SaveWeighted(DataTable dt) {
            try {
                foreach (DataRow dr in dt.Rows) {
                    USP.AddParameter(WeightedHistory.TradeDate, Convert.ToDateTime(dr[WeightedHistory.TradeDate]));
                    USP.AddParameter(WeightedHistory.ClosingPrice, dr[WeightedHistory.ClosingPrice]);
                    USP.AddParameter(WeightedHistory.HighestPrice, dr[WeightedHistory.HighestPrice]);
                    USP.AddParameter(WeightedHistory.LowestPrice, dr[WeightedHistory.LowestPrice]);
                    USP.AddParameter(WeightedHistory.OpenPrice, dr[WeightedHistory.OpenPrice]);
                    USP.AddParameter(WeightedHistory.Remark, dr[WeightedHistory.Remark]);
                    USP.ExeProcedureNotQuery(SP.SaveWeighted);
                }
            }
            catch (Exception ex) {
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
                var Closing = Convert.ToDecimal(temp[0][OptionHistory.Closing]);
                NewRow[OptionHistory.TradeDate] = Temp.Date.ToString(Default.TimeFormat);
                NewRow[OptionHistory.Option] = Option;
                NewRow[OptionHistory.DueMonth] = DueMonth;
                NewRow[OptionHistory.Price] = Price;
                NewRow[OptionHistory.Closing] = Closing;
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
                    NewRow[OptionHistory.Closing] = row.ItemArray[9];
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

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
namespace Stock {
    public class StockData {
        private class Default {
            public const string sqlconnection = "sqlconnection";
            public const string SqlConnectionString = "Data Source=.;Initial Catalog=Stock;User Id=sa;Password=dsc;";
            public const int Second = 1000;
            public const string Html = ".html";
            public const string TableTag = "//table[@class='ext-big-tbl']";
            public const string HtmlTr = "//tr";
            public const string Call = "Call";
            public const string Put = "Put";
            public const string MomStart = "opym";
            public const string NullSing = "--";
        }

        private class SP {
            public const string SaveOption = "SaveOption";
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

        public StockData(string pStockNum) {
            _stock = new Stock();
            StockNum = pStockNum;
            _stock.StockNum = this._stockNum;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic[Default.sqlconnection] = Default.SqlConnectionString;
            SQLHelper.SHelper.InitSHelper(dic);
        }

        public StockData() { }

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
                #region  20150324 modifed 修改維新的方式撰寫SQL
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

        /// <summary></summary>
        /// <param name="Options"></param>
        public void SaveOpionData(List<Option> Options) {
            SQLHelper.UseStoreProcedure USP = new SQLHelper.UseStoreProcedure();
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

        #endregion
    }
}

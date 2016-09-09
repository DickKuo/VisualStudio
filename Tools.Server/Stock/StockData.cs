using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
namespace Stock {
    public class StockData {
        private class Default {
            public const string sqlconnection = "sqlconnection";
            public const string SqlConnectionString = "Data Source=.;Initial Catalog=Stock;User Id=sa;Password=dsc;";
            public const int Second = 1000;
            public const string Html = ".html";            
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
                string temp = ResponseFromService.Substring(start, diff).Replace("<td align=\"center\" bgcolor=\"#FFFfff\" nowrap>", string.Empty).Replace("</td>", string.Empty).Replace("<b>", string.Empty).Replace("</b>", string.Empty).Replace("<font color=#009900>", string.Empty).Replace("<font color=#ff0000>",string.Empty).Replace("<font color=#000000>",string.Empty);
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
                #region  20150324 modifed 修改維新的方式撰寫SQL 
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
        #endregion
    }
}

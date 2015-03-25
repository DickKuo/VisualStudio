using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

namespace Stock
{
    public class Stock
    {
        #region 私有變數

        private string _stockNum;

        private DateTime _stockTime;

        private decimal _price;

        private decimal _buyPrice;

        private decimal _sellPrice;

        private string _change;

        private decimal _quantity;

        private decimal _yesterday;

        private decimal _start;

        private decimal _highest;

        private decimal _lowest;

        private bool _IsSucess;

        #endregion

        /// <summary>
        /// 是否成功擷取資料
        /// </summary>       
        [Description("Howest")]
        public System.Boolean IsSucess
        {
            get
            {
                return this._IsSucess;
            }
            set
            {
                if ((_IsSucess != value))
                {
                    this._IsSucess = value;
                }
            }
        }

        /// <summary>
        /// 最低
        /// </summary>       
        [Description("Howest")]
        public System.Decimal Lowest
        {
            get
            {
                return this._lowest;
            }
            set
            {
                if ((_lowest != value))
                {
                    this._lowest = value;
                }
            }
        }

        /// <summary>
        /// 最高
        /// </summary>       
        [Description("Highest")]
        public System.Decimal Highest
        {
            get
            {
                return this._highest;
            }
            set
            {
                if ((_highest != value))
                {
                    this._highest = value;
                }
            }
        }


        /// <summary>
        /// 開盤
        /// </summary>       
        [Description("Start")]
        public System.Decimal Start
        {
            get
            {
                return this._start;
            }
            set
            {
                if ((_start != value))
                {
                    this._start = value;
                }
            }
        }


        /// <summary>
        /// 昨收
        /// </summary>       
        [Description("Yesterday")]
        public System.Decimal Yesterday
        {
            get
            {
                return this._yesterday;
            }
            set
            {
                if ((_yesterday != value))
                {
                    this._yesterday = value;
                }
            }
        }


        /// <summary>
        /// 張數
        /// </summary>       
        [Description("Quantity")]
        public System.Decimal Quantity
        {
            get
            {
                return this._quantity;
            }
            set
            {
                if ((_quantity != value))
                {
                    this._quantity = value;
                }
            }
        }


        /// <summary>
        /// 漲跌
        /// </summary>       
        [Description("Change")]
        public System.String Change
        {
            get
            {
                return this._change;
            }
            set
            {
                if ((_change != value))
                {
                    this._change = value;
                }
            }
        }


        /// <summary>
        /// 賣價
        /// </summary>       
        [Description("SellPrice")]
        public System.Decimal SellPrice
        {
            get
            {
                return this._sellPrice;
            }
            set
            {
                if ((_sellPrice != value))
                {
                    this._sellPrice = value;
                }
            }
        }


        /// <summary>
        /// 買價
        /// </summary>       
        [Description("BuyPrice")]
        public System.Decimal BuyPrice
        {
            get
            {
                return this._buyPrice;
            }
            set
            {
                if ((_buyPrice != value))
                {
                    this._buyPrice = value;
                }
            }
        }

        /// <summary>
        /// 成交價
        /// </summary>       
        [Description("Price")]
        public System.Decimal Price
        {
            get
            {
                return this._price;
            }
            set
            {
                if ((_price != value))
                {
                    this._price = value;
                }
            }
        }


        /// <summary>
        /// 股票代號
        /// </summary>       
        [Description("StockNum")]
        public System.String StockNum
        {
            get
            {
                return this._stockNum;
            }
            set
            {
                if ((_stockNum != value))
                {
                    this._stockNum = value;
                }
            }
        }


        /// <summary>
        /// 時間
        /// </summary>       
        [Description("StockNum")]
        public System.DateTime StockTime
        {
            get
            {
                return this._stockTime;
            }
            set
            {
                if ((_stockTime != value))
                {
                    this._stockTime = value;
                }
            }
        }

    }


    public class StockData
    {
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

        public StockData(string pStockNum)
        {
            _stock = new Stock();
            StockNum = pStockNum;
            _stock.StockNum = this._stockNum;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["sqlconnection"] = "Data Source=.;Initial Catalog=Stock;User Id=sa;Password=dsc;";
            SQLHelper.SHelper.InitSHelper(dic);
        }        
        public StockData(){}

        #region public mothed

        public Stock GetStockData(string Url,string ConnetionString)
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
                string temp = ResponseFromService.Substring(start, diff).Replace("<td align=\"center\" bgcolor=\"#FFFfff\" nowrap>", "").Replace("</td>", "").Replace("<b>", "").Replace("</b>", "").Replace("<font color=#009900>", "").Replace("<font color=#ff0000>", "").Replace("<font color=#000000>", "");
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
                #region  20150324 modifed 修改維新的方式撰寫SQL                
                string sql = string.Format(@"insert into ErrorLog values({0},{1},{2},{3})", "001", ex.Message, DateTime.Now, this._stock.StockNum);
                //SQL.SQL sql = new SQL.SQL(ConnetionString);
                //sql.CommandString = "insert into ErrorLog values(@ErrorType,@ErrorMessage,@ErrorTime,@ErrorStockNum)";
                //sql.AddParameter("@ErrorType", "001");
                //sql.AddParameter("@ErrorMessage", ex.Message);
                //sql.AddParameter("@ErrorTime", DateTime.Now);
                //sql.AddParameter("@ErrorStockNum", this._stock.StockNum);
                //sql.ExeNotRetun();
                SQLHelper.SHelper.ExeNoQuery(sql);
                #endregion               
            }
            return this._stock;
        }

        public void SetkData(string ConnetionString)
        {
            if (this._stock.IsSucess)
            {
                #region  20150324 modifed 修改維新的方式撰寫SQL
                //SQL.SQL sql = new SQL.SQL(ConnetionString);
                //sql.CommandString = "Insert into StockData values(@StockNum, GetDate(),@Price,@BuyPrice, @SellPrice, @Change, @Quantity, @YesterDay, @Start,@Highest,@Lowest)";
                //sql.AddParameter("@StockNum", this._stock.StockNum);
                ////sql.AddParameter("@StockTime", this._stock.StockTime);
                //sql.AddParameter("@Price", this._stock.Price);
                //sql.AddParameter("@BuyPrice", this._stock.BuyPrice);
                //sql.AddParameter("@SellPrice", this._stock.SellPrice);
                //sql.AddParameter("@Change", this._stock.Change);
                //sql.AddParameter("@Quantity", this._stock.Quantity);
                //sql.AddParameter("@YesterDay", this._stock.Yesterday);
                //sql.AddParameter("@Start", this._stock.Start);
                //sql.AddParameter("@Highest", this._stock.Highest);
                //sql.AddParameter("@Lowest", this._stock.Lowest);
                //sql.ExeNotRetun();
                string sql = string.Format(@"Insert into StockData values({0}, {1},{2},{3}, {4},{5}, {6}, {7}, {8},{9},{10})", this._stock.StockNum,
                    this._stock.StockTime, this._stock.Price, this._stock.BuyPrice, this._stock.SellPrice, this._stock.Change, this._stock.Quantity,
                   this._stock.Yesterday, this._stock.Start, this._stock.Highest, this._stock.Lowest);
                SQLHelper.SHelper.ExeNoQuery(sql);
                #endregion
            }
        }

        /// <summary>
        /// 更新股票清單
        /// EPS 也同步更新
        /// </summary>
        public void RefreshList()
        {      
            for (int i = 1100; i < 9999; i++)
            {
                pRefreshList(i.ToString());
                if (i % 5 == 0)
                {
                    System.Threading.Thread.Sleep(1000);
                   
                }              
            }
        }

        private string RefreshStockList()
        {
            try
            {
                WebRequest myWebRequest = WebRequest.Create(_url + _stockNum+".html");
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)myWebRequest.GetResponse();
                Stream DataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(DataStream, Encoding.Default);
                string ResponseFromService = reader.ReadToEnd();
                int start = ResponseFromService.IndexOf("description");
                int end = ResponseFromService.IndexOf("公司資料，查詢 ");
                int deffence = end - start;              
                StringBuilder sb = new StringBuilder();
                sb.Append(ResponseFromService.Substring(start, deffence).Replace("description\" content=\"", "").Replace("(" + _stockNum + ")", ""));
                sb.Append(",");
                string[] sArray = Regex.Split(ResponseFromService, "table", RegexOptions.IgnoreCase);
                string[] arry = Regex.Split(sArray[23], "tr", RegexOptions.IgnoreCase);              
                for (int i = 3; i < arry.Length; i++)
                {
                    string[] parry = Regex.Split(arry[i], "td", RegexOptions.IgnoreCase);
                    if (parry.Length > 8)
                    {

                        sb.Append(parry[5].Replace("bgcolor=\"#FFFAE8\">", "").Replace("</", "").Replace("height=\"25\"", "").Trim());

                        sb.Append(parry[7].Replace("align=\"center\">", "").Replace("</", ""));

                        sb.Append(",");
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                CommTool.ToolLog log = new  CommTool.ToolLog();
                CommTool.ToolLog.Log(CommTool.LogType.Error, _stockNum.ToString());
                return "error";
            }
        }
        private void pRefreshList(string pCode)
        {
            this.StockNum = pCode;
            this.URL = "http://tw.stock.yahoo.com/d/s/company_";
            string temp = this.RefreshStockList();
            if (temp != "error")
            {
                string[]  arry =temp.Split(',');
                #region  20150324 modifed by Dick for 修改維新的方式撰寫SQL
                #region old
                //SQL.SQL sql = new SQL.SQL("");
                //sql.CommandString = "select count(*) from StockList where StockNum=@StockNum and EPS1=@EPS1 and EPS2=@EPS2 and EPS3 =@EPS3 and EPS4 =@EPS4 ";
                //sql.AddParameter("@StockNum", pCode);
                //sql.AddParameter("@EPS1", arry[1]);
                //sql.AddParameter("@EPS2", arry[2]);
                //sql.AddParameter("@EPS3", arry[3]);
                //sql.AddParameter("@EPS4", arry[4]);
                //if (Convert.ToInt32(sql.ExeRetrunScalary()) == 0)
                //{
                //    sql.CommandString = "Insert into  StockList values(@StockNum,@StockName,@EPS1,@EPS2,@EPS3,@EPS4,@Date) ";
                //    sql.AddParameter("@StockNum", pCode);
                //    sql.AddParameter("@StockName", arry[0]);
                //    sql.AddParameter("@EPS1", arry[1]);
                //    sql.AddParameter("@EPS2", arry[2]);
                //    sql.AddParameter("@EPS3", arry[3]);
                //    sql.AddParameter("@EPS4", arry[4]);
                //    sql.AddParameter("@Date", DateTime.Now.Date);
                //    sql.ExeNotRetun();
                //}              
                #endregion
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

        /// <summary>
        /// 追蹤EPS 連4季 都是上升的股票
        /// 最佳進步獎
        /// </summary>
        /// <returns></returns>
        public string  TraceEPS()
        {
            #region  20150324 modifed by Dick for 修改維新的方式撰寫SQL
            
            #region Old          
            //SQL.SQL sql = new SQL.SQL("Data Source=.;Initial Catalog=Stock;User Id=sa;Password=dsc;");
            //sql.CommandString = "Select StockNum,StockName,EPS1,EPS2,EPS3,EPS4 from StockList Group by StockNum,StockName,EPS1,EPS2,EPS3,EPS4";
            //DataTable dt = sql.ExeRetrunDataTable();
            #endregion
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
            else {
                return "0";
            }
        }

        #endregion

    }
}

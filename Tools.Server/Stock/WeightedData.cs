using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Stock {
    public class WeightedData: BaseData {
        private class SP {
            public const string GetWeighted = "GetWeighted";
            public const string GetWeightedByDay = "GetWeightedByDay";
            public const string SaveWeighted = "SaveWeighted";
        }

        private class SPParameter {
            public const string BeginDate = "BeginDate";
            public const string EndDate = "EndDate";
            public const string TradeDate = "TradeDate";
            public const string ClosingPrice = "ClosingPrice";
            public const string HighestPrice = "HighestPrice";
            public const string LowestPrice = "LowestPrice";
            public const string OpenPrice = "OpenPrice";
            public const string Price = "Price";
            public const string Futures = "Futures";
            public const string Change = "Change";
            public const string Volume = "Volume";
            public const string Remark = "Remark";
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

        /// <summary>取的時間區間內的大盤資料</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<Weighted> GetWeightedByDay(DateTime BeginDate,DateTime EndDate) {
            List<Weighted> WeightedList = new List<Weighted>();
            USP.AddParameter(SPParameter.BeginDate,BeginDate);
            USP.AddParameter(SPParameter.EndDate, EndDate);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetWeighted);
            if (dt != null && dt.Rows.Count > 0) {
                foreach (DataRow Row in dt.Rows) {
                    Weighted _Weighted = new Weighted();
                    PropertyInfo[] infos = typeof(Weighted).GetProperties();
                    foreach (PropertyInfo info in infos) {
                        _Weighted.GetType().GetProperty(info.Name).SetValue(_Weighted, Row[info.Name], null);
                    }
                    WeightedList.Add(_Weighted);
                }
            }
            return WeightedList;
        }

        /// <summary>取得大盤價格走勢</summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public Weighted GetWeightedDaily(string Url) {
            WebInfo.WebInfo Info = new WebInfo.WebInfo();
            HtmlAgilityPack.HtmlDocument Doc = Info.GetWebHtmlDocument(Url, Encoding.UTF8);
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

        /// <summary>儲存大盤歷史資料</summary>
        public void SaveWeighted(Weighted _Weighted) {
            try {
                USP.AddParameter(SPParameter.TradeDate, _Weighted.TradeDate);
                USP.AddParameter(SPParameter.ClosingPrice, _Weighted.ClosingPrice);
                USP.AddParameter(SPParameter.HighestPrice, _Weighted.HighestPrice);
                USP.AddParameter(SPParameter.LowestPrice, _Weighted.LowestPrice);
                USP.AddParameter(SPParameter.OpenPrice, _Weighted.OpenPrice);
                USP.AddParameter(SPParameter.Price, _Weighted.Price);
                USP.AddParameter(SPParameter.Futures, _Weighted.Futures);
                USP.AddParameter(SPParameter.Change, _Weighted.Change);
                USP.AddParameter(SPParameter.Volume, _Weighted.Volume);
                USP.AddParameter(SPParameter.Remark, _Weighted.Remark == null ? string.Empty : _Weighted.Remark);
                USP.ExeProcedureNotQuery(SP.SaveWeighted);
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
                    USP.AddParameter(SPParameter.TradeDate, Convert.ToDateTime(dr[SPParameter.TradeDate]));
                    USP.AddParameter(SPParameter.ClosingPrice, dr[SPParameter.ClosingPrice]);
                    USP.AddParameter(SPParameter.HighestPrice, dr[SPParameter.HighestPrice]);
                    USP.AddParameter(SPParameter.LowestPrice, dr[SPParameter.LowestPrice]);
                    USP.AddParameter(SPParameter.OpenPrice, dr[SPParameter.OpenPrice]);
                    USP.AddParameter(SPParameter.Price, dr[SPParameter.Price]);
                    USP.AddParameter(SPParameter.Futures, dr[SPParameter.Futures]);
                    USP.AddParameter(SPParameter.Remark, dr[SPParameter.Remark]);
                    USP.ExeProcedureNotQuery(SP.SaveWeighted);
                }
            }
            catch (Exception ex) {
                CommTool.ToolLog.Log(ex);
            }
        }
    }
}

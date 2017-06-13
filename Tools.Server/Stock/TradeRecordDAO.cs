﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Stock {
    public class TradeRecordDAO : BaseData {

        private class Default {
            public const string DueMonthFormat =  "{0}{1}";
            public const string MonthFormat = "00";
        }

        private class SP {
            public const string AddTradeRecord = "AddTradeRecord";
            public const string UpdateTradeRecord = "UpdateTradeRecord";
            public const string GetTradeRecord = "GetTradeRecord";
            public const string GetDueDateSettlement="GetDueDateSettlement";
            public const string GetTradeRecordByDueDay = "GetTradeRecordByDueDay";
        }

        private class SPParameter {
            public const string SN = "SN";
            public const string TradeDate = "TradeDate";
            public const string DueMonth = "DueMonth";
            public const string OP = "OP";
            public const string Contract = "Contract";
            public const string Type = "Type";
            public const string Lot = "Lot";
            public const string Price = "Price";
            public const string IsPyeongchang = "IsPyeongchang";
            public const string IsMail = "IsMail";
            public const string StopPrice = "StopPrice";
            public const string Settlement = "Settlement";
            public const string Level = "Level";
            public const string PyeongchangTime = "PyeongchangTime";
        }

        /// <summary>新增操作紀錄</summary>
        /// <param name="_TradeRecord"></param>
        /// <returns></returns>
        public int AddTradeRecord(TradeRecord _TradeRecord) {
            USP.AddParameter(SPParameter.TradeDate, _TradeRecord.TradeDate);
            USP.AddParameter(SPParameter.DueMonth, _TradeRecord.DueMonth.ToUpper());
            USP.AddParameter(SPParameter.OP, _TradeRecord.OP);
            USP.AddParameter(SPParameter.Contract, _TradeRecord.Contract);
            USP.AddParameter(SPParameter.Type, _TradeRecord.Type);
            USP.AddParameter(SPParameter.Lot, _TradeRecord.Lot);
            USP.AddParameter(SPParameter.Price, _TradeRecord.Price); 
            TradeRecord Result= USP.ExeProcedureGetObject(SP.AddTradeRecord,new TradeRecord());
            return Result.SN;
        }

        /// <summary>更新操作紀錄</summary>
        /// <param name="_TradeRecord"></param>
        public void UpdateTradeRecord(TradeRecord _TradeRecord) {
            USP.AddParameter(SPParameter.SN, _TradeRecord.SN);
            USP.AddParameter(SPParameter.DueMonth, _TradeRecord.DueMonth.ToUpper());
            USP.AddParameter(SPParameter.OP, _TradeRecord.OP);
            USP.AddParameter(SPParameter.Contract, _TradeRecord.Contract);
            USP.AddParameter(SPParameter.Type, _TradeRecord.Type);
            USP.AddParameter(SPParameter.Lot, _TradeRecord.Lot);
            USP.AddParameter(SPParameter.Price, _TradeRecord.Price);
            USP.AddParameter(SPParameter.IsPyeongchang, _TradeRecord.IsPyeongchang);
            USP.AddParameter(SPParameter.IsMail, _TradeRecord.IsMail);
            USP.AddParameter(SPParameter.StopPrice, _TradeRecord.StopPrice);
            USP.AddParameter(SPParameter.Settlement, _TradeRecord.Settlement);
            USP.AddParameter(SPParameter.Level, _TradeRecord.Level);
            USP.AddParameter(SPParameter.PyeongchangTime, _TradeRecord.PyeongchangTime);
            USP.ExeProcedureGetDataTable(SP.UpdateTradeRecord);
        }
        
        /// <summary>取得未平昌的交易紀錄</summary>
        /// <returns></returns>
        public List<TradeRecord> GetTradeRecord() {
            List<TradeRecord> TradeList = new List<TradeRecord>();
            TradeList = USP.ExeProcedureGetObjectList(SP.GetTradeRecord,new TradeRecord()); 
            return TradeList;
        }

        /// <summary>取得時間區間內的結算金額</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public decimal GetDueDateSettlement(string BeginDate,string EndDate) {
            USP.AddParameter(BaseData.BaseSParameter.BeginDate,BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetDueDateSettlement);
            if (dt != null && dt.Rows.Count > 0) {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else {
                return 0;
            }
        }

        /// <summary>取得時間區間的操作紀錄</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataTable GetTradeRecordByDueDay(string BeginDate, string EndDate) {
            USP.AddParameter(BaseData.BaseSParameter.BeginDate, BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetTradeRecordByDueDay);
            return dt;
        }
        
        /// <summary>計算時間區間的獎勵</summary>
        /// <returns></returns>
        public decimal CalculateReward(decimal RecordSettlement) {            
            int BasePrice = 50;
            if (RecordSettlement >= 500 && RecordSettlement <= 999) {
                return BasePrice * RecordSettlement * 0.06m;
            }
            else if (RecordSettlement >= 1000 && RecordSettlement <= 1999) {
                return BasePrice * RecordSettlement * 0.09m;
            }
            else if (RecordSettlement >= 2000 && RecordSettlement <= 3999) {
                return BasePrice * RecordSettlement * 0.12m;
            }
            else if (RecordSettlement >= 4000 && RecordSettlement <= 6999) {
                return BasePrice * RecordSettlement * 0.15m;
            }
            else if (RecordSettlement >= 7000 && RecordSettlement <= 9999) {
                return BasePrice * RecordSettlement * 0.18m;
            }
            else if (RecordSettlement >= 10000) {
                return BasePrice * RecordSettlement * 0.21m;
            }
            else {
                return 0;
            }
        }

        /// <summary>寄送每月結算結果報表</summary>
        public void CalculateResultReport() { 
            CalendarDAO CalDAO = new CalendarDAO();
            string Begin = string.Format(Default.DueMonthFormat, DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month.ToString(Default.MonthFormat));
            string End = string.Format(Default.DueMonthFormat, DateTime.Now.Year, DateTime.Now.Month.ToString(Default.MonthFormat));
            Calendar _Calendar = CalDAO.GetDueMonthWeekLasDay(End);
            if (_Calendar.Daily.Day == DateTime.Now.Day) {
                if (!_Calendar.IsSettlement) {
                    Calendar Last_Calendar = CalDAO.GetDueMonthWeekLasDay(Begin);
                    string ReportText = RecordTable(Last_Calendar.Daily.ToString(BaseData.BaseSParameter.DataTimeFormat), _Calendar.Daily.ToString(BaseData.BaseSParameter.DataTimeFormat));
                    CommTool.MailData MailDB = new CommTool.MailData();
                    DataTable MaillDataTable = MailDB.GetSendMail();
                    if (MaillDataTable != null && MaillDataTable.Rows.Count > 0) {
                        foreach (DataRow dr in MaillDataTable.Rows) {
                            MailDB.RegistrySend(dr[1].ToString(), "本月操作報表", ReportText);
                        }
                    }
                    _Calendar.IsSettlement = true;
                    CalDAO.UpdateCalendar(_Calendar);
                }
            }           
        }

        private string RecordTable(string BeginDate, string EndDate) {
            decimal RecordSettlement = GetDueDateSettlement(BeginDate,EndDate);
            decimal Result = this.CalculateReward(RecordSettlement);
            DataTable RecordTable = GetTradeRecordByDueDay(BeginDate, EndDate);
            StringBuilder Html = new StringBuilder();
            Html.AppendLine( "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            Html.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml'>");
            Html.AppendLine("<head>");
            Html.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0' />");
            Html.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");
            Html.AppendLine("<title>Dick Stock Report</title>");
            Html.AppendLine("</head>");
            Html.AppendLine("<body style='font-family:'Open Sans', '微軟正黑體';margin:0;padding:0;'>");
            
            Html.AppendLine("<table  style='border:3px #FFAC55 double;padding:5px;' rules='all' cellpadding='5';>");
            Html.AppendLine("<tr>");
            Html.AppendLine("<th>RowID</th>");
            Html.AppendLine("<th>TradeDate</th>");
            Html.AppendLine("<th>DueMonth</th>");
            Html.AppendLine("<th>OP</th>");
            Html.AppendLine("<th>Contract</th>");
            Html.AppendLine("<th>Type</th>");
            Html.AppendLine("<th>Lot</th>");
            Html.AppendLine("<th>Price</th>");
            Html.AppendLine("<th>StopPrice</th>");
            Html.AppendLine("<th>Spreads</th>");            
            Html.AppendLine("<th>PyeongchangTime</th>");
            Html.AppendLine("</tr>");
            if (RecordTable != null && RecordTable.Rows.Count > 0) {
                foreach (DataRow dr in RecordTable.Rows)
                {
                    Html.AppendLine("<tr>");
                    decimal Spreads = 0;
                    if (dr[5].ToString() == "Buy") {
                        Spreads = Convert.ToDecimal(dr[8]) - Convert.ToDecimal(dr[7]);
                    }
                    else {
                        Spreads = Convert.ToDecimal(dr[7]) - Convert.ToDecimal(dr[8]);
                    }
                    Spreads = Spreads * Convert.ToInt32(dr[6]);
                    Html.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td>",
                        dr[0], Convert.ToDateTime(dr[1]).ToString(BaseSParameter.DateTimeFormat2), dr[2], dr[3], dr[4], dr[5], dr[6], dr[7], dr[8], Spreads , Convert.ToDateTime(dr[9]).ToString(BaseSParameter.DateTimeFormat2)
                        );
                    Html.AppendLine("</tr>");
                } 
            }
            Html.AppendLine("<tr>");
            Html.Append("<td colspan='2'> 結算點數 </td>");
            Html.AppendFormat("<td colspan='3'>NP {0}</td>", RecordSettlement);
            Html.Append("<td colspan='3'> 結算獎金 </td>");
            Html.AppendFormat("<td colspan='3'>$ {0}</td>", Result);
            Html.AppendLine("</tr>");
            Html.AppendLine("</table> </body></html>");      
            return Html.ToString();
        }
    }
}

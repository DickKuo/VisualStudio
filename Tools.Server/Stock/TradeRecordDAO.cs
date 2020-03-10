using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ObjectBase;
using System;

namespace Stock
{
    public class TradeRecordDAO : BaseData
    {
        private class Default
        {
            public const string DueMonthFormat = "{0}{1}";
            public const string MonthFormat = "00";
        }

        private class SP
        {
            public const string AddTradeRecord = "AddTradeRecord";
            public const string UpdateTradeRecord = "UpdateTradeRecord";
            public const string GetTradeRecord = "GetTradeRecord";
            public const string GetDueDateSettlement = "GetDueDateSettlement";
            public const string GetTradeRecordByDueDay = "GetTradeRecordByDueDay";
            public const string GetTradeRecordByDueDayPage = "GetTradeRecordByDueDayPage";
            public const string GetTradeRecordPagesByDueDay = "GetTradeRecordPagesByDueDay";
            public const string PyeongchangTrade = "PyeongchangTrade";
            public const string GetDueDateSettlementByAdviserSN = "GetDueDateSettlementByAdviserSN";
            public const string GetTradeRecordBySN = "GetTradeRecordBySN";
            public const string GetDueMonthList = "GetDueMonthList";
            public const string GetTradeRecordeByDueMonth = "GetTradeRecordeByDueMonth";
        }

        private class SPParameter
        {
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
            public const string Range = "Range";
            public const string Page = "Page";
            public const string CustomerSN = "CustomerSN";
            public const string AdviserSN = "AdviserSN";
        }

        /// <summary>新增操作紀錄</summary>
        /// <param name="_TradeRecord"></param>
        /// <returns></returns>
        public int AddTradeRecord(TradeRecord _TradeRecord)
        {
            USP.AddParameter(SPParameter.DueMonth, _TradeRecord.DueMonth.ToUpper());
            USP.AddParameter(SPParameter.OP, _TradeRecord.OP);
            USP.AddParameter(SPParameter.Contract, _TradeRecord.Contract);
            USP.AddParameter(SPParameter.Type, _TradeRecord.Type);
            USP.AddParameter(SPParameter.Lot, _TradeRecord.Lot);
            USP.AddParameter(SPParameter.Price, _TradeRecord.Price);
            USP.AddParameter(SPParameter.AdviserSN, _TradeRecord.AdviserSN);
            TradeRecord Result = USP.ExeProcedureGetObject(SP.AddTradeRecord, new TradeRecord());
            return Result.SN;
        }

        /// <summary>平昌單子</summary>
        /// <param name="_TradeRecord"></param>
        /// <returns></returns>
        public int PyeongchangTrade(TradeRecord _TradeRecord)
        {
            USP.AddParameter(SPParameter.SN, _TradeRecord.SN);
            USP.AddParameter(SPParameter.Lot, _TradeRecord.Lot);
            USP.AddParameter(SPParameter.StopPrice, _TradeRecord.StopPrice);
            TradeRecord Result = USP.ExeProcedureGetObject(SP.PyeongchangTrade, new TradeRecord());
            return Result.SN;
        }

        /// <summary>更新操作紀錄</summary>
        /// <param name="_TradeRecord"></param>
        public void UpdateTradeRecord(TradeRecord _TradeRecord)
        {
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
        public List<TradeRecord> GetTradeRecord()
        {
            List<TradeRecord> TradeList = new List<TradeRecord>();
            TradeList = USP.ExeProcedureGetObjectList(SP.GetTradeRecord, new TradeRecord());
            return TradeList;
        }

        /// <summary>取得時間區間內的結算金額</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public decimal GetDueDateSettlement(string BeginDate, string EndDate)
        {
            USP.AddParameter(BaseData.BaseSParameter.BeginDate, BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetDueDateSettlement);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>取得顧問時間區間內的結算金額</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="AdviserSN"></param>
        /// <returns></returns>
        public decimal GetDueDateSettlementByAdviserSN(string BeginDate, string EndDate, int AdviserSN)
        {
            USP.AddParameter(BaseData.BaseSParameter.BeginDate, BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            USP.AddParameter(SPParameter.AdviserSN, AdviserSN);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetDueDateSettlementByAdviserSN);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>取得時間區間的操作紀錄</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public DataTable GetTradeRecordByDueDay(string BeginDate, string EndDate)
        {
            USP.AddParameter(BaseData.BaseSParameter.BeginDate, BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetTradeRecordByDueDay);
            return dt;
        }

        public List<TradeRecord> GetListTradeRecordByDueDay(string BeginDate, string EndDate)
        {
            USP.AddParameter(BaseData.BaseSParameter.BeginDate, BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            List<TradeRecord> ListTradeRecord = USP.ExeProcedureGetObjectList(SP.GetTradeRecordByDueDay, new TradeRecord());
            return ListTradeRecord;
        }

        /// <summary>取得時間區間的操作紀錄(分頁)</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Range"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public DataTable GetTradeRecordByDueDayPage(int AdviserSN, string BeginDate, string EndDate, int Range, int Page)
        {
            USP.AddParameter(BaseData.BaseSParameter.BeginDate, BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            USP.AddParameter(SPParameter.AdviserSN, AdviserSN);
            USP.AddParameter(SPParameter.Range, Range);
            USP.AddParameter(SPParameter.Page, Page);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetTradeRecordByDueDayPage);
            return dt;
        }

        /// <summary>依據SN取得交易單</summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public TradeRecord GetTradeRecordBySN(int SN)
        {
            USP.AddParameter(BaseData.BaseSParameter.SN, SN);
            TradeRecord Result = USP.ExeProcedureGetObject<TradeRecord>(SP.GetTradeRecordBySN, new TradeRecord());
            return Result;
        }

        /// <summary>取得時間區間紀錄的總筆數</summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public int GetTradeRecordPagesByDueDay(string BeginDate, string EndDate, int Range)
        {
            USP.AddParameter(BaseData.BaseSParameter.BeginDate, BeginDate);
            USP.AddParameter(BaseData.BaseSParameter.EndDate, EndDate);
            USP.AddParameter(SPParameter.Range, Range);
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetTradeRecordPagesByDueDay);
            int Result = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                Result = Convert.ToInt32(dt.Rows[0][0]);
            }
            return Result;
        }

        /// <summary>計算時間區間的獎勵</summary>
        /// <returns></returns>
        public decimal CalculateReward(decimal RecordSettlement)
        {
            int BasePrice = 50;
            if (RecordSettlement >= 500 && RecordSettlement <= 999)
            {
                return BasePrice * RecordSettlement * 0.06m;
            }
            else if (RecordSettlement >= 1000 && RecordSettlement <= 1999)
            {
                return BasePrice * RecordSettlement * 0.09m;
            }
            else if (RecordSettlement >= 2000 && RecordSettlement <= 3999)
            {
                return BasePrice * RecordSettlement * 0.12m;
            }
            else if (RecordSettlement >= 4000 && RecordSettlement <= 6999)
            {
                return BasePrice * RecordSettlement * 0.15m;
            }
            else if (RecordSettlement >= 7000 && RecordSettlement <= 9999)
            {
                return BasePrice * RecordSettlement * 0.18m;
            }
            else if (RecordSettlement >= 10000)
            {
                return BasePrice * RecordSettlement * 0.21m;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>寄送每月結算結果報表</summary>
        public void CalculateResultReport()
        {
            CalendarDAO CalDAO = new CalendarDAO();
            string Begin = string.Format(Default.DueMonthFormat, DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month.ToString(Default.MonthFormat));
            string End = string.Format(Default.DueMonthFormat, DateTime.Now.Year, DateTime.Now.Month.ToString(Default.MonthFormat));
            Calendar _Calendar = CalDAO.GetDueMonthWeekLasDay(End);
            if (_Calendar.Daily.Day == DateTime.Now.Day)
            {
                if (!_Calendar.IsSettlement)
                {
                    Calendar Last_Calendar = CalDAO.GetDueMonthWeekLasDay(Begin);
                    string BeginDate = Last_Calendar.Daily.ToString(BaseData.BaseSParameter.DataTimeFormat);
                    string EndDate = _Calendar.Daily.ToString(BaseData.BaseSParameter.DataTimeFormat);
                    CommTool.MailData MailDB = new CommTool.MailData();

                    //DataTable MaillDataTable = MailDB.GetSendMail();
                    //if (MaillDataTable != null && MaillDataTable.Rows.Count > 0) {
                    //    foreach (DataRow dr in MaillDataTable.Rows) {
                    //        MailDB.RegistrySend(dr[1].ToString(), "本月操作報表", ReportText);
                    //    }
                    //}      

                    AdviserDAO AdviserDB = new AdviserDAO();
                    List<Adviser> ListAdviser = AdviserDB.GetListAdviser();
                    foreach (var Item in ListAdviser)
                    {

                        string ReportText = RecordTable(BeginDate, EndDate, Item.SN);
                        MailDB.RegistrySend(Item.Email, "本月操作報表", ReportText);

                        System.Threading.Thread.Sleep(2000);

                        string BounsReport = BounsTable(BeginDate, EndDate, Item.SN);
                        MailDB.RegistrySend(Item.Email, "獲利報表", BounsReport);
                    }

                    _Calendar.IsSettlement = true;
                    CalDAO.UpdateCalendar(_Calendar);
                }
            }
        }

        private string RecordTable(string BeginDate, string EndDate, int AdviserSN)
        {
            decimal RecordSettlement = GetDueDateSettlementByAdviserSN(BeginDate, EndDate, AdviserSN);    //GetDueDateSettlement(BeginDate,EndDate);
            decimal Result = this.CalculateReward(RecordSettlement);
            DataTable RecordTable = GetTradeRecordByDueDay(BeginDate, EndDate);
            StringBuilder Html = new StringBuilder();
            Html.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
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
            Html.AppendLine("<th>Fee</th>");
            Html.AppendLine("<th>Subtotal</th>");
            Html.AppendLine("</tr>");
            if (RecordTable != null && RecordTable.Rows.Count > 0)
            {
                foreach (DataRow dr in RecordTable.Rows)
                {
                    Html.AppendLine("<tr>");
                    double Spreads = 0;
                    if (dr[5].ToString() == "Buy")
                    {
                        Spreads = Convert.ToDouble(dr[8]) - Convert.ToDouble(dr[7]);
                    }
                    else
                    {
                        Spreads = Convert.ToDouble(dr[7]) - Convert.ToDouble(dr[8]);
                    }
                    double Lot = Convert.ToInt32(dr[6]);
                    Spreads = Spreads * Lot;
                    Html.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td><td>{11}</td><td>{12}</td>",
                        dr[0], Convert.ToDateTime(dr[1]).ToString(BaseSParameter.DateTimeFormat2), dr[2], dr[3], dr[4], dr[5], dr[6], dr[7], dr[8], Spreads,
                          Convert.ToDateTime(dr[9]).ToString(BaseSParameter.DateTimeFormat2), Lot * 2, Spreads - (Lot * 2)
                        );
                    Html.AppendLine("</tr>");
                }
            }
            Html.AppendLine("<tr>");
            Html.Append("<td colspan='2'> 結算點數 </td>");
            Html.AppendFormat("<td colspan='3'>NP {0}</td>", RecordSettlement);
            Html.Append("<td colspan='3'> 結算獎金 </td>");
            Html.AppendFormat("<td colspan='5'>$ {0}</td>", Result);
            Html.AppendLine("</tr>");
            Html.AppendLine("</table> </body></html>");
            return Html.ToString();
        }

        private string BounsTable(string BeginDate, string EndDate, int AdviserSN)
        {
            decimal RecordSettlement = GetDueDateSettlementByAdviserSN(BeginDate, EndDate, AdviserSN);
            CustomerDAO CustomerDB = new CustomerDAO();
            List<Customer> ListCustomer = CustomerDB.GetCustomerByHelperSN(AdviserSN);
            decimal TotalChips = ListCustomer.Sum(x => x.Chips);

            StringBuilder Html = new StringBuilder();
            Html.AppendLine("<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            Html.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml'>");
            Html.AppendLine("<head>");
            Html.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0' />");
            Html.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");
            Html.AppendLine("<title>Dick Stock Report</title>");
            Html.AppendLine("</head>");
            Html.AppendLine("<body style='font-family:'Open Sans', '微軟正黑體';margin:0;padding:0;'>");
            Html.AppendLine("<table  style='border:3px #FFAC55 double;padding:5px;' rules='all' cellpadding='5';>");

            Html.AppendLine("<tr>");
            Html.AppendFormat("<td colspan='5'> 本月獲利(NP): {0} </td>", RecordSettlement.ToString("##.00"));
            //Html.AppendFormat("<td colspan='3'>NP {0}</td>", RecordSettlement.ToString("##.00"));  
            Html.AppendLine("</tr>");

            Html.AppendLine("<tr>");
            Html.AppendLine("<th>Account</th>");
            Html.AppendLine("<th>Name</th>");
            Html.AppendLine("<th>Chips</th>");
            Html.AppendLine("<th>Bouns</th>");
            //Html.AppendLine("<th>Commission</th>");
            //Html.AppendLine("<th>Result</th>");
            Html.AppendLine("</tr>");

            SettleTimeDAO SettleDB = new SettleTimeDAO();
            SettleTime SeT = SettleDB.GetNearlySettleTime();

            TranscationDAO TransDB = new TranscationDAO();


            foreach (var Item in ListCustomer)
            {
                decimal Result = RecordSettlement * (Item.Chips / TotalChips);
                //decimal Commission = 0; // Bouns > 0 ? Bouns * (Item.CommissionRate / 100) : 0;                
                if (Result == 0)
                {
                    continue;
                }
                //TotalCommission += Commission;
                Html.AppendLine("<tr>");
                Html.AppendLine("<td>" + Item.Account + "</td>");
                Html.AppendLine("<td>" + Item.Member.NickName + "</td>");
                Html.AppendLine("<td>" + Item.Chips + "</td>");
                //Html.AppendLine("<td>" + Bouns.ToString("##.00") + "</td>");
                //Html.AppendLine("<td>" + Commission.ToString("##.00") + "</td>");
                Html.AppendLine("<th>" + Result.ToString("##.00") + "</th>");
                Html.AppendLine("</tr>");
                                
                Transaction Trans = new Transaction();
                Trans.Detail = new TransactionDetail();
                Trans.CustomerSN = Item.SN;
                Trans.TradeTime = DateTime.Now;
                Trans.TradeType = Result >= 0 ? TranscationTypes.Dividend : TranscationTypes.Loss;
                Trans.Detail.BankName = " ";
                Trans.Detail.BankAccount = " ";
                Trans.Detail.BankCode = " ";
                Trans.Detail.Remark = " ";
                Trans.Detail.Draw = Result;
                Trans.Detail.Commission = 0;// Commission;
                TransDB.AddTranscation(Trans);

                if (SeT.EndTime.Day == DateTime.Now.Day && SeT.EndTime.Month == DateTime.Now.Month)
                {
                    TransDB.CaculateTransactionByCustomerSN(AdviserSN, Item.SN, SeT.BeginTime, SeT.EndTime);
                }
            }

            Html.AppendLine("</table> </body></html>");
            return Html.ToString();
        }

        /// <summary></summary>
        /// <returns></returns>
        public List<string> GetDueMonthList()
        {
            DataTable dt = USP.ExeProcedureGetDataTable(SP.GetDueMonthList);
            List<string> Li = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string str = dr[0].ToString();
                Li.Add(str);
            }
            return Li;
        }

        /// <summary>取得未平昌的交易紀錄</summary>
        /// <returns></returns>
        public List<TradeRecord> GetTradeRecordeByDueMonth(string DueMonth)
        {
            List<TradeRecord> TradeList = new List<TradeRecord>();
            USP.AddParameter(BaseData.BaseSParameter.DueMonth, DueMonth);
            TradeList = USP.ExeProcedureGetObjectList(SP.GetTradeRecordeByDueMonth, new TradeRecord());
            return TradeList;
        }
    }
}
using Adviser.Helpers;
using Adviser.Models;
using Adviser.Models.Code;
using ObjectBase;
using Resources;
using Stock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace Adviser.Controllers
{
    public class ReportController : BaseLoginController {
        private class Default {
            public const string DateTimeFormat = "yyyy-MM-dd";
            public const string DueMonth = "DueMonth";
            public const string TradeDate = "TradeDate";
            public const string Contract = "Contract";
            public const string Lot = "Lot";
            public const string OP = "OP";
            public const string Price = "Price";
            public const string Type = "Type";
            public const string StopPrice = "StopPrice";
            public const string PyeongchangTime = "PyeongchangTime";
            public const string IsPyeongchang = "IsPyeongchang";
            public const string Call = "Call";
            public const string Put = "Put";
            public const string Sell = "Sell";
            public const string Buy = "Buy";
            public const string SN = "SN";
            public const string IsMail = "IsMail";
        }

        /// <summary></summary>
        /// <returns></returns>
        public ActionResult RecordReport() {
            Stock.TradeRecordDAO TradeDAO = new Stock.TradeRecordDAO();
            DateTime NowTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime TimeEnd = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1);
            DataTable dt = TradeDAO.GetTradeRecordByDueDayPage(LoginHelper.GetLoginInfo().Adviser.SN, NowTime.ToString(Default.DateTimeFormat), TimeEnd.ToString(Default.DateTimeFormat), 300, 1);
            ReportViewModels.TradeRecordReportModel TradeModel = new ReportViewModels.TradeRecordReportModel();
            if (dt != null && dt.Rows.Count > 0) {
                TradeModel.BeginTime = NowTime;
                TradeModel.EndTime = TimeEnd;
                TradeModel.Page = 1;
                MachResult(dt, TradeModel);
                TradeModel.MaxPage = Convert.ToInt32(dt.Rows[0]["MaxPage"]);
            }
            return View(TradeModel);
        }//end RecordReport

        private void MachResult(DataTable dt, ReportViewModels.TradeRecordReportModel TradeModel) {
            LoginInfo Info = LoginHelper.GetLoginInfo(); 
            TradeModel._TradeRecord = new List<TradeRecord>();
            if (dt != null && dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    TradeRecord _TradeRecord = new TradeRecord();
                    _TradeRecord.SN = Convert.ToInt32(dr[Default.SN]);
                    _TradeRecord.TradeDate = dr[Default.TradeDate].ToString() == string.Empty ? DateTime.MinValue : Convert.ToDateTime(dr[Default.TradeDate]);
                    _TradeRecord.DueMonth = dr[Default.DueMonth].ToString();
                    _TradeRecord.OP = dr[Default.OP].ToString() == Default.Call ? ResourceReport.Label_Call : ResourceReport.Label_Put;
                    _TradeRecord.Contract = dr[Default.Contract].ToString();
                    _TradeRecord.Type = dr[Default.Type].ToString();
                    _TradeRecord.Lot = dr[Default.Lot].ToString();
                    _TradeRecord.Price = Convert.ToDecimal(dr[Default.Price]);
                    _TradeRecord.StopPrice = dr[Default.StopPrice].ToString() == string.Empty ? 0 : Convert.ToDecimal(dr[Default.StopPrice]);
                    _TradeRecord.PyeongchangTime = dr[Default.PyeongchangTime].ToString() == string.Empty ? DateTime.MinValue : Convert.ToDateTime(dr[Default.PyeongchangTime]);
                    _TradeRecord.IsPyeongchang = Convert.ToBoolean(dr[Default.IsPyeongchang]);
                    _TradeRecord.IsMail = Convert.ToBoolean(dr[Default.IsMail]);
                    decimal Point = 0;
                    if (_TradeRecord.Type == Default.Sell) {
                        Point = (_TradeRecord.Price - _TradeRecord.StopPrice) * Convert.ToDecimal(_TradeRecord.Lot);
                    }
                    else {
                        Point = (_TradeRecord.StopPrice - _TradeRecord.Price) * Convert.ToDecimal(_TradeRecord.Lot);
                    }
                    TradeModel.Total += _TradeRecord.IsPyeongchang == true ? Point : 0;
                    TradeModel._TradeRecord.Add(_TradeRecord);
                }
            }
        } //end MachResult
        
        public ActionResult SearchRecord(ReportRequest _Request) {
            Stock.TradeRecordDAO TradeDAO = new Stock.TradeRecordDAO();
            ReportViewModels.TradeRecordReportModel TradeModel = new ReportViewModels.TradeRecordReportModel();
            if (_Request.BeginTime > DateTime.MinValue && _Request.EndTime > DateTime.MinValue) {
                DataTable dt = TradeDAO.GetTradeRecordByDueDayPage(LoginHelper.GetLoginInfo().Adviser.SN, _Request.BeginTime.ToString(Default.DateTimeFormat), _Request.EndTime.ToString(Default.DateTimeFormat), 300, _Request.Page == 0 ? 1 : _Request.Page);
                MachResult(dt, TradeModel);
                TradeModel.BeginTime = _Request.BeginTime;
                TradeModel.EndTime = _Request.EndTime;
                TradeModel.Page = 1;
                if (dt != null && dt.Rows.Count > 0) {
                    TradeModel.MaxPage = Convert.ToInt32(dt.Rows[0]["MaxPage"]);
                }                
            }
            return View("RecordReport", TradeModel);
        }//end SearchRecord

        public dynamic ChagePage(ReportRequest _Request) {
            Stock.TradeRecordDAO TradeDAO = new Stock.TradeRecordDAO();
            ReportViewModels.TradeRecordReportModel TradeModel = new ReportViewModels.TradeRecordReportModel();
            if (_Request.BeginTime > DateTime.MinValue && _Request.EndTime > DateTime.MinValue) {
                DataTable dt = TradeDAO.GetTradeRecordByDueDayPage(LoginHelper.GetLoginInfo().Adviser.SN, _Request.BeginTime.ToString(Default.DateTimeFormat), _Request.EndTime.ToString(Default.DateTimeFormat), 300, _Request.Page == 0 ? 1 : _Request.Page);
                MachResult(dt, TradeModel);
                TradeModel.BeginTime = _Request.BeginTime;
                TradeModel.EndTime = _Request.EndTime;
                TradeModel.Page = _Request.Page;
                TradeModel.MaxPage = Convert.ToInt32(dt.Rows[0]["MaxPage"]);
            }
            return PartialView("_ReportTable", TradeModel);
        }//end ChagePage
        
        public ActionResult Bouns() {
            Adviser.Models.ReportViewModels.BounsViewModel ViewModel = new Adviser.Models.ReportViewModels.BounsViewModel();
            BounsDAO BounsDB = new BounsDAO();
            ViewModel.BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewModel.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
            ViewModel.Page = 1;
            int PageCount = 0;
            ViewModel.ListBouns = BounsDB.GetListBouns(ViewModel.BeginTime.ToString(Default.DateTimeFormat), ViewModel.EndTime.ToString(Default.DateTimeFormat), LoginHelper.GetLoginInfo().Adviser.SN, ViewModel.Page, out PageCount);
            ViewModel.MaxPage = PageCount;
            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult BounsSearch(Adviser.Models.ReportViewModels.BounsViewModel ViewModel) {
            BounsDAO BounsDB = new BounsDAO();
            int PageCount = 0;
            ViewModel.Page = 1;
            ViewModel.ListBouns = BounsDB.GetListBouns(ViewModel.BeginTime.ToString(Default.DateTimeFormat), ViewModel.EndTime.ToString(Default.DateTimeFormat), LoginHelper.GetLoginInfo().Adviser.SN, ViewModel.Page, out PageCount);
            ViewModel.MaxPage = PageCount;
            return View("Bouns", ViewModel);
        }
        
        /// <summary>平昌</summary>
        /// <param name="_TradeRecord"></param>
        /// <returns></returns>
        public dynamic Pyeongchang(TradeRecord _TradeRecord) {
            if (_TradeRecord.SN > 0) {
                Stock.TradeRecordDAO TradeDAO = new Stock.TradeRecordDAO();
                int Resut = 0;
                Resut = TradeDAO.PyeongchangTrade(_TradeRecord);
                if (Resut > 0) {
                    return "OK";
                }
                else {
                    return "Error";
                }
            }
            else {
                return "Error";
            }
        }//end Pyeongchang

        [HttpPost]
        public dynamic ChangeAlertSetting(TradeRecord _TradeRecord) {
            if (_TradeRecord.SN > 0) {
                Stock.TradeRecordDAO TradeDAO = new Stock.TradeRecordDAO();
                TradeRecord Result = TradeDAO.GetTradeRecordBySN(_TradeRecord.SN);
                Result.IsMail = _TradeRecord.IsMail;
                TradeDAO.UpdateTradeRecord(Result);
            }
            return "OK";
        }

        public ActionResult WeekPoint() {
            Adviser.Models.ReportViewModels.WeekPointViewModel WeekView = new ReportViewModels.WeekPointViewModel();
            WeekPointDAO WeekDB = new WeekPointDAO();
            WeekView.ListWeekPoint = WeekDB.GetListWeekPointByYear(DateTime.Now.Year);
            return View(WeekView);
        }

        [HttpPost]
        public ActionResult SearchWeekPoint(Adviser.Models.ReportViewModels.WeekPointViewModel WeekView) {
            WeekPointDAO WeekDB = new WeekPointDAO();
            WeekView.ListWeekPoint = WeekDB.GetListWeekPointByYear(WeekView.Year);
            return PartialView("_WeekPointPartial", WeekView);
        }
        
        public ActionResult TradeRecordeChart() {
            TradeRecordDAO DB = new TradeRecordDAO();
            List<string> Li = DB.GetDueMonthList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (string str in Li) {
                items.Add(new SelectListItem { Text = str, Value = str });
            }
            ViewBag.DueMonthList = items;
            return View();
        }

        [HttpPost]
        public ActionResult TradeChart() {
                                                                                                                                                                                        
            var DueMonth = Request["DueMonth"].ToString();
            TradeRecordDAO DB = new TradeRecordDAO();
            WeekPointDAO WeekDB = new WeekPointDAO();
            OptionDAO OpDB = new OptionDAO();

            List<TradeRecord> ListTrade = DB.GetTradeRecordeByDueMonth(DueMonth);
            List<WeekPoint> LiWeek = WeekDB.GetWeekPointByDueMonth(DueMonth);

            WeekPoint _Callweekpoion = LiWeek.Where(x => x.OP == "Call").ToList()[0];
            List<Option> ListCall = OpDB.GetHistoryOption(_Callweekpoion);




            return View();
        }

	}
}
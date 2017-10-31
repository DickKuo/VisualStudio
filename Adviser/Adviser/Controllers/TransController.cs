using Adviser.Helpers;
using Adviser.Models.Code;
using Adviser.Models.ViewModels;
using ObjectBase;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adviser.Controllers
{
    public class TransController : BaseLoginController
    {
        private class Default {
            public const string TimeFormat = "yyyy-MM-dd";
        }

        /// <summary>會員入金</summary>
        /// <returns></returns>
        public ActionResult Deposit()
        {
            TransactionViewModels.TransactionViewModel ViewModel = new TransactionViewModels.TransactionViewModel();
            ViewModel.BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewModel.EndTime = ViewModel.BeginTime.AddMonths(1).AddDays(-1); 
            TranscationDAO  TransDAO =new TranscationDAO();
            ViewModel.Page = 1;
            List<TransInfo> ListTrans = TransDAO.GetTransactionsNotAuditByTradeType(ViewModel.BeginTime.ToString(Default.TimeFormat), ViewModel.EndTime.ToString(Default.TimeFormat), 10, 1, TranscationTypes.Deposit);
            ViewModel.ListTransInfo = ListTrans;
            return View(ViewModel);
        }
        
        /// <summary></summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult Search(TransactionViewModels.TransactionViewModel ViewModel) {
            TranscationDAO TransDAO = new TranscationDAO();
            List<TransInfo> ListTrans = TransDAO.GetTransactionsNotAuditByTradeType(ViewModel.BeginTime.ToString(Default.TimeFormat), ViewModel.EndTime.ToString(Default.TimeFormat), 10, 1, TranscationTypes.Deposit);
            ViewModel.ListTransInfo = ListTrans;
            return View("Deposit", ViewModel);
        }

        /// <summary>交易明細</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult TransDetail(TransactionViewModels.TransactionViewModel ViewModel) {
            if (ViewModel.TransSN >0) {
                TranscationDAO _TransDAO = new TranscationDAO();
                CustomerDAO _CustomerDAO = new CustomerDAO();   
                Transaction _Trans = _TransDAO.GetTranscationBySN(ViewModel.TransSN);
                ViewModel.Trans = _Trans;
                ViewModel._Customer = _CustomerDAO.GetCustomerBySN(_Trans.CustomerSN);
                ViewBag.AuditList = base.GetAuditItemsNoSelected();
                return View(ViewModel);
            }
            else {
                return RedirectToAction("Index","Home");
            }           
        }

        /// <summary>會員出金申請</summary>
        /// <returns></returns>
        public ActionResult Withdrawal() {
            TransactionViewModels.TransactionViewModel ViewModel = new TransactionViewModels.TransactionViewModel();
            DateTime BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime EndTime = BeginTime.AddMonths(1).AddDays(-1);
            TranscationDAO TransDAO = new TranscationDAO();
            ViewModel.Page = 1;
            List<TransInfo> ListTrans = TransDAO.GetTransactionsNotAuditByTradeType(BeginTime.ToString(Default.TimeFormat), EndTime.ToString(Default.TimeFormat), 10, 1, TranscationTypes.Withdrawal);
            ViewModel.ListTransInfo = ListTrans;
            return View(ViewModel);
        }

        /// <summary>審核交易單</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult AuditTrans(TransactionViewModels.TransactionViewModel ViewModel) {
            if (ViewModel.TransSN > 0) {
                LoginInfo Info = LoginHelper.GetLoginInfo();
                TranscationDAO TransDAO = new TranscationDAO();
                TransDAO.AuditTranscation(ViewModel.TransSN, ViewModel.Audit, Info.Adviser.SN);
                return RedirectToAction(ViewModel.Trans.TradeType.ToString(), "Trans");
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }

        ///// <summary></summary>
        ///// <returns></returns>
        //public ActionResult Quotes(string DueMonth) {
        //    DateTime NowTime = DateTime.Now;
        //    DateTime TimeEnd = NowTime.AddDays(1);
        //    TradeViewModels.TradeViewModel ViewModel = new TradeViewModels.TradeViewModel();
        //    CalendarDAO _CalendarDAO = new CalendarDAO();
        //    Calendar _Calendar = _CalendarDAO.GetCalendar(DateTime.Now);
        //    ViewBag.DueMonthList = GetDueMonthItems(_Calendar);
        //    ViewModel.BeginTime = NowTime;
        //    ViewModel.EndTime = TimeEnd;
        //    ViewModel.Page = 1;
        //    ViewModel.DueMonth = string.IsNullOrEmpty(DueMonth) == false ? DueMonth : _Calendar.Week;
        //    ViewModel = SearchQuotes(ViewModel);
        //    return View(ViewModel);
        //}//end Quotes

	}
}
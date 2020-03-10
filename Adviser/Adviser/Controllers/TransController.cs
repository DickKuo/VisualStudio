using System.Collections.Generic;
using Adviser.Models.ViewModels;
using Adviser.Models.Code;
using Adviser.Helpers;
using System.Web.Mvc;
using ObjectBase;
using System;

namespace Adviser.Controllers
{
    public class TransController : BaseLoginController
    {       
        /// <summary>會員入金</summary>
        /// <returns></returns>
        public ActionResult Deposit()
        {
            TransactionViewModels.TransactionViewModel ViewModel = new TransactionViewModels.TransactionViewModel();
            ViewModel.TransType = TranscationTypes.Deposit;
            ViewModel.BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewModel.EndTime = ViewModel.BeginTime.AddMonths(1).AddDays(-1);
            TranscationDAO TransDAO = new TranscationDAO();
            ViewModel.Page = 1;
            List<TransInfo> ListTrans = TransDAO.GetTransactionsNotAuditByTradeType(ViewModel.BeginTime.ToString(TimeFormat), ViewModel.EndTime.ToString(TimeFormat), 10, 1, ViewModel.TransType);
            ViewModel.ListTransInfo = ListTrans;
            ViewBag.AuditList = GetAuditItemsNoSelected();
            return View(ViewModel);
        }

        /// <summary></summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult Search(TransactionViewModels.TransactionViewModel ViewModel)
        {
            TranscationDAO TransDAO = new TranscationDAO();
            List<TransInfo> ListTrans = TransDAO.GetTransactionsByCondition(ViewModel.BeginTime.ToString(TimeFormat), ViewModel.EndTime.ToString(TimeFormat), 10, 1, ViewModel.TransType, ViewModel.Audit);
            ViewModel.ListTransInfo = ListTrans;
            ViewBag.AuditList = GetAuditItemsNoSelected();
            return View(ViewModel.TransType.ToString(), ViewModel);
        }

        /// <summary>交易明細</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult TransDetail(TransactionViewModels.TransactionViewModel ViewModel)
        {
            if (ViewModel.TransSN > 0)
            {
                TranscationDAO _TransDAO = new TranscationDAO();
                CustomerDAO _CustomerDAO = new CustomerDAO();
                Transaction _Trans = _TransDAO.GetTranscationBySN(ViewModel.TransSN);
                ViewModel.Trans = _Trans;
                ViewModel._Customer = _CustomerDAO.GetCustomerBySN(_Trans.CustomerSN);
                ViewBag.AuditList = base.GetAuditItemsNoSelected();
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>會員出金申請</summary>
        /// <returns></returns>
        public ActionResult Withdrawal()
        {
            TransactionViewModels.TransactionViewModel ViewModel = new TransactionViewModels.TransactionViewModel();
            ViewModel.TransType = TranscationTypes.Withdrawal;
            ViewModel.BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewModel.EndTime = ViewModel.BeginTime.AddMonths(1).AddDays(-1);
            TranscationDAO TransDAO = new TranscationDAO();
            ViewModel.Page = 1;
            List<TransInfo> ListTrans = TransDAO.GetTransactionsNotAuditByTradeType(ViewModel.BeginTime.ToString(TimeFormat), ViewModel.EndTime.ToString(TimeFormat), 10, 1, ViewModel.TransType);
            ViewModel.ListTransInfo = ListTrans;
            ViewBag.AuditList = GetAuditItemsNoSelected();
            return View(ViewModel);
        }

        /// <summary>審核交易單</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult AuditTrans(TransactionViewModels.TransactionViewModel ViewModel)
        {
            if (ViewModel.TransSN > 0)
            {
                LoginInfo Info = LoginHelper.GetLoginInfo();
                TranscationDAO TransDAO = new TranscationDAO();
                TransDAO.AuditTranscation(ViewModel.TransSN, ViewModel.Audit, Info.Adviser.SN);
                return RedirectToAction(ViewModel.Trans.TradeType.ToString(), "Trans");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
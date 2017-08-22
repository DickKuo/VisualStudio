using ObjectBase;
using System;
using System.Web.Mvc;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;
using WebApplication1.Models.Code;

namespace WebApplication1.Controllers
{
    public class TransactionController : BaseLoginController
    {
        private class Default {
            public const string DateTimeFormat = "yyyy-MM-dd";
        }

        /// <summary>初始畫面</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            TranscationViewModels.TranscationViewModel Model = new TranscationViewModels.TranscationViewModel();
            DateTime NowTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime TimeEnd = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1);
            Model.BeginTime = NowTime;
            Model.EndTime = TimeEnd;
            Model.Page = 1;
            LoginInfo Info =LoginHelper.GetLoginInfo();
            TranscationDAO DAO = new TranscationDAO();
            Model.TransactionList =  DAO.GetTop10TransactionByCustomerSN(NowTime.ToString(Default.DateTimeFormat), TimeEnd.ToString(Default.DateTimeFormat), Info.Customer.SN);
            Model.MaxPage = DAO.GetTranscationPagesByDueDay(NowTime.ToString(Default.DateTimeFormat), TimeEnd.ToString(Default.DateTimeFormat), Info.Customer.SN,10);
            return View(Model);
        }//end Index

        /// <summary>換頁</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public dynamic ChagePage(TranscationViewModels.TranscationViewModel Model) {            
            if (Model.BeginTime > DateTime.MinValue && Model.EndTime > DateTime.MinValue) {
                LoginInfo Info = LoginHelper.GetLoginInfo();
                TranscationDAO DAO = new TranscationDAO();               
                Model.TransactionList = DAO.GetTransactionByCustomerSNPages(Model.BeginTime.ToString(Default.DateTimeFormat), Model.EndTime.ToString(Default.DateTimeFormat), Info.Customer.SN, Model.Page, Model.Range);
                Model.MaxPage = DAO.GetTranscationPagesByDueDay(Model.BeginTime.ToString(Default.DateTimeFormat), Model.EndTime.ToString(Default.DateTimeFormat), Info.Customer.SN, 10);
            }
            return PartialView("_TransactionTable", Model);  
        }//end ChagePage

        /// <summary></summary>
        /// <param name="_BaseRequest"></param>
        /// <returns></returns>
        public ActionResult TranscationHistory(BaseRequest _BaseRequest) {
            DateTime NowTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime TimeEnd = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1);
            return null;
        }//end TranscationHistory

	}
}
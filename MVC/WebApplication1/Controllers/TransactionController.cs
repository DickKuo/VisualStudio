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
        public ActionResult Index(TranscationViewModels.TranscationViewModel Model)
        {            
            DateTime NowTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime TimeEnd = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1);
            Model.BeginTime = Model.BeginTime == DateTime.MinValue ? NowTime : Model.BeginTime;
            Model.EndTime = Model.EndTime == DateTime.MinValue ? TimeEnd : Model.EndTime;
            SearchMothed(Model);
            return View(Model);
        }//end Index

        private  void SearchMothed(TranscationViewModels.TranscationViewModel Model) {
            int MaxPage = 0;
            LoginInfo Info = LoginHelper.GetLoginInfo();
            TranscationDAO DAO = new TranscationDAO();
            Model.TransactionList = DAO.GetTransactionByCustomerSNPages(Model.BeginTime.ToString(Default.DateTimeFormat), Model.EndTime.ToString(Default.DateTimeFormat), Info.Customer.SN, Model.Page, Model.Range, Convert.ToInt32(Model.TradeType), Convert.ToInt32(Model.AuditState), out MaxPage);
            Model.MaxPage = DAO.GetTranscationPagesByDueDay(Model.BeginTime.ToString(Default.DateTimeFormat), Model.EndTime.ToString(Default.DateTimeFormat), Info.Customer.SN, 10);
        }

        /// <summary>換頁</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public dynamic ChagePage(TranscationViewModels.TranscationViewModel Model) {            
            if (Model.BeginTime > DateTime.MinValue && Model.EndTime > DateTime.MinValue) {
                SearchMothed(Model);
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
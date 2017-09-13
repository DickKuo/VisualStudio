using ObjectBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;
using WebApplication1.Models.Code;

namespace WebApplication1.Controllers
{
    public class WithdrawalController : BaseLoginController
    { 
        /// <summary>出金畫面</summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            WebApplication1.Models.DepositViewModels.DepositViewModel ViewModel = new Models.DepositViewModels.DepositViewModel();
            LoginInfo Info = LoginHelper.GetLoginInfo();
            ViewModel._Customer = Info.Customer;
            ViewModel._Transaction = new ObjectBase.Transaction();
            ViewModel._Transaction.Detail = new ObjectBase.TransactionDetail();
            return View(ViewModel);
        }//end Index

        /// <summary>出金申請</summary>
        /// <param name="_BaseRequest"></param>
        /// <returns></returns>
        public ActionResult Withdrawal(BaseRequest _BaseRequest) {
            CustomerDAO CusDAO = new CustomerDAO();
            Customer _Customer = CusDAO.GetCustomerByAccount(_BaseRequest.Account);
            if (_Customer.SN > 0) {
                if (_Customer.Audit == AuditTypes.OK) {
                    TranscationDAO TransDAO = new TranscationDAO();
                    Transaction Trans = new Transaction();
                    Trans.CustomerSN = _Customer.SN;
                    Trans.TradeType = TranscationTypes.Withdrawal;
                    TransactionDetail TransDetail = new TransactionDetail();
                    TransDetail.Draw = _BaseRequest.Draw;
                    TransDetail.Remark = _BaseRequest.Remark;
                    TransDetail.BankAccount = _BaseRequest.BankAccount;
                    TransDetail.BankName = _BaseRequest.BankName;
                    TransDetail.BranchName = _BaseRequest.BranchName;
                    Trans.Detail = TransDetail;
                    TransDAO.AddTranscation(Trans);
                }
            }
            return RedirectToAction("Index", "EWallet");
        }//end Withdraw

	}
}
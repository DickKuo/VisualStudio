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
        public ActionResult Index(string TransKey)
        {
            try {
                WebApplication1.Models.DepositViewModels.DepositViewModel ViewModel = new Models.DepositViewModels.DepositViewModel();
                LoginInfo Info = LoginHelper.GetLoginInfo();
                ViewModel._Customer = Info.Customer;
                if (!string.IsNullOrEmpty(TransKey)) {
                    ViewModel._PageAction = WebApplication1.Models.Code.BaseCode.PageAction.View;
                    TranscationDAO TransDAO = new TranscationDAO();
                    ViewModel._Transaction = TransDAO.GetTranscationByTranskey(TransKey);
                    return View(ViewModel);
                }
                else {
                    ViewModel._Transaction = new ObjectBase.Transaction();
                    ViewModel._Transaction.Detail = new ObjectBase.TransactionDetail();
                    return View(ViewModel);
                }
            }
            catch (Exception ex) {
                Log(ex);
                return ReturnMessage(Resources.ResourceDeposit.Withdrawal_Fail, "~/EWallet/Index", BaseCode.MessageType.danger);
            }
        }//end Index

        /// <summary>出金申請</summary>
        /// <param name="_BaseRequest"></param>
        /// <returns></returns>
        public ActionResult Withdrawal(BaseRequest _BaseRequest) {
            if (!string.IsNullOrEmpty(_BaseRequest.Status) && _BaseRequest.Status == "OK") {
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
                        TransDetail.BankCode = _BaseRequest.BankCode;
                        Trans.Detail = TransDetail;
                        if (TransDetail.Draw > 100) {
                            int Result = TransDAO.AddTranscation(Trans);
                            if (Result > 0) {
                                AdviserDAO _AdviserDB = new AdviserDAO();
                                Adviser _Adviser = _AdviserDB.GetAdviserBySN(_Customer.HelperSN);
                                CommTool.MailData _MailData = new CommTool.MailData();
                                _MailData.RegistrySend(_Adviser.Email, "會員申請出金通知", string.Format("會員帳號:{0} 申請出金，請審核!", _Customer.Account));
                                return ReturnMessage(Resources.ResourceDeposit.Withdrawal_Success, "~/EWallet/Index", BaseCode.MessageType.success);
                            }
                            else {
                                return ReturnMessage(Resources.ResourceDeposit.Withdrawal_Fail, "~/EWallet/Index", BaseCode.MessageType.danger);
                            }
                        }
                        else {
                            return ReturnMessage(Resources.ResourceDeposit.Withdrawal_Fail, "~/EWallet/Index", BaseCode.MessageType.danger);
                        }
                    }
                } 
                return ReturnMessage(Resources.ResourceDeposit.Withdrawal_Fail, "~/EWallet/Index", BaseCode.MessageType.danger);
            }
            else {
                return RedirectToAction("Index", "EWallet");
            }
        }//end Withdraw

	}
}
using ObjectBase;
using System.Web.Mvc;
using WebApplication1.Code.DAO;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;
using WebApplication1.Models.Code;
using System.Collections.Generic;
using CommTool;
using System.Web;
using System;
using System.IO;
using System.Drawing;

namespace WebApplication1.Controllers
{
    public class DepositController : BaseLoginController
    {

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
            catch(Exception ex){
                Log(ex);
                return ReturnMessage(Resources.ResourceDeposit.Deposit_Fail, "~/EWallet/Index", BaseCode.MessageType.danger);
            }
        }

        /// <summary>入金開始</summary>
        /// <returns></returns>
        public ActionResult Deposit(DepositRequest _Request) {
            if (!string.IsNullOrEmpty(_Request.Status) && _Request.Status == "OK") {
                CustomerDAO CusDAO = new CustomerDAO();
                Customer _Customer = CusDAO.GetCustomerByAccount(_Request.Account);
                if (_Customer.SN > 0) {
                    if (_Customer.Audit == AuditTypes.OK) {
                        try {
                            TranscationDAO TransDAO = new TranscationDAO();
                            Transaction Trans = new Transaction();
                            AddAttachments(_Request.MoneyOrder1, Trans.AttachmentsList);
                            AddAttachments(_Request.MoneyOrder2, Trans.AttachmentsList);
                            AddAttachments(_Request.MoneyOrder3, Trans.AttachmentsList);
                            Trans.CustomerSN = _Customer.SN;
                            Trans.TradeType = TranscationTypes.Deposit;
                            TransactionDetail TransDetail = new TransactionDetail();
                            TransDetail.Draw = _Request.Draw;
                            TransDetail.Remark = string.IsNullOrEmpty(_Request.Remark) ? string.Empty : _Request.Remark;
                            TransDetail.BankAccount = string.Empty;
                            TransDetail.BankName = string.Empty;
                            TransDetail.BankCode = string.Empty;
                            Trans.Detail = TransDetail;
                            if (TransDetail.Draw > 100) {
                                int Result = TransDAO.AddTranscation(Trans);
                                if (Result > 0) {
                                    AdviserDAO _AdviserDB = new AdviserDAO();
                                    Adviser _Adviser = _AdviserDB.GetAdviserBySN(_Customer.HelperSN);
                                    CommTool.MailData _MailData = new CommTool.MailData();
                                    _MailData.RegistrySend(_Adviser.Email, "會員申請入金通知", string.Format("會員帳號:{0} 申請入金，請審核!", _Customer.Account));
                                    return ReturnMessage(Resources.ResourceDeposit.Deposit_Success, "~/EWallet/Index", BaseCode.MessageType.success);
                                }
                                else {
                                    return ReturnMessage(Resources.ResourceDeposit.Deposit_Fail, "~/EWallet/Index", BaseCode.MessageType.danger);
                                }
                            }
                            else {
                                return ReturnMessage(Resources.ResourceDeposit.Deposit_Fail, "~/EWallet/Index", BaseCode.MessageType.danger);
                            }
                        }
                        catch (Exception ex) {
                            Log(ex);
                            return ReturnMessage(Resources.ResourceDeposit.Deposit_Fail, "~/EWallet/Index", BaseCode.MessageType.danger);
                        }
                    }
                }
            }
            return RedirectToAction("Index", "EWallet");
        }

        private void AddAttachments(HttpPostedFileBase FileBase, List<Attachments> AttachmentsList) {
            if (FileBase != null) {
                Attachments att = new Attachments();
                att.AttName = CommTool.Files.StreamToBase64(FileBase.InputStream); 
                att.AttType = (int)AttTypes.Deposit;
                AttachmentsList.Add(att);               
            }
        }

	}
}
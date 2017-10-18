using ObjectBase;
using System.Web.Mvc;
using WebApplication1.Code.DAO;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;
using WebApplication1.Models.Code;

namespace WebApplication1.Controllers
{
    public class DepositController : BaseLoginController
    {
       
        public ActionResult Index()
        {
            WebApplication1.Models.DepositViewModels.DepositViewModel ViewModel = new Models.DepositViewModels.DepositViewModel();
            LoginInfo Info = LoginHelper.GetLoginInfo();
            ViewModel._Customer = Info.Customer;
            ViewModel._Transaction = new ObjectBase.Transaction();
            ViewModel._Transaction.Detail = new ObjectBase.TransactionDetail();
            return View(ViewModel);
        }

        /// <summary>入金開始</summary>
        /// <returns></returns>
        public ActionResult Deposit(BaseRequest _Request) {
            CustomerDAO CusDAO = new CustomerDAO();
            Customer _Customer = CusDAO.GetCustomerByAccount(_Request.Account);
            if (_Customer.SN > 0) {
                if (_Customer.Audit == AuditTypes.OK)
                {
                    TranscationDAO TransDAO = new TranscationDAO();
                    Transaction Trans = new Transaction();
                    Trans.CustomerSN = _Customer.SN;
                    Trans.TradeType = TranscationTypes.Deposit;
                    TransactionDetail TransDetail = new TransactionDetail();
                    TransDetail.Draw = _Request.Draw;
                    TransDetail.Remark = _Request.Remark;
                    TransDetail.BankAccount = string.Empty;
                    TransDetail.BankName = string.Empty;
                    TransDetail.BranchName = string.Empty;
                    Trans.Detail = TransDetail;
                    int Result = TransDAO.AddTranscation(Trans);
                    if (Result > 0)
                    {
                        AdviserDAO _AdviserDB = new AdviserDAO();                     
                        Adviser _Adviser = _AdviserDB.GetAdviserBySN(_Customer.HelperSN);
                        CommTool.MailData _MailData = new CommTool.MailData();
                        _MailData.RegistrySend(_Adviser.Email, "會員申請入金通知", string.Format("會員帳號:{0} 申請入金，請審核!", _Customer.Account));
                        return ReturnMessage(Resources.ResourceDeposit.Deposit_Success, "~/EWallet/Index", BaseCode.MessageType.success);                        
                    }
                    else
                    {
                        return ReturnMessage(Resources.ResourceDeposit.Deposit_Fail, "~/EWallet/Index", BaseCode.MessageType.danger);                        
                    }
                }
            }
            return RedirectToAction("Index", "EWallet");
        } 

	}
}
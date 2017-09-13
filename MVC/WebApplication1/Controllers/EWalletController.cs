using ObjectBase;
using System;
using System.Web.Mvc;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EWalletController : BaseLoginController
    {
        public ActionResult Index()
        {
            WebApplication1.Models.EWalletViewModels.EWalletViewModel WalletModel = new Models.EWalletViewModels.EWalletViewModel();
            LoginInfo Info = LoginHelper.GetLoginInfo();
            EWalletDAO WalletDAO = new EWalletDAO();

            DateTime BeginTime =new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);
            DateTime EndTime =BeginTime.AddMonths(1).AddDays(-1);

            EWallet Wallet = WalletDAO.GetEWalletByCustomerSN(Info.Customer.SN);
            WalletModel._Customer = Info.Customer;
            WalletModel._EWallet = Wallet;

            TranscationDAO TransDAO = new TranscationDAO();            

            WalletModel.TransList = TransDAO.GetTop10TransactionByCustomerSN(BeginTime.ToString("yyyy-MM-dd"), EndTime.ToString("yyyy-MM-dd"), Info.Customer.SN);
            
            return View(WalletModel);
        }
	}
}
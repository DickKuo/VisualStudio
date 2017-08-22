using ObjectBase;
using System;
using System.Web.Mvc;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SettingController : Controller
    {
        /// <summary>初始頁面</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult Index(WebApplication1.Models.SettingViewModels.SettingViewModel ViewModel) {
            LoginInfo Info = LoginHelper.GetLoginInfo();
            ViewModel.MinimunLotLimit = Info.Customer.MinimunLotLimit;
            return View(ViewModel);
        }//end Index

        /// <summary>變更最小投資組數設定</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public dynamic ChangeMiniLot(WebApplication1.Models.SettingViewModels.SettingViewModel ViewModel) {
            if (ViewModel.MinimunLotLimit < 0) {
                ViewModel.MinimunLotLimit = ViewModel.MinimunLotLimit * (-1);
            }
            ExtraTagDAO ExtrDAO = new ExtraTagDAO();
            LoginInfo Info = LoginHelper.GetLoginInfo();
            if (ViewModel.MinimunLotLimit == (int)MinimunLotLimit.NotLimit) {
                ExtrDAO.UpdateExtraTag(ExtraUserType.Customer, ExtraClass.MinimunLot, Info.Customer.SN, ViewModel.MinimunLotLimit);
                Info.Customer.MinimunLotLimit = ViewModel.MinimunLotLimit;
                LoginHelper.SetSeesion(Info);
                return "OK";
            }
            else {
                EWalletDAO WalletDAO = new EWalletDAO();
                EWallet _EWallet = WalletDAO.GetEWalletByCustomerSN(Info.Customer.SN);
                int Reult = Convert.ToInt32(_EWallet.Balance / 2000);
                if (ViewModel.MinimunLotLimit <= Reult) {
                    ExtrDAO.UpdateExtraTag(ExtraUserType.Customer, ExtraClass.MinimunLot, Info.Customer.SN, ViewModel.MinimunLotLimit);
                    Info.Customer.MinimunLotLimit = ViewModel.MinimunLotLimit;
                    LoginHelper.SetSeesion(Info);
                    return "OK";
                }
                else {
                    return "Error";
                }
            }
        }//end ChangeMiniLot

	}
}
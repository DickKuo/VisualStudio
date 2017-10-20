using ObjectBase;
using System;
using System.Web.Mvc;
using WebApplication1.Code.Helpers;
using WebApplication1.Models; 

namespace WebApplication1.Controllers
{
    public class SettingController : BaseLoginController
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
        
        /// <summary>銀行設定瀏覽頁面</summary>
        /// <returns></returns>
        public ActionResult Bank() {
            WebApplication1.Models.BankViewModels.BankViewModel Model = new BankViewModels.BankViewModel();
            BankDAO BankDB = new BankDAO();
            Model.ListBank = BankDB.GetBankByCustomerSN(LoginHelper.GetLoginInfo().Customer.SN);
            return View(Model);
        }
        
        /// <summary>銀行設定瀏覽頁面</summary>
        /// <returns></returns>
        public ActionResult BankEdit(WebApplication1.Models.BankViewModels.BankViewModel Model) {

            switch (Model._PageAction) {
                case WebApplication1.Models.Code.BaseCode.PageAction.Add:
                    break;
                case WebApplication1.Models.Code.BaseCode.PageAction.Edit:
                    BankDAO BankDB = new BankDAO();
                    Model._Bank = BankDB.GetBankBySN(Model.BankSN);
                    break;
            }
            return View(Model);
        }

        /// <summary>BankEditPost</summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ActionResult BankEditPost(WebApplication1.Models.BankViewModels.BankViewModel Model) {
            string BackUrl = "../Setting/Bank";
            BankDAO BankDB = new BankDAO();
            int Result = 0;
            Model._Bank.CustomerSN = LoginHelper.GetLoginInfo().Customer.SN;
            switch (Model._PageAction) {
                case WebApplication1.Models.Code.BaseCode.PageAction.Add:                 
                    Result = BankDB.AddBank(Model._Bank);
                    if (Result > 0) {
                        return ReturnMessage("新增銀行成功", BackUrl, WebApplication1.Models.Code.BaseCode.MessageType.success);
                    }
                    else {
                        return ReturnMessage("新增銀行失敗", BackUrl, WebApplication1.Models.Code.BaseCode.MessageType.danger);
                    }
                case WebApplication1.Models.Code.BaseCode.PageAction.Edit:
                    Result = BankDB.UpdateBankBySN(Model._Bank);
                    if (Result > 0) {
                        return ReturnMessage("更新銀行成功", BackUrl, WebApplication1.Models.Code.BaseCode.MessageType.success);
                    }
                    else {
                        return ReturnMessage("更新銀行失敗", BackUrl, WebApplication1.Models.Code.BaseCode.MessageType.danger);
                    }
            }
            return ReturnMessage("未做任何動作", "../Home/Index", WebApplication1.Models.Code.BaseCode.MessageType.info);
        }

	}
}
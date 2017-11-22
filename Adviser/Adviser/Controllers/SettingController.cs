using ObjectBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adviser.Controllers
{
    public class SettingController : BaseLoginController
    {
        // GET: Setting
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChipsSetting()
        {
            Adviser.Models.ViewModels.SettingViewModels.ChipsSettingViewModel ViewModel = new Models.ViewModels.SettingViewModels.ChipsSettingViewModel();
            CustomerDAO _CustomerDAO = new CustomerDAO();
            ViewModel.ListCustomer = _CustomerDAO.GetCustomerListByPage(1, 10);
            return View(ViewModel);            
        }

        public ActionResult ChipsSettingEdit(Adviser.Models.ViewModels.SettingViewModels.ChipsSettingViewModel ViewModel)
        {            
            CustomerDAO _CustomerDAO = new CustomerDAO();
            ViewModel._Customer = _CustomerDAO.GetCustomerBySN(ViewModel.CustomerSN);
            EWalletDAO WalletDAO = new EWalletDAO();
            EWallet Wallet = WalletDAO.GetEWalletByCustomerSN(ViewModel._Customer.SN);
            ViewModel._EWallet = Wallet;
            return View(ViewModel);
        }
        
        public ActionResult ChipsSettingEditPost(Adviser.Models.ViewModels.SettingViewModels.ChipsSettingViewModel ViewModel)
        {
            CustomerDAO _CustomerDAO = new CustomerDAO();
            int Result = _CustomerDAO.UpdateCustomerChipsByAccount(ViewModel._Customer);
            if (Result > 0) {
                return RedirectToAction("ChipsSetting", "Setting");
            }
            else {
                return RedirectToAction("Index", "Home");
            }           
        }

        public ActionResult SettleSetting() {
            Adviser.Models.ViewModels.SettingViewModels.SettleSettingViewModel ViewModel = new Models.ViewModels.SettingViewModels.SettleSettingViewModel();
            SettleTimeDAO _SettleTimeDAO = new SettleTimeDAO();
            ViewModel.ListSettleTime = _SettleTimeDAO.GetListSettleTime();
            return View(ViewModel); 
        }

        public ActionResult SettleSettingEdit(Adviser.Models.ViewModels.SettingViewModels.SettleSettingViewModel ViewModel) {
            if (ViewModel.PageAction == "Add") {
                ViewModel._SettleTime = new SettleTime();
                ViewModel._SettleTime.BeginTime = DateTime.Now;
                ViewModel._SettleTime.EndTime = DateTime.Now;
                return View(ViewModel);
            }
            else {
                SettleTimeDAO _SettleTimeDAO = new SettleTimeDAO();
                ViewModel._SettleTime = _SettleTimeDAO.GetSettleTimeBySN(ViewModel.SettleTimeSN);
                return View(ViewModel);
            }
        }

        public ActionResult SettleSettingEditPost(Adviser.Models.ViewModels.SettingViewModels.SettleSettingViewModel ViewModel) {                      
            SettleTimeDAO _SettleTimeDAO = new SettleTimeDAO();
            int Result = 0;
            if (ViewModel.PageAction == "Add") {
                Result = _SettleTimeDAO.AddSettleTime(ViewModel._SettleTime);
            }
            else {
                Result = _SettleTimeDAO.UpdateSettleTime(ViewModel._SettleTime);
            }
            if (Result > 0) {
                return RedirectToAction("SettleSetting", "Setting");
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }
        
    }
}
using System.Collections.Generic;
using System.Web.Mvc;
using ObjectBase;
using System; 

namespace Adviser.Controllers
{
    public class SettingController : BaseLoginController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChipsSetting()
        {
            Models.ViewModels.SettingViewModels.ChipsSettingViewModel ViewModel = new Models.ViewModels.SettingViewModels.ChipsSettingViewModel();
            CustomerDAO _CustomerDAO = new CustomerDAO();
            ViewModel.ListCustomer = _CustomerDAO.GetCustomerListByPage(1, 10);
            return View(ViewModel);
        }

        public ActionResult ChipsSettingEdit(Models.ViewModels.SettingViewModels.ChipsSettingViewModel ViewModel)
        {
            CustomerDAO _CustomerDAO = new CustomerDAO();
            ViewModel._Customer = _CustomerDAO.GetCustomerBySN(ViewModel.CustomerSN);
            EWalletDAO WalletDAO = new EWalletDAO();
            EWallet Wallet = WalletDAO.GetEWalletByCustomerSN(ViewModel._Customer.SN);
            ViewModel._EWallet = Wallet;
            return View(ViewModel);
        }

        public ActionResult ChipsSettingEditPost(Models.ViewModels.SettingViewModels.ChipsSettingViewModel ViewModel)
        {
            CustomerDAO _CustomerDAO = new CustomerDAO();
            int Result = _CustomerDAO.UpdateCustomerChipsByAccount(ViewModel._Customer);
            if (Result > 0)
            {
                return RedirectToAction("ChipsSetting", "Setting");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult SettleSetting()
        {
            Models.ViewModels.SettingViewModels.SettleSettingViewModel ViewModel = new Models.ViewModels.SettingViewModels.SettleSettingViewModel();
            SettleTimeDAO _SettleTimeDAO = new SettleTimeDAO();
            ViewModel.ListSettleTime = _SettleTimeDAO.GetListSettleTime();
            return View(ViewModel);
        }

        public ActionResult SettleSettingEdit(Models.ViewModels.SettingViewModels.SettleSettingViewModel ViewModel)
        {
            if (ViewModel.PageAction == "Add")
            {
                ViewModel._SettleTime = new SettleTime();
                ViewModel._SettleTime.BeginTime = DateTime.Now;
                ViewModel._SettleTime.EndTime = DateTime.Now;
                return View(ViewModel);
            }
            else
            {
                SettleTimeDAO _SettleTimeDAO = new SettleTimeDAO();
                ViewModel._SettleTime = _SettleTimeDAO.GetSettleTimeBySN(ViewModel.SettleTimeSN);
                return View(ViewModel);
            }
        }

        public ActionResult SettleSettingEditPost(Models.ViewModels.SettingViewModels.SettleSettingViewModel ViewModel)
        {
            SettleTimeDAO _SettleTimeDAO = new SettleTimeDAO();
            int Result = 0;
            if (ViewModel.PageAction == "Add")
            {
                Result = _SettleTimeDAO.AddSettleTime(ViewModel._SettleTime);
            }
            else
            {
                Result = _SettleTimeDAO.UpdateSettleTime(ViewModel._SettleTime);
            }
            if (Result > 0)
            {
                return RedirectToAction("SettleSetting", "Setting");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CalendarSetting()
        {
            Models.ViewModels.SettingViewModels.CalendarSetting Model = new Models.ViewModels.SettingViewModels.CalendarSetting();
            Stock.CalendarDAO _DB = new Stock.CalendarDAO();
            List<Stock.Calendar> ListCalendar = new List<Stock.Calendar>();
            ListCalendar = _DB.GetCalendarByMonth(DateTime.Now);
            foreach (var item in ListCalendar)
            {
                Models.Code.CalendarSet _Week = new Models.Code.CalendarSet();
                _Week.Title = item.Week;
                _Week.NowDay = item.Daily;
                Model.listCalendar.Add(_Week);               

                Models.Code.CalendarSet NearMonth1 = new Models.Code.CalendarSet();
                NearMonth1.Title = item.NearMonth1;
                NearMonth1.NowDay = item.Daily;
                Model.listCalendar.Add(NearMonth1);               

                Models.Code.CalendarSet NearMonth2 = new Models.Code.CalendarSet();
                NearMonth2.Title = item.NearMonth2;
                NearMonth2.NowDay = item.Daily;
                Model.listCalendar.Add(NearMonth2);              
            }
            return View(Model);
        }
    }
}
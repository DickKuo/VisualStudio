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
            return View(ViewModel);
        }

        
        public ActionResult ChipsSettingEditPost(Adviser.Models.ViewModels.SettingViewModels.ChipsSettingViewModel ViewModel)
        {
            CustomerDAO _CustomerDAO = new CustomerDAO();
            int Result  = _CustomerDAO.UpdateCustomerChipsByAccount(ViewModel._Customer);
            if (Result>0)
            {
                return RedirectToAction("ChipsSetting", "Setting");
            }
            else {
                return RedirectToAction("Index","Home");
            }

           
        }


    }
}
using Adviser.Models.ViewModels;
using System.Web.Mvc;
using ObjectBase;

namespace Adviser.Controllers
{
    public class CustomerController : BaseLoginController
    {
        public ActionResult Mangement()
        {
            CustomerViewModels.CustomerViewModel ViewModel = new CustomerViewModels.CustomerViewModel();
            CustomerDAO _CustomerDAO = new CustomerDAO();
            ViewModel.ListCustomer = _CustomerDAO.GetCustomerListByPage(1, 10);
            return View(ViewModel);
        }

        /// <summary>依條件搜尋客戶資料</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult SearchRecord(CustomerViewModels.CustomerViewModel ViewModel)
        {
            if (ViewModel._Customer != null)
            {
                CustomerDAO _CustomerDAO = new CustomerDAO();
                if (!string.IsNullOrEmpty(ViewModel._Customer.Account) ||
                    !string.IsNullOrEmpty(ViewModel._Customer.Member.NickName) ||
                      !string.IsNullOrEmpty(ViewModel._Customer.Member.Phone) ||
                      !string.IsNullOrEmpty(ViewModel._Customer.Member.LastName + ViewModel._Customer.Member.FirstName)
                    )
                {
                    ViewModel.ListCustomer = _CustomerDAO.SearchCustomerList(ViewModel._Customer);
                    return View("Mangement", ViewModel);
                }
                else
                {
                    return RedirectToAction("Mangement", "Customer");
                }
            }
            else
            {
                return View("Mangement", ViewModel);
            }
        }

        /// <summary>客戶資料細節</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult ShowCustomerDetail(CustomerViewModels.CustomerViewModel ViewModel)
        {
            if (ViewModel.CustomerSN > 0)
            {
                CustomerDAO _CustomerDAO = new CustomerDAO();
                ViewModel._Customer = _CustomerDAO.GetCustomerBySN(ViewModel.CustomerSN);
                ViewBag.AuditList = base.GetAuditItems(ViewModel._Customer);
                ViewBag.AdviserList = GetAdviserItems(ViewModel._Customer.HelperSN);
                EWalletDAO WalletDAO = new EWalletDAO();
                EWallet Wallet = WalletDAO.GetEWalletByCustomerSN(ViewModel._Customer.SN);
                ViewModel._EWallet = Wallet;
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>變更會員資料</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult CustomerModify(CustomerViewModels.CustomerViewModel ViewModel)
        {
            if (ViewModel._Customer != null)
            {
                if (ViewModel._Customer.Account != null)
                {
                    CustomerDAO _CustomerDAO = new CustomerDAO();
                    _CustomerDAO.UpdateCustomerByAccount(ViewModel._Customer);
                }
            }
            return RedirectToAction("Mangement", "Customer");
        }
    }
}
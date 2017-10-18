using ObjectBase;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class RegisterController : Controller
    {
        /// <summary>註冊畫面</summary>
        /// <param name="RegisterView"></param>
        /// <returns></returns>
        public ActionResult Index(WebApplication1.Models.RegisterViewModels.RegisterViewModel RegisterView)
        {
            ViewBag.GenderList = GetGenderItems();
            if (RegisterView.Account != null) {
                int Result = 0;
                CustomerDAO _CustomerDAO = new CustomerDAO();
                Customer _Customer = new Customer();
                _Customer.Account = RegisterView.Account;
                _Customer.PassWord = RegisterView.PassWord;
                _Customer.Member = new Member();
                _Customer.Member.BirthDay = RegisterView.BirthDay;
                _Customer.Member.Email = RegisterView.Account;
                _Customer.Member.FirstName = RegisterView.FirstName;
                _Customer.Member.LastName = RegisterView.LastName;
                _Customer.Member.NickName = RegisterView.NickName;
                _Customer.Member.Phone = RegisterView.Phone;
                _Customer.Member.ID = RegisterView.ID;
                _Customer.Member.HomeAddr = RegisterView.HomeAddr;
                _Customer.Member.Gender = RegisterView.Gender;              
                Result = _CustomerDAO.AddCustomer(_Customer);
                if (Result > 0) {
                    CommTool.MailData Data = new CommTool.MailData();
                    Data.RegistrySend(_Customer.Account,"註冊信件","感謝您的註冊"+
                        "<br/>您的帳號:" + _Customer.Account +
                        "<br/>您的密碼:" + _Customer.PassWord +
                        "<br/>請您以這組帳號登入系統");
                    return RedirectToAction("Messages", "Home", new {
                        info = Resources.Resource.Register_Success,
                        returnUrl = "../Login/LoginView"
                    });
                }
                else {
                    ModelState.AddModelError(string.Empty, Resources.Resource.Register_Error);
                    return View(RegisterView);
                }
            }
            else {              
                return View();
            }         
        }//end Index

        /// <summary>性別列舉</summary>
        /// <returns></returns>
        public List<SelectListItem> GetGenderItems() {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = Resources.Resource.Gender_Male, Value = Convert.ToInt32(GenderType.Male).ToString(), Selected = true });
            items.Add(new SelectListItem { Text = Resources.Resource.Gender_Fmale, Value = Convert.ToInt32(GenderType.Fmale).ToString() });
            return items;
        }//end  GetGenderItems           

	}
}
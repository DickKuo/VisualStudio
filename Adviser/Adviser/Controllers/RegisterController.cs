using Adviser.Models;
using ObjectBase;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adviser.Controllers
{
    public class RegisterController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.GenderList = GetGenderItems();
            return View();
        }//end Index

        /// <summary>註冊Action</summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult RegisterAdviser(Adviser.Models.ViewModels.RegisterViewModels.RegisterViewModel ViewModel) {            
            ObjectBase.Adviser _Adviser = new ObjectBase.Adviser();
            _Adviser.Account = ViewModel.Account;
            _Adviser.Address = ViewModel.Address;
            _Adviser.BirthDay = ViewModel.BirthDay;
            _Adviser.Email = ViewModel.Email;
            _Adviser.FirstName = ViewModel.FirstName;
            _Adviser.Gender = ViewModel.Gender;
            _Adviser.ID = ViewModel.ID;
            _Adviser.LastName = ViewModel.LastName;
            _Adviser.PassWord = ViewModel.PassWord;
            _Adviser.Phone = ViewModel.Phone;
            _Adviser.Remark = ViewModel.Remark;            
            AdviserDAO _AdviserDAO = new AdviserDAO();
            int SN = _AdviserDAO.AddAdviser(_Adviser);
            if (SN > 0) {
                CommTool.MailData _MailDAO = new CommTool.MailData();
                _MailDAO.RegistrySend(_Adviser.Email, "註冊信件", "感謝您的註冊" +
                         "<br/>您的帳號:" + _Adviser.Account +
                         "<br/>您的密碼:" + _Adviser.PassWord +
                         "<br/>請您以這組帳號登入系統");
            }
            return RedirectToAction("Index","Home");
        }//end RegisterAdviser

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
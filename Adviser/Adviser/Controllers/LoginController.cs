using Adviser.Helpers;
using Adviser.Models;
using Adviser.Models.Code;
using ObjectBase;
using System.Web.Mvc;

namespace Adviser.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>載入登入畫面</summary>
        /// <returns></returns>
        public ActionResult LoginView() {         
            return View();
        }//end LoginView
        
        /// <summary></summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public ActionResult LoginAction(Adviser.Models.ViewModels.LoginViewModels.LoginViewModel ViewModel) {
            ObjectBase.Adviser _Adviser = new ObjectBase.Adviser();
            _Adviser.Account = ViewModel.Account;
            _Adviser.PassWord = ViewModel.PassWord;
            AdviserDAO _AdviserDAO = new AdviserDAO();
            LoginInfo Info = new LoginInfo();
            Info.Adviser = _AdviserDAO.LoginCheckAdviser(_Adviser);
            if (Info.Adviser.SN > 0) {
                LoginHelper.SetSeesion(Info);
                return RedirectToAction("Index", "Home");
            }
            else {
                return RedirectToAction("LoginView", "Login");
            }
        }//end LoginAction

        /// <summary>登出</summary>
        /// <returns></returns>
        public ActionResult LogOut() {
            LoginHelper.LogOut();
            return RedirectToAction("LoginView", "Login");
        }//end LogOut
	}
}
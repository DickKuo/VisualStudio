using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApplication1.Code.DAO;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {

        public const string ErrorMessage = "登入錯誤";

        private class ControlerNames {
            public const string HomeControler = "Home";
            public const string EmployeeControler = "Home";
        }

        private class ActonNames {
            public const string HomeIndex = "Index";
            public const string IndexByAjax = "IndexByAjax";
        }


        //
        // GET: /Login/
        /// <summary>登入畫面 /// </summary>
        /// <returns></returns>
        public ActionResult LoginView()
        {
            return View();
        }


        /// <summary>
        /// 登入驗證
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoginView(EmployeeMgrViewModels.SysAdminManagerViewModel model)
        {
            LoginDAO logingDAO = new LoginDAO();
            if (MessageType.Sucess == logingDAO.Check(model.SysAd))
            {
                return RedirectToAction(ActonNames.HomeIndex, ControlerNames.HomeControler);
            }
            else
            {
                ModelState.AddModelError("","請輸入正確的帳號或密碼!!");                
                return View(model);               
            }
        }


        /// <summary>登出</summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            LoginHelper.LogOut();
            return RedirectToAction(ActonNames.HomeIndex, ControlerNames.HomeControler);
        }

             
	}
}
using System.Web.Mvc;
using WebApplication1.Code.DAO;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private class ControlerNames {
            public const string HomeControler = "Home";
            public const string EmployeeControler = "Home";
        }

        private class ActonNames {
            public const string HomeIndex = "Index";
            public const string IndexByAjax = "IndexByAjax";
        }

        /// <summary>登入畫面</summary>
        /// <returns></returns>
        public ActionResult LoginView()
        {
            return View();
        }

        /// <summary>登入驗證</summary>
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
                ModelState.AddModelError(string.Empty, Resources.Resource.Login_AccountError);                
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
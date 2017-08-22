using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BaseLoginController : Controller
    {
            
        /// <summary>
        /// 執行Action 之前會做的事情
        /// 驗證登入狀態
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {           
            if (!LoginHelper.CheckIsLogin()) {
                filterContext.Result = RedirectToAction("LoginView", "Login");
            }
            base.OnActionExecuting(filterContext);            
        } 
      
    }
}

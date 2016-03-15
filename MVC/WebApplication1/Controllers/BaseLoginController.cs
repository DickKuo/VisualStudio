using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Code.Helpers;

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
            if (!LoginHelper.CheckIsLogin())
            {
              filterContext.Result=  RedirectToAction("LoginView", "Login");
            }            
            base.OnActionExecuting(filterContext);            
        }


      
    }
}

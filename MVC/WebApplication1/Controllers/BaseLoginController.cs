using ObjectBase;
using System;
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
        private class Default {
            public const string LogPath = "\temp";
            public const string DateTimeFormat_yyyyMMddHHmmss = "yyyyMMddHHmmss";
        }    
                
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

        /// <summary>提示訊息轉址</summary>
        /// <param name="MessageStr"></param>
        /// <param name="ReturnURL"></param>
        /// <param name="_MessageType"></param>
        /// <returns></returns>
        protected ActionResult ReturnMessage(string MessageStr,string ReturnURL ,WebApplication1.Models.Code.BaseCode.MessageType _MessageType)
        {
            return RedirectToAction("Messages", "Home", new { MessageStr = MessageStr, ReturnURL = ReturnURL, MessageType = _MessageType });               
        }

        protected void Log(string Message) {
            System.IO.File.WriteAllText(Default.LogPath + DateTime.Now.ToString(Default.DateTimeFormat_yyyyMMddHHmmss) + ".txt", Message, System.Text.Encoding.UTF8);       
        }

        protected void Log(Exception ex) {
            System.IO.File.WriteAllText(Default.LogPath + DateTime.Now.ToString(Default.DateTimeFormat_yyyyMMddHHmmss) + ".txt", ex.Message, System.Text.Encoding.UTF8);   
        }

        protected string FTPCustomerDirectory() {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["FTPCustomerDirectory"].ToString();
        }

      
    }
     
}

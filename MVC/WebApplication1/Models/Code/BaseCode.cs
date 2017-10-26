using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Code
{
    public class BaseCode
    {

        /// <summary>
        /// 取得ClientIP
        /// </summary>
        /// <returns></returns>
        public static string GetUserIP() {
            string VisitorsIPAddr = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) {
                VisitorsIPAddr = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0) {
                VisitorsIPAddr = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }


        public enum MessageType : int
        {
            danger = 0,
            warning = 1,
            info = 2,
            success = 3,
        }

        public enum PageAction : int {
            None = 0,
            Add = 1,
            Edit = 2,
            Delete = 3,
            View =4 
        }
    }
}
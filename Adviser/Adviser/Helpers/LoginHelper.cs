using Adviser.Models.Code;
using System.Web;

namespace Adviser.Helpers
{
    public class LoginHelper
    {
        private class SessionNames
        {
            public const string LoginSession = "LoginSession";          //登入資訊的Session
        }

        public static bool CheckIsLogin()
        {
            if (HttpContext.Current.Session[SessionNames.LoginSession] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 將登入的使用者資訊放入Session中
        /// </summary>
        /// <param name="user"></param>
        public static void SetSeesion(LoginInfo Info)
        {
            HttpContext.Current.Session[SessionNames.LoginSession] = Info;
        }

        /// <summary>取得Session中的登入資訊</summary>
        /// <returns></returns>
        public static LoginInfo GetLoginInfo()
        {
            return (LoginInfo)HttpContext.Current.Session[SessionNames.LoginSession];
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void LogOut()
        {
            HttpContext.Current.Session[SessionNames.LoginSession] = null;
        }
    }
}
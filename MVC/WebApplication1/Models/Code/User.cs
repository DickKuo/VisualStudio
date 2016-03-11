using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Code
{

    /// <summary>使用者</summary>
    public class User
    {
        public string UserID { set; get; }

        public string UserName { set; get; }

        public string PassWord { set; get; }

        public string Email { set; get; }

        /// <summary>註冊日期</summary>
        public DateTime RegistrationDate { set; get; }

        /// <summary>是否啟用</summary>
        public bool IsEnable { set; get; }

        public string RoleID { set; get; }
                
    }
}
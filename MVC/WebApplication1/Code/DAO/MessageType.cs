using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Code.DAO
{
    public class MessageType
    {
        public const string Parameter = "Parameter Add Error";          //SQL 參數錯誤

        public const string Sucess = "Store Procedure Execute Sucess";  //預存執行成功

        public const string LogInFail = "Login Failed";                 //登入失敗

        public const string LoginSucess = "Login Sucess";               //登入成功
    }

        

}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1.Code.Helpers;
using WebApplication1.Models;


namespace WebApplication1.Code.DAO
{
    public class LoginDAO :DAOBase
    {
        protected class Parameter
        {
            public const string Account = "Account";
            public const string PassWord = "PassWord";
        }

        private class Sp
        {
            public const string LoginCheck = "LoginCheck";
        }
            
        private class DataRows {
            public const string UserID = "UserID";
            public const string UserName = "UserName";
            public const string PassWord = "PassWord";
            public const string Email = "Email";
            public const string RegistrationDate = "RegistrationDate";
            public const string RoleID = "RoleID";
            public const string IsEnable = "IsEnable";
        }

        private string LogPath ;       //Log檔的路徑
        
        /// <summary>初始化Login物件 </summary>      
        public LoginDAO()
        {             
            //WebConfig 取得
            //_DbIstance.ConnectiinString = System.Web.Configuration.WebConfigurationManager.AppSettings["ConnectionString"];
            LogPath = System.Web.Configuration.WebConfigurationManager.AppSettings["LogPath"];   
        }

        /// <summary>驗證登入使用者，成功後儲存Session </summary>
        /// <param name="sysamdin"></param>
        /// <returns>回傳錯誤碼</returns>
        public string Check(LoginInfo Info)
        {
            USP.AddParameter(Parameter.Account, Info.User.UserName);
            USP.AddParameter(Parameter.PassWord, Info.User.PassWord);
            DataTable dt = USP.ExeProcedureGetDataTable(Sp.LoginCheck);
            if (dt != null && dt.Rows.Count > 0)
            {   
                bool IsEnable = Convert.ToBoolean(dt.Rows[0][DataRows.IsEnable]);
                if (IsEnable)
                {
                    ObjectBase.CustomerDAO DAO = new ObjectBase.CustomerDAO();
                    Info.Customer = DAO.GetCustomerByAccount(Info.User.UserName);
                    LoginHelper.SetSeesion(Info);
                    return MessageType.Sucess;
                }
                else
                {
                    return MessageType.LogInFail;
                } 
            }
            else
            {
                LogDAO log = new LogDAO();
                log.LogFilePath = LogPath;
                log.LogMethod(MessageType.LogInFail);
                return MessageType.LogInFail;
            }
        }


        public override object Add<T>(T Obj)
        {
            throw new NotImplementedException();
        }

        public override object Delete<T>(T Obj)
        {            
            throw new NotImplementedException();
        }

        public override object Edit<T>(T Obj)
        {
            throw new NotImplementedException();
        }

        public override object Select<T>(T Obj)
        {
            throw new NotImplementedException();
        }

    }
}
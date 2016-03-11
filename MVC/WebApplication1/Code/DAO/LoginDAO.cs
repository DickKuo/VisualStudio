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
            public const string UserNameParameter = "UserName";
            public const string UserPWParameter = "PassWord";
        }

        private class Sp
        {
            public const string LoginCheck = "SysView";
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
            _DbIstance.ConnectiinString = System.Web.Configuration.WebConfigurationManager.AppSettings["ConnectionString"];
            LogPath = System.Web.Configuration.WebConfigurationManager.AppSettings["LogPath"];   
        }


        /// <summary>設定DB連線字串 /// </summary>
        /// <param name="Connectionstring"></param>
        public void SetConnection(string Connectionstring)
        {
           _DbIstance.ConnectiinString = Connectionstring;
        }


        /// <summary>驗證登入使用者，成功後儲存Session </summary>
        /// <param name="sysamdin"></param>
        /// <returns>回傳錯誤碼</returns>
        public string Check(LoginInfo Info)
        {
            base._DbIstance.AddParameter(Parameter.UserNameParameter, Info.User.UserName);
            base._DbIstance.AddParameter(Parameter.UserPWParameter, Info.User.PassWord);
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(Sp.LoginCheck);
            if (dt != null && dt.Rows.Count > 0)
            {
                bool IsEnable = Convert.ToBoolean(dt.Rows[0][DataRows.IsEnable]);
                if (IsEnable)
                {
                    WebApplication1.Models.Code.User NowUser = new Models.Code.User();
                    Info.User.UserID = dt.Rows[0][DataRows.UserID].ToString();
                    Info.User.Email = dt.Rows[0][DataRows.Email].ToString();
                    Info.User.RegistrationDate = Convert.ToDateTime(dt.Rows[0][DataRows.RegistrationDate]);
                    Info.User.IsEnable = IsEnable;
                    Info.User.RoleID = dt.Rows[0][DataRows.RoleID].ToString();
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
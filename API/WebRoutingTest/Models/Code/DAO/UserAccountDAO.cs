using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;
using WebApplication1.Models.Code;

namespace WebRoutingTest.Models.Code.DAO
{
    public class UserAccountDAO : DAOBase
    {

        public UserAccountDAO(string ConnectionString)
        {
            base.SetConnetionString(ConnectionString);
        }

        private class SP {
            public const string SpSelect = "SpUserAccountSelect";
            public const string SpAdd = "SpUserAccountAdd";
            public const string SpDelete = "SpUserAccountDelete";
            public const string SpEdit = "SpUserAccountEdit";
        }

        private class DataRows
        {
           
            public const string UserID = "UserID";
            public const string UserName = "UserName";
            public const string PassWord = "PassWord";
            public const string Email = "Email";
            public const string RegistrationDate = "RegistrationDate";
            public const string IsDone = "IsDone";
            public const string RoleID = "RoleID";
        }

        public override object Select<T>(T Obj)
        {
            Operation Oper = new Operation();
            List<User> UserList = new List<User>();
            string UserName = GlobalParameter.DefaultValues.Negative;
            if (Obj != null)
            {
                User _User = JsonConvert.DeserializeObject<User>(Obj.ToString());
                UserName = _User.UserName;
            }
            base._DbIstance.AddParameter(DataRows.UserName, UserName);
            base._DbIstance.AddParameter(DataRows.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SP.SpSelect);
            if (dt != null && dt.Rows.Count > 0)
            {                
               foreach(DataRow dr in dt.Rows)
               {
                   User _User = new User();
                   _User.UserID = dr[DataRows.UserID].ToString();
                   _User.UserName = dr[DataRows.UserName].ToString();
                   _User.PassWord = dr[DataRows.PassWord].ToString();
                   _User.Email = dr[DataRows.Email].ToString();
                   _User.RoleID = dr[DataRows.RoleID].ToString();
                   _User.RegistrationDate =Convert.ToDateTime(dr[DataRows.RegistrationDate]);
                   UserList.Add(_User);
               }
            }
            Oper.Obj = UserList;
            return Oper;
        }

        public override object Add<T>(T Obj)
        {
            Operation Oper = new Operation();
            User User = JsonConvert.DeserializeObject<User>(Obj.ToString());
            User.UserID = Guid.NewGuid().ToString();
            User.RegistrationDate = DateTime.Now;
            base._DbIstance.AddParameter(DataRows.UserID, User.UserID);
            base._DbIstance.AddParameter(DataRows.UserName, User.UserName);
            base._DbIstance.AddParameter(DataRows.PassWord, User.PassWord);
            base._DbIstance.AddParameter(DataRows.Email, User.Email);
            base._DbIstance.AddParameter(DataRows.RoleID, User.RoleID);
            base._DbIstance.AddParameter(DataRows.RegistrationDate, User.RegistrationDate);
            base._DbIstance.AddParameter(DataRows.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureNotQuery(SP.SpAdd);
            Oper.Obj = User;
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
            return Oper;
        }

        public override object Edit<T>(T Obj)
        {
            Operation Oper = new Operation();
            User User = JsonConvert.DeserializeObject<User>(Obj.ToString());
            base._DbIstance.AddParameter(DataRows.UserName, User.UserName);
            base._DbIstance.AddParameter(DataRows.PassWord, User.PassWord);
            base._DbIstance.AddParameter(DataRows.Email, User.Email);
            base._DbIstance.AddParameter(DataRows.RoleID, User.RoleID);
            base._DbIstance.AddParameter(DataRows.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureNotQuery(SP.SpEdit);
            Oper.Obj = User;
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
            return Oper;
        }
        
        public override object Delete<T>(T Obj)
        {
            Operation Oper = new Operation();
            User User = JsonConvert.DeserializeObject<User>(Obj.ToString());
            base._DbIstance.AddParameter(DataRows.UserID, User.UserID);
            base._DbIstance.AddParameter(DataRows.IsDone, 1, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureNotQuery(SP.SpDelete);
            Oper.Obj = User;
            string OutParameter = base._DbIstance.OutParameterValues[0].ToString();
            if (OutParameter == GlobalParameter.StoreProcedureResult.Error)
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Error;
            }
            else
            {
                Oper.ResultCode = WebApplication1.Models.Code.OperationResultCode.OperationCode.Success;
            }
            return Oper;
        }

                
    }
}
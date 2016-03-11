using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;
using WebApplication1.Models;

namespace WebRoutingTest.Models.Code.DAO
{
    public class EmployeeDAO : DAOBase
    {
        private class SP
        {
            public const string Select = "EmpView";
            public const string Add = "EmpAdd";
            public const string Edit = "EmpUpdate";
            public const string Delete = "EmpDel";
        }

        /// <summary>資料欄位名稱</summary>
        private class DataRows
        {
            public const string DepNo = "DepNo";
            public const string EmpNo = "EmpNo";
            public const string EmpFirstName = "EmpFirstName";
            public const string EmpLastName = "EmpLastName";
            public const string Tel = "Tel";
            public const string Addr = "Addr";
            public const string IsDone = "IsDone";
            public const string InintValue = "0";
        }

        public EmployeeDAO(string ConnectionString)
        {
            base.SetConnetionString(ConnectionString);
        }

        public override object Select<T>(T Obj)
        {
            Operation Oper = new Operation();
            Oper.ObjList = new List<object>();
            base._DbIstance.AddParameter(DataRows.EmpNo, GlobalParameter.DefaultValues.Negative);
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SP.Select);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Employee Emp = new Employee();
                    Emp.EmpNo = dr[DataRows.EmpNo].ToString();
                    Emp.EmpFirstName = dr[DataRows.EmpFirstName].ToString();
                    Emp.EmpLastName = dr[DataRows.EmpLastName].ToString();
                    Emp.Tel = dr[DataRows.Tel].ToString();
                    Emp.Addr = dr[DataRows.Addr].ToString();
                    Emp.DepNo = dr[DataRows.DepNo].ToString();
                    Oper.ObjList.Add(Emp);
                }
            }
            return Oper;
        }


        /// <summary>新增</summary>
        /// <param name="Oper"></param>
        /// <returns></returns>
        public override object Add<T>(T Obj)
        {
            Employee Emp = JsonConvert.DeserializeObject<Employee>(Obj.ToString());
            Operation Oper = new Operation();
            Emp.EmpNo = DateTime.Now.ToString(GlobalParameter.DefaultValues.DateTimeFormate);
            base._DbIstance.AddParameter(DataRows.EmpFirstName, Emp.EmpFirstName);
            base._DbIstance.AddParameter(DataRows.EmpLastName, Emp.EmpLastName);
            base._DbIstance.AddParameter(DataRows.Addr, Emp.Addr);
            base._DbIstance.AddParameter(DataRows.Tel, Emp.Tel);
            base._DbIstance.AddParameter(DataRows.DepNo, Emp.DepNo);
            base._DbIstance.AddParameter(DataRows.IsDone, DataRows.InintValue, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.AddParameter(DataRows.EmpNo, Emp.EmpNo, System.Data.SqlDbType.NVarChar, 20, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Add);
            Oper.Obj = Emp;
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


        /// <summary>刪除</summary>
        /// <param name="Oper"></param>
        /// <returns></returns>
        public override object Delete<T>(T Obj)
        {
            Employee Emp = JsonConvert.DeserializeObject<Employee>(Obj.ToString());            
            Operation Oper = new Operation();
            Oper.Obj = Emp;
            base._DbIstance.AddParameter(DataRows.EmpNo, Emp.EmpNo);
            base._DbIstance.AddParameter(DataRows.IsDone, DataRows.InintValue, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Delete);
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


        /// <summary>編輯</summary>
        /// <param name="Oper"></param>
        /// <returns></returns>
        public override object Edit<T>(T Obj)
        {
            Employee Emp = JsonConvert.DeserializeObject<Employee>(Obj.ToString());           
            Operation Oper = new Operation();
            Oper.Obj = Emp;
            base._DbIstance.AddParameter(DataRows.EmpNo, Emp.EmpNo);
            base._DbIstance.AddParameter(DataRows.EmpFirstName, Emp.EmpFirstName);
            base._DbIstance.AddParameter(DataRows.EmpLastName, Emp.EmpLastName);
            base._DbIstance.AddParameter(DataRows.Tel, Emp.Tel);
            base._DbIstance.AddParameter(DataRows.Addr, Emp.Addr);
            base._DbIstance.AddParameter(DataRows.IsDone, DataRows.InintValue, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Edit);
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
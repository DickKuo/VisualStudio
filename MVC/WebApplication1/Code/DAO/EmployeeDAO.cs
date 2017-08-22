using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Code.DAO
{
    public class EmployeeDAO : DAOBase
    { 
        /// <summary> StoreProcedure 的名稱 --名字修改統一修改這邊就可 </summary>
        private class SP {
            public const string ADD_EMP = "EmpAdd";
            public const string DELETE_EMP = "EmpDel";
            public const string EDIT_EMP = "EmpUpdate";
            public const string LIST_EMP = "EmpView";
        }

        /// <summary>資料欄位列 --這樣欄位修改只需要修改這邊就可以，下面方法都不需要改 </summary>
        private class DataRowName
        {
            public const string EmpNo = "EmpNo";
            public const string EmpFirstName = "EmpFirstName";
            public const string EmpLastName = "EmpLastName";
            public const string EmpAddr = "Addr";
            public const string EmpTel = "Tel";
            public const string IsDone = "IsDone";
            public const string InintValue = "0";
            public const string DepNo = "DepNo";
        }

        private class Default {
            public const string DepNo = "";
            public const string EmpFormat = "yyyyMMddHHmmss";
        }


        /// <summary> 新增員工  </summary>
        /// <param name="EMP"></param>
        /// <returns></returns>
        public string Create(Employee EMP)
        {            
            EMP.EmpNo = DateTime.Now.ToString(Default.EmpFormat);
            USP.AddParameter(DataRowName.EmpFirstName, EMP.EmpFirstName);
            USP.AddParameter(DataRowName.EmpLastName, EMP.EmpLastName);
            USP.AddParameter(DataRowName.EmpAddr, EMP.Addr);
            USP.AddParameter(DataRowName.EmpTel, EMP.Tel);
            USP.AddParameter(DataRowName.DepNo, Default.DepNo);
            USP.AddParameter(DataRowName.IsDone, DataRowName.InintValue, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            USP.AddParameter(DataRowName.EmpNo, EMP.EmpNo, System.Data.SqlDbType.NVarChar, 20, System.Data.ParameterDirection.Output);
            return USP.ExeProcedureHasResult(SP.ADD_EMP);            
        }


        /// <summary>編輯員工 </summary>
        /// <param name="EMP"></param>
        /// <returns></returns>
        public string Edit(Employee EMP)
        {
            USP.AddParameter(DataRowName.EmpNo, EMP.EmpNo);
            USP.AddParameter(DataRowName.EmpFirstName, EMP.EmpFirstName);
            USP.AddParameter(DataRowName.EmpLastName, EMP.EmpLastName);
            USP.AddParameter(DataRowName.EmpAddr, EMP.Addr);
            USP.AddParameter(DataRowName.EmpTel, EMP.Tel);
            USP.AddParameter(DataRowName.IsDone, DataRowName.InintValue, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            return USP.ExeProcedureHasResult(SP.EDIT_EMP);

        }


        /// <summary>查詢員工資料</summary>
        /// <param name="Emp">給Null時查詢全部員工</param>
        /// <returns></returns>
        public WebApplication1.Models.EmployeeMgrViewModels.EmployeeManamgerViewModel ViewEmployees(Employee Emp)
        {
            EmployeeMgrViewModels.EmployeeManamgerViewModel Model = new EmployeeMgrViewModels.EmployeeManamgerViewModel();
            Model.EmpList = new List<Employee>();
            string Tag = string.Empty;           
            Tag = Emp != null ? Emp.EmpNo.ToString() : "-1";
            USP.AddParameter(DataRowName.EmpNo, Tag);
            string Result = string.Empty;
            DataTable dt = USP.ExeProcedureGetDataTable(SP.LIST_EMP);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Employee emp = new Employee();
                    emp.EmpNo = dr[DataRowName.EmpNo].ToString();
                    emp.EmpFirstName = dr[DataRowName.EmpFirstName].ToString();
                    emp.EmpLastName = dr[DataRowName.EmpLastName].ToString();
                    emp.Tel = dr[DataRowName.EmpTel].ToString();
                    emp.Addr = dr[DataRowName.EmpAddr].ToString();
                    Model.EmpList.Add(emp);
                }
            }
            return Model;
        }


        /// <summary>刪除操作</summary>
        /// <param name="Emp"></param>
        /// <returns></returns>
        public string Delete(Employee Emp)
        {
            USP.AddParameter(DataRowName.EmpNo, Emp.EmpNo);
            USP.AddParameter(DataRowName.IsDone, DataRowName.InintValue, System.Data.SqlDbType.NVarChar, 1, System.Data.ParameterDirection.Output);
            USP.ExeProcedureHasResult(SP.DELETE_EMP);
            return USP.Result;
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
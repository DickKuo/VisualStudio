using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1.Code.DAO;
using WebApplication1.Models;
using WebApplication1.Models.Code;

namespace WebRoutingTest.Models.Code.DAO
{
    public class DepartmentDAO : DAOBase
    {
        private class SP
        {
            public const string Select = "SpSelectDepartment";
            public const string Add = "SpAddDepartment";
            public const string Edit = "SpEditDepartment";
            public const string Delete = "SpDeleteDepartment";
        }

        /// <summary>資料欄位名稱</summary>
        private class DataRows
        {
            public const string DepNo = "DepNo";
            public const string Name = "Name";
            public const string EmpCount = "EmpCount";
            public const string IsSuccess = "IsSuccess";
            public const string Remark = "Remark";
        }

        private class EmpTypes
        {
            public const string FullTime = "全職";
            public const string PTime = "臨時工";
            public const string Stagnation = "駐點";
        }


        public DepartmentDAO(string ConnectionString)
        {
            base.SetConnetionString(ConnectionString);
        }




        public override object Select<T>(T Obj)
        {
            Operation Oper = new Operation();
            Oper.ObjList = new List<object>();
            DataTable dt = base._DbIstance.ExeProcedureGetDataTable(SP.Select);


            FullTimeDAO DAo = new FullTimeDAO(base._DbIstance.ConnectiinString);
            Operation EmpObj = DAo.Select(Oper.Obj) as Operation;
            PTimeDAO PTDAo = new PTimeDAO(base._DbIstance.ConnectiinString);
            Operation PTEmpObj = PTDAo.Select(Oper.Obj) as Operation;
            StagnationDAO STDAo = new StagnationDAO(base._DbIstance.ConnectiinString);
            Operation STEmpObj = STDAo.Select(Oper.Obj) as Operation;


            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Department Dep = new Department();
                    Dep.DepNo = dr[DataRows.DepNo].ToString();
                    Dep.Name = dr[DataRows.Name].ToString();
                    //Dep.EmpList = new List<Employee>();
                    List<Employee> Emps = new List<Employee>();

                    Emps.AddRange(GetList(EmpObj, EmpTypes.FullTime));
                    Emps.AddRange(GetList(PTEmpObj, EmpTypes.PTime));
                    Emps.AddRange(GetList(STEmpObj, EmpTypes.Stagnation));
                    //Dep.EmpList = Emps.Where(X => X.DepNo == Dep.DepNo).ToList();
                    Oper.ObjList.Add(Dep);

                }
            }
            return Oper;
        }


        private List<Employee> GetList(Operation Obj,string EmpType)
        {
            List<Employee> Emps = new List<Employee>();
            foreach (var item in Obj.ObjList)
            {
                Employee emp = item as Employee;
                //emp.EmpType = EmpType;
                Emps.Add(emp);
            }
            return Emps;
        }


        /// <summary>新增</summary>
        /// <param name="Oper"></param>
        /// <returns></returns>
        public override object Add<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            Operation Oper = new Operation();
            //Oper.Obj = Obj;         
            Dep.DepNo = DateTime.Now.ToString(GlobalParameter.DefaultValues.DateTimeFormate);
            Oper.Obj = Dep;
            base._DbIstance.AddParameter(DataRows.DepNo, Dep.DepNo);
            base._DbIstance.AddParameter(DataRows.Name, Dep.Name);
            base._DbIstance.AddParameter(DataRows.Remark, Dep.Remark);
            base._DbIstance.AddParameter(DataRows.IsSuccess, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            base._DbIstance.ExeProcedureHasResult(SP.Add);
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
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = Obj;
            base._DbIstance.AddParameter(DataRows.DepNo, Dep.DepNo);
            base._DbIstance.AddParameter(DataRows.IsSuccess, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
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
            Department Dep =JsonConvert.DeserializeObject<Department>(Obj.ToString());
            Operation Oper = new Operation();
            Oper.Obj = Obj;
            base._DbIstance.AddParameter(DataRows.DepNo, Dep.DepNo);
            base._DbIstance.AddParameter(DataRows.Name, Dep.Name);
            //base._DbIstance.AddParameter(DataRows.EmpCount, Dep.EmpCount);
            base._DbIstance.AddParameter(DataRows.IsSuccess, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
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
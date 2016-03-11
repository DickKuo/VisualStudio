using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using WebApplication1.Models;

namespace WebRoutingTest.Models.Code.DAO
{
    public class DAOSPDepartment : DAOBranchBase
    {
        
        private UseStoreProcedure StoreProcedure;


        private class SP {
            public const string GetDepartment = "SpGetDepartment";
            public const string GetAllEmployeeNoDepartment = "SpGetAllEmployeeNoDepartment";
            public const string EditDepartment = "SpEditDepartment";
            public const string DeleteDepartmentByDepNo = "SpDeleteDepartmentByDepNo";
            public const string AddDepartment = "SpAddDepartment";
            public const string AddEmployeeByDepNo = "SpAddEmployeeByDepNo";
            public const string DeleteDepartmentEmp = "SpDeleteDepartmentEmp";
        }
        

        private class DepDataRows {
            public const string DepNo = "DepNo";
            public const string Name = "Name";
            public const string Remark = "Remark";
            public const string Emps = "Emps";
        }


        private class EmpDataRows {
            public const string EmpNo = "EmpNo";
            public const string EmpFirstName = "EmpFirstName";
            public const string EmpLastName = "EmpLastName";            
            public const string Tel = "Tel";
            public const string Addr = "Addr";
            public const string DepNo = "DepNo";
        }


        private class Default {
            public const string DepNo = "-1";
            public const string IsDone = "IsDone";
            public const string IsSuccess = "IsSuccess";
            public const string KeyFormat = "yyyyMMddHHmmss";
        }


        public DAOSPDepartment()
        {
            StoreProcedure = new UseStoreProcedure();
            StoreProcedure.ConnectiinString = System.Web.Configuration.WebConfigurationManager.AppSettings["ConnectionString"]; ;
        }

        /// <summary>取得所有未配置部門的員工</summary>
        /// <returns></returns>
        public override object GetAllEmployeeNoDepartment()
        {
            JsonObj Jobj = new JsonObj();
            List<Employee> EmpList = new List<Employee>();
            StoreProcedure.AddParameter(DepDataRows.DepNo, Default.DepNo);
            StoreProcedure.AddParameter(Default.IsDone, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            DataTable ResultTable= StoreProcedure.ExeProcedureGetDataTable(SP.GetAllEmployeeNoDepartment);
            if (ResultTable != null && ResultTable.Rows.Count > 0)
            {
                foreach (DataRow dr in ResultTable.Rows)
                {
                    Employee Emp = new Employee();
                    Emp.EmpNo = dr[EmpDataRows.EmpNo].ToString();
                    Emp.EmpFirstName = dr[EmpDataRows.EmpFirstName].ToString();
                    Emp.EmpLastName = dr[EmpDataRows.EmpLastName].ToString();
                    Emp.Tel = dr[EmpDataRows.Tel].ToString();
                    Emp.Addr = dr[EmpDataRows.Addr].ToString();
                    Emp.DepNo = dr[EmpDataRows.DepNo].ToString();
                    EmpList.Add(Emp);
                }
            }
            Jobj.JsonString = JsonConvert.SerializeObject(EmpList);
            return Jobj;
        }



        /// <summary>取得指定部門所屬員工</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object GetALLEmployeeByDepNo<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            JsonObj Jobj = new JsonObj();
            List<Employee> EmpList = new List<Employee>();
            StoreProcedure.AddParameter(DepDataRows.DepNo, Dep.DepNo);
            StoreProcedure.AddParameter(Default.IsDone, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            DataTable ResultTable = StoreProcedure.ExeProcedureGetDataTable(SP.GetAllEmployeeNoDepartment);
            if (ResultTable != null && ResultTable.Rows.Count > 0)
            {
                foreach (DataRow dr in ResultTable.Rows)
                {
                    Employee Emp = new Employee();
                    Emp.EmpNo = dr[EmpDataRows.EmpNo].ToString();
                    Emp.EmpFirstName = dr[EmpDataRows.EmpFirstName].ToString();
                    Emp.EmpLastName = dr[EmpDataRows.EmpLastName].ToString();
                    Emp.Tel = dr[EmpDataRows.Tel].ToString();
                    Emp.Addr = dr[EmpDataRows.Addr].ToString();
                    Emp.DepNo = dr[EmpDataRows.DepNo].ToString();
                    EmpList.Add(Emp);
                }
            }
            Jobj.JsonString = JsonConvert.SerializeObject(EmpList);
            return Jobj;
        }


        /// <summary>編輯部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object EditDepartment<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            JsonObj Jobj = new JsonObj();
            if (string.IsNullOrEmpty(Dep.Remark))
            {
                Dep.Remark = string.Empty;
            }
            StoreProcedure.AddParameter(DepDataRows.DepNo, Dep.DepNo);
            StoreProcedure.AddParameter(DepDataRows.Name, Dep.Name);
            StoreProcedure.AddParameter(DepDataRows.Remark, Dep.Remark);            
            StoreProcedure.AddParameter(Default.IsDone, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            StoreProcedure.ExeProcedureHasResult(SP.EditDepartment);
            return Jobj;
        }


        /// <summary> 取得所有部門 </summary>
        /// <returns></returns>
        public override object GetALLDepartment()
        {
            JsonObj Jobj = new JsonObj();
            List<Department> DepList = new List<Department>();
            StoreProcedure.AddParameter(DepDataRows.DepNo, Default.DepNo);
            StoreProcedure.AddParameter(Default.IsDone, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            DataTable ResultTable= StoreProcedure.ExeProcedureGetDataTable(SP.GetDepartment);
            if (ResultTable != null && ResultTable.Rows.Count > 0)
            { 
                
                foreach(DataRow dr in ResultTable.Rows)
                {
                    Department Dep = new Department();
                    Dep.DepNo = dr[DepDataRows.DepNo].ToString();
                    Dep.Name = dr[DepDataRows.Name].ToString();
                    Dep.Remark = dr[DepDataRows.Remark].ToString();
                    DepList.Add(Dep);
                }
            }
            Jobj.JsonString = JsonConvert.SerializeObject(DepList);
            return Jobj;
        }


        /// <summary>刪除指定部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object DeleteDepartmentByDepNo<T>(T Obj)
        {
            JsonObj Jobj = new JsonObj();
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            StoreProcedure.AddParameter(DepDataRows.DepNo, Dep.DepNo);
            StoreProcedure.AddParameter(Default.IsDone, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            StoreProcedure.ExeProcedureHasResult(SP.DeleteDepartmentByDepNo);
            return Jobj;
        }


        /// <summary>新增部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object AddDepartment<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            Dep.DepNo = DateTime.Now.ToString(Default.KeyFormat);
            JsonObj Jobj = new JsonObj();
            StoreProcedure.AddParameter(DepDataRows.DepNo, Dep.DepNo);
            StoreProcedure.AddParameter(DepDataRows.Name, Dep.Name);
            StoreProcedure.AddParameter(DepDataRows.Remark, Dep.Remark);
            StoreProcedure.AddParameter(Default.IsSuccess, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
            StoreProcedure.ExeProcedureHasResult(SP.AddDepartment);
            Jobj.JsonString = JsonConvert.SerializeObject(Dep);
            return Jobj;
        }


        //public override object GetDepEmployeeSlefAndNoDep<T>(T Obj)
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>員工加入所屬部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object AddEmployeeByDepNo<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            JsonObj Jobj = new JsonObj();
            if (Dep.Remark != null)
            {
                StoreProcedure.AddParameter(DepDataRows.DepNo, Dep.DepNo);
                StoreProcedure.AddParameter(DepDataRows.Emps, Dep.Remark);
                StoreProcedure.AddParameter(Default.IsDone, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
                StoreProcedure.ExeProcedureHasResult(SP.AddEmployeeByDepNo);
            }
            return Jobj;
        }


        /// <summary>移除部門的員工</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object DeleteDepartmentEmp<T>(T Obj)
        {
            JsonObj Json = new JsonObj();
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            if (Dep.Remark != null)
            {
                StoreProcedure.AddParameter(DepDataRows.DepNo, Dep.DepNo);
                StoreProcedure.AddParameter(DepDataRows.Emps, Dep.Remark);
                StoreProcedure.AddParameter(Default.IsDone, 0, System.Data.SqlDbType.Int, 1, System.Data.ParameterDirection.Output);
                StoreProcedure.ExeProcedureHasResult(SP.DeleteDepartmentEmp);
            }
            return Json;
        }

        
    }
}
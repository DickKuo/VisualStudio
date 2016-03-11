using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebRoutingTest.Models.Code.DAO
{
    public class DAOLinqDepartment : DAOBranchBase
    {
        private LinqDepartmentDataContext DB;

        private class Default
        {
            public const string DepNoTimeFormat = "yyyyMMddHHmmss";
        }

        public DAOLinqDepartment()
        {
            DB = new LinqDepartmentDataContext();
        }

        /// <summary>取得所有未配置部門的員工</summary>
        /// <returns></returns>
        public override object GetAllEmployeeNoDepartment()
        {
            JsonObj Json = new JsonObj();
            var Emps = from v in DB.Employee
                       where v.DepNo == null | v.DepNo == string.Empty
                       select v;
            Json.JsonString = JsonConvert.SerializeObject(Emps.ToList());
            return Json;
        }


        /// <summary>取得指定部門所屬員工</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object GetALLEmployeeByDepNo<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            JsonObj Json = new JsonObj();
            var Emps = from v in DB.Employee
                       where v.DepNo == Dep.DepNo
                       select v;
            Json.JsonString = JsonConvert.SerializeObject(Emps.ToList());
            return Json;
        }

        /// <summary> 取得所有部門 </summary>
        /// <returns></returns>
        public override object GetALLDepartment()
        {
            JsonObj Json = new JsonObj();
            var AllDepartment = from v in DB.Department
                                select v;
            try
            {
                Json.JsonString = JsonConvert.SerializeObject(AllDepartment.ToList());                
            }
            catch (Exception ex)
            { 
            
            }
            return Json;            
        }

        /// <summary>編輯部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object EditDepartment<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            JsonObj Json = new JsonObj();
            var ThisDep = from v in DB.Department
                          where v.DepNo == Dep.DepNo
                          select v;
            ThisDep.ToList()[0].Name = Dep.Name;
            ThisDep.ToList()[0].Remark = Dep.Remark;
            DB.SubmitChanges();
            return Json;
        }

        /// <summary>新增部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object AddDepartment<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            Models.Department Department = new Models.Department();
            Department.DepNo = DateTime.Now.ToString(Default.DepNoTimeFormat);
            Department.Name = Dep.Name;
            Department.Remark = Dep.Remark;
            JsonObj Json = new JsonObj();
            Json.JsonString = JsonConvert.SerializeObject(Department);
            try
            {
                DB.Department.InsertOnSubmit(Department);
                DB.SubmitChanges();
            }
            catch (Exception ex)
            { 
            }
            return Json;
        }


        /// <summary>刪除指定部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object DeleteDepartmentByDepNo<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            JsonObj Json = new JsonObj();
            Models.Department Department = new Models.Department();
            var Deps=from v in  DB.Department
                     where v.DepNo==Dep.DepNo
                     select v;            
            try
            {
                DB.Department.DeleteAllOnSubmit(Deps);
                DB.SubmitChanges();
            }
            catch (Exception ex)
            { 
            }
            Json.JsonString = JsonConvert.SerializeObject(Dep);
            return Json;
        }



        /// <summary>員工加入所屬部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object AddEmployeeByDepNo<T>(T Obj)
        {
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            string[] sp = new string[] { };
            if (Dep.Remark != null)
            {
                sp = Dep.Remark.Split(',');
            }
            JsonObj Json = new JsonObj();
            var AllEmplyee = from v in DB.Employee
                             where sp.Contains(v.EmpNo)
                             select v;

            foreach(var item in AllEmplyee)
            {
                item.DepNo = Dep.DepNo;
            }
            DB.SubmitChanges();
            return Json;   
        }


        /// <summary>移除部門的員工</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public override object DeleteDepartmentEmp<T>(T Obj)
        {
            JsonObj Json = new JsonObj();
            Department Dep = JsonConvert.DeserializeObject<Department>(Obj.ToString());
            Json.JsonString = Obj.ToString();
            if (Dep.Remark != null)
            {
                string[] sp = new string[] { };
                sp = Dep.Remark.Split(',');
                var AllEmplyee = from v in DB.Employee
                                 where sp.Contains(v.EmpNo)
                                 select v;

                foreach (var item in AllEmplyee)
                {
                    item.DepNo = string.Empty;
                }
                DB.SubmitChanges();
            }
            return Json;   
        }

        
    }
}
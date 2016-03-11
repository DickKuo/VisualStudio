using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRoutingTest.Models.Code.DAO
{
    public abstract class DAOBranchBase
    {

        /// <summary> 取得所有部門 </summary>
        /// <returns></returns>
        public abstract object GetALLDepartment();

        /// <summary>取得指定部門所屬員工</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public abstract object GetALLEmployeeByDepNo<T>(T Obj);


        /// <summary>取得所有未配置部門的員工</summary>
        /// <returns></returns>
        public abstract object GetAllEmployeeNoDepartment();

        /// <summary>新增部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public abstract object AddDepartment<T>(T Obj);

        /// <summary>編輯部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public abstract object EditDepartment<T>(T Obj);


        /// <summary>刪除指定部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public abstract object DeleteDepartmentByDepNo<T>(T Obj);


        ///// <summary>
        ///// 取得指定部門的員工及未配置部門的員工
        ///// </summary>
        ///// <returns></returns>
        //public abstract object GetDepEmployeeSlefAndNoDep<T>(T Obj);

        /// <summary>員工加入所屬部門</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public abstract object AddEmployeeByDepNo<T>(T Obj);


        public abstract object DeleteDepartmentEmp<T>(T Obj);
    }
}
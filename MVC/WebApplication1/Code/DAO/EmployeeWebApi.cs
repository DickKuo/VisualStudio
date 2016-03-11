using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Code.DAO
{
    public class EmployeeWebApi : WebApi
    {
        public EmployeeWebApi()
        {
            this.ModelName = "Employee";
        }
        
        
        public string View = "ViewAll";

        public string Delete = "DeleteEmployee";

        public string Add = "AddEmployee";

        public string Eidt = "EditEmployee";

        
        /// <summary>編輯員工資訊</summary>
        /// <param name="Emp"></param>
        /// <returns></returns>
        public string EditEmployee(Employee Emp)
        {
            string Str = string.Format("{0}/{1}/{2}", this._uri, this.ModelName, Eidt);
            ApiOperation Operation = new ApiOperation();
            Operation.Uri = Str;
            Operation.obj = Emp;
            return base.ApiOperation(Operation);
        }


        /// <summary>新增員工資訊</summary>
        /// <param name="Emp"></param>
        /// <returns></returns>
        public Employee AddEmployee(Employee Emp)
        {
            string Str = string.Format("{0}/{1}/{2}", this._uri, this.ModelName, Add);
            ApiOperation Operation = new ApiOperation();
            Operation.Uri = Str;
            Operation.obj = Emp;
            string temp = base.ApiOperation(Operation);       
            Employee resultemp = JsonConvert.DeserializeObject<Employee>(temp);
            return resultemp;
        }


        /// <summary> 刪除員工</summary>
        /// <param name="Emp"></param>
        /// <returns></returns>
        public string DeleteEmployee(Employee Emp)
        {
            string Str = string.Format("{0}/{1}/{2}", this._uri, this.ModelName, Delete);
            ApiOperation Operation = new ApiOperation();
            Operation.Uri = Str;
            Operation.obj = Emp;
            return base.ApiOperation(Operation);
        }
    }

}
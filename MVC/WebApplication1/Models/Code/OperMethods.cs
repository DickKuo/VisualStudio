using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRoutingTest.Models.Code
{
    public class OperMethods
    {
        public enum OperMethod
        {
            GetALLDepartment = 0,
            GetALLEmployeeByDepNo = 1,
            GetAllEmployeeNoDepartment = 2,
            AddDepartment = 3,
            EditDepartment = 4,
            DeleteDepartmentByDepNo = 5,
            GetDepEmployeeSlefAndNoDep = 6,
            AddEmployeeByDepNo=7,
            DeleteDepartmentEmp = 8
        }
    }

}
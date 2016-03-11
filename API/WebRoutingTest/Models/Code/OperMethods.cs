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
            GetALlEmployeeNoDepartment = 2,
            AddDepartment = 3,
            EditDepartment = 4,
            DeleteDepartmentByDepNo = 5,
            GetDepEmployeeSlefAndNoDep = 6
        }
    }

}
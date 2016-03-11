  using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Models.Code;

namespace WebRoutingTest.Models.Code
{
    public class Department
    {

        public string DepNo { set; get; }
        
        public string Name { set; get; }

        public string Remark { set; get; }


        /// <summary>所屬員工清單</summary>
        public List<Employee> EmpList { set; get; }
        
    }
}
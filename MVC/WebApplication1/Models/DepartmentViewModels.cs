using System.Collections.Generic;
using WebRoutingTest.Models.Code;

namespace WebApplication1.Models
{
    public class DepartmentViewModels
    {
        public class DepartmentViewModel
        {

            public Department Dep { set; get; }

            public List<Department> DepList { set; get; }

            public string DepNo { set; get; }

            public string QueryString { set; get; }

        }
    }
}
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class EmployeeMgrViewModels
    {
        public class EmployeeManamgerViewModel{

            public  Employee  Emp {get;set;}
            public List<Employee> EmpList { set; get; }
            
            public string EmployeeJson { set; get; }

            public string EmployeeType { set; get; }

            public string Extend { set; get; }

            private List<string> _PhotoFileNames = new List<string>();

            public List<string> PhotoFileNames { get { return this._PhotoFileNames; } set { this._PhotoFileNames = value; } }

            public string ImageRemark { set; get; }
        }


        public class SysAdminManagerViewModel {
            public LoginInfo SysAd { set; get; }

            public List<LoginInfo> SysAdminList { set; get; }

            public string ErrorMessage { set; get; }
        }
        
    }
}
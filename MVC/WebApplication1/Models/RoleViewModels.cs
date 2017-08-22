using System.Collections.Generic;
using WebApplication1.Models.Code;

namespace WebApplication1.Models
{
    public class RoleViewModels
    {
        public class RoleViewModel
        {
            public Role Role { set; get; }

            private List<Role> _roleList = new List<Role>();

            public List<Role> RoleList { get { return _roleList; } set{_roleList=value;} }


            private List<RolesMenu> _rolemenu = new List<RolesMenu>();

            public List<RolesMenu> RoleMenu { get { return _rolemenu; } set { _rolemenu = value; } }

            public string JsonString { set; get; }
        
        }
    }
}
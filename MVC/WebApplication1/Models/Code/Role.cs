using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Code
{
    /// <summary>權限角色</summary>
    public class Role
    {

        public string RoleNo { set; get; }

        public string RoleID { set; get; }

        public string Name { set; get; }

        public bool IsEnable { set; get; }

        private List<RolesMenu> _RolesMenu = new List<RolesMenu>();

        public List<RolesMenu> RoleMenuList { get { return _RolesMenu; } set { _RolesMenu = value; } }
    }
}
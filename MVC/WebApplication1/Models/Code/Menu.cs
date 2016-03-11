using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApplication1.Code.DAO
{
    public class Menu
    {
        public string MenuNo { set; get; }
        
        public string Url { set; get; }

        public string Name { set; get; }

        public string ParentNo { set; get; }

        public bool IsEnable { set; get; }

        public int MenuOrder { set; get; }

        public List<Menu> MenuList { set; get; }
    }
  
}
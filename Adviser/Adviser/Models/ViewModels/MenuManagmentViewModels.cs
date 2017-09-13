using Adviser.Models.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adviser.Models.ViewModels {
    public class MenuManagmentViewModels {
        
        public class MenuViewModel {
            public Menu MainMenu { set; get; }

            private List<Menu> _MenuList = new List<Menu>();

            public List<Menu> MenuList { get { return _MenuList; } set { _MenuList = value; } }

            public string JsonString { set; get; }
        }
    }
}
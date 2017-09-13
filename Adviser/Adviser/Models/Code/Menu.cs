using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adviser.Models.Code {
    public class Menu {
        public string MenuNo { set; get; }

        public string Url { set; get; }

        public string Name { set; get; }

        public string ParentNo { set; get; }

        public int Seq { set; get; }

        public int Permission { set; get; }

        public List<Menu> MenuList { set; get; }
    }
}
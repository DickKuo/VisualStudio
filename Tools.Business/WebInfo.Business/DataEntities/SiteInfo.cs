using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebInfo.Business.DataEntities
{
    public class SiteInfo
    {
        public string Author { set; get; }
        public DateTime PostDate { set; get; }
        public string Address { set; get; }
        public string Title { set; get; }
        public string Context { set; get; }
        public StringBuilder Push = new StringBuilder();
        public List<string> PushList = new List<string>();
    }
}

using System.Collections.Generic;
using System.Text;
using System;

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
        public bool IsDuty = false;
        public List<string> HissList = new List<string>();
    }
}
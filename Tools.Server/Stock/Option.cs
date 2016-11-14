using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock {
    public class Option {

        public string OP { set; get; }
        
        public double buy {set;get;}

        public double sell { set; get; }

        public double clinch { set; get; }

        public double Change { set; get; }

        public double Open { set; get; }

        public double Total { set; get; }

        public DateTime Time { set; get; }

        public string Contract { set; get; }

        public string Mom { set; get; }
    }
}

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

        public string Time { set; get; }

        public string Contract { set; get; }

        /// <summary>成交量</summary>
        public int Volume { set; get; }

        /// <summary>未沖銷契約數</summary>
        public int NumberOfContracts { set; get; }

        /// <summary>類別</summary>
        public string DueMonth { set; get; }

    }
}

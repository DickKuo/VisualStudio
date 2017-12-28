using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock {
    public class OptionDAO : BaseData {

        private class SP {
            public const string StoreToHistory = "StoreToHistory";
        }

        /// <summary>儲存歷史資料</summary>
        public int StoreToHistory() {
             return  USP.ExeProcedureHasResultReturnCode(SP.StoreToHistory);
        }

        
    }
}

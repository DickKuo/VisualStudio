using Stock;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebApplication1.Models {
    public class ReportViewModels {

        public class TradeRecordReportModel {

            public int MaxPage { set; get; }

            public DateTime BeginTime { set; get; }

            public DateTime EndTime { set; get; }

            public List<TradeRecord> _TradeRecord { set; get; }

            public int Page { set; get; }

            public decimal Total { set; get; }

            public int Permission { set; get; }
        }

        public class FormatTableViewModel {
            public DataTable dt { set; get; }
        }
    }
}
using Adviser.Models.ViewModels;
using ObjectBase;
using Stock;
using System;
using System.Collections.Generic;
using System.Data;

namespace Adviser.Models {
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

        public class BounsViewModel : BaseViewModel {

            public BounsViewModel() {
                ListBouns = new List<Bouns>();
            }

            public List<Bouns> ListBouns { set; get; }
        }

        public class WeekPointViewModel {

            public int Year { set; get; }

            public List<WeekPoint> ListWeekPoint { set; get; }
        }
        
    }
}